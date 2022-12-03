using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : CombatStatus, IDamgeable
{
    public EnemyStatus Stat;
    public EnemyType Type;
    public ElementType Element;

    public float Timer;

    public STATE State;
    public enum STATE
    {
        IDLE,
        PATROL,
        CHASE,
        RUNAWAY,
        SLEEP
    }

    public LayerMask Mask = -1;
    public GameObject target;
    public GameObject Bullet;
    public Vector3 centerPosition;
    public bool isHitted = false;
    public bool runChance = true;
    public bool isAttacked = false;
    public int shootCount;

    public NavMeshAgent MyAgent;
    public Animator Anim;
    private Rigidbody rigid;
    private BoxCollider col;

    public float DealtDamage;

    float genTerm; //현재 묻어있는 속성을 TextMeshPro로 나타내는 부분.

    private Shield shield;
    [SerializeField] private bool setShield;

    //QuestObject questObject;//퀘스트 오브젝트 선언 위치 변경

    // Start is called before the first frame update
    void Start()
    {
        Stat = new EnemyStatus();
        Stat = Stat.SetEnemyStatus(Type, Element);

        this.MyElement = Stat.element;
        centerPosition = this.transform.position;

        target = null;
        shootCount = 0;
        
        Timer = 0.0f;

        State = STATE.IDLE;

        MyAgent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();

        UI_EnemyHp.EnemyHps.hpObjects.Add(Instantiate(UI_Control.Inst.EnemyHp.getPrefab(true), GameObject.Find("UI").transform.GetChild(1)));
        UI_EnemyHp.EnemyHps.ShieldObjects.Add(Instantiate(UI_Control.Inst.EnemyHp.getPrefab(true), GameObject.Find("UI").transform.GetChild(1)));
        UI_EnemyHp.EnemyHps.EnemyObjects.Add(this.gameObject);


        shield = this.gameObject.transform.Find("Shield").GetComponent<Shield>();
        setShield = false;
        //questObject = GameObject.Find("GameManager").GetComponent<QuestObject>();//퀘스트 오브젝트 받아오는 위치 변경
    }

    // Update is called once per frame
    void Update()
    {
        //현재 묻어있는 속성을 TextMeshPro로 나타내는 부분.
        this.genTerm += Time.deltaTime;
        if (genTerm >= 0.5f)
        {
            UI_Control.Inst.ElementStateGen(this.gameObject, this.genTerm);
            genTerm = 0.0f;
        }
        //현재 묻어있는 속성을 TextMeshPro로 나타내는 부분.

        rigid.velocity = Vector3.zero;
        MyAgent.speed = Stat.moveSpeed * SpeedMultiply;

        findPlayer(Stat.sight);

        if(State != STATE.CHASE && State != STATE.RUNAWAY)
            stateChange();

        if(Stat.hp % Stat.maxHp == 1)
            runChance = true;

        if(runChance && Stat.hp % Stat.maxHp <= 0.34)
        {
            runChance = false;
            isHitted = false;
            State = STATE.RUNAWAY;
        }

        if(isHitted)
        {
            State = STATE.CHASE;
        }

        if (setShield)
        {
            shield.SetActive(true);
            col.enabled = false;
            //setShield = false;
        }
    }
    
    private void stateChange()
    {
        Timer += Time.deltaTime;

        if(Timer >= 5.0f)
        {
            if(State == STATE.IDLE)
                State = STATE.PATROL;
            else
                State = STATE.IDLE;
            Timer = 0.0f;
        }
    }

    private void findPlayer(float sight)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sight, 
                                                    Mask, QueryTriggerInteraction.Ignore);

        if(colliders.Length > 0)
        {    
            target = colliders[0].gameObject;
            State = STATE.CHASE;
        }
    }

    private void Died()
    {
        //Destroy(gameObject);
        StartCoroutine(this.transform.Find("Cube_001").GetComponent<Dissolve>().Act(this.gameObject));
    }

    public void MeleeAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Stat.attackRange, 
                                                    Mask, QueryTriggerInteraction.Ignore);
            
        if(colliders.Length > 0)
        {
            setEnemyElement(colliders[0].GetComponent<PlayerContorl>().MyElement);
            colliders[0].GetComponent<PlayerContorl>().TakeElementHit(Stat.damage, Stat.element);
        }
    }

    public void RangedAttack()
    {
        this.Bullet.GetComponent<BulletEne>().myEle = Stat.element;
        this.Bullet.GetComponent<BulletEne>().myDmg = Stat.damage;
        Vector3 myVec = transform.position;
        //myVec += Vector3.forward*1.5f;
        Instantiate(this.Bullet, myVec, transform.rotation);
    }

    public virtual void TakeHit(float damage)
    {
        if (!isHitted)
        {
            isHitted = true;
            findPlayer(100.0f);
        }
        
        if (!setShield)
            Stat.hp -= damage * AdditionalDamage;
        else
            Stat.barrier -= damage * AdditionalDamage;
        UI_Control.Inst.damageSet((damage * AdditionalDamage).ToString(), this.gameObject);//대미지 UI 추가 코드
        DealtDamage = Mathf.Round(damage * 10) * 0.1f;

        if (Stat.hp <= 0.0f)
        {
            MyAgent.isStopped = true;
            Anim.SetTrigger("IsDied");
        }

        if (Stat.barrier <= 0.0f)
        {
            setShield = false;
            shield.SetActive(false);
            col.enabled = true;
        }
        
    }

    public virtual void TakeElementHit(float damage, ElementRule.ElementType enemyElement) //���߿� ������ ���ɼ� ����.
    {
        if (!isHitted)
        {
            isHitted = true;
            findPlayer(100.0f);
        }
        setEnemyElement(enemyElement); // �̷��� EnemyElement�� �ٲ㼭 ���ų� enemyElement�״�� �ᵵ �ɵ�.
        float curDamage = attackedOnNormal(damage);

        if (EnemyElement != ElementType.NONE)
        {
            if (ElementStack.Count != 0)
            {
                if (EnemyElement != this.ElementStack.Peek())
                {
                    Debug.Log("Pushed!");
                    this.ElementStack.Push(EnemyElement);
                    checkIsPopTime();
                }
            }
            else
            {
                Debug.Log("Pushed!");
                this.ElementStack.Push(EnemyElement);
                checkIsPopTime();
            }
        }

        Stat.hp -= curDamage * AdditionalDamage;
        UI_Control.Inst.damageSet((curDamage * AdditionalDamage).ToString(), this.gameObject);//����� UI �߰� �ڵ�
        DealtDamage = Mathf.Round(curDamage * 10) * 0.1f;

        if (Stat.hp <= 0.0f)
        {
            MyAgent.isStopped = true;
            Anim.SetTrigger("IsDied");
        }        
    }

    public void RunToSleepChange()
    {
        State = STATE.SLEEP;
    }

    public void ReCharge()
    {
        if(Stat.hp == Stat.maxHp)
            State = STATE.IDLE;
    }
    private void OnDestroy()//에너미HP바 제거 및 퀘스트 인덱스 조정
    {//기존 퀘스트 오브젝트 선언 위치
     //switch (questObject.GetQuestKind())
     //{
     //    case QuestKind.kill:
     //        //if(questObject.GetObjectId()) 에너미 ID 설정 후 조정할 것
     //        int sum = questObject.GetObjectIndex() + 1;
     //        questObject.SetObjectIndex(sum);
     //        break;
     //    default:
     //        break;
     //}
     //questObject = null;

        if(Stat.hp<=0)
            switch (QuestObject.manager.GetQuestKind())
            {
                case QuestKind.kill:
                    if (QuestObject.manager.GetObjectId() == Stat.id)
                        QuestObject.manager.SetObjectIndex(QuestObject.manager.GetObjectIndex() + 1);
                    break;
                default:
                    break;
            }

        int index = UI_EnemyHp.EnemyHps.EnemyObjects.IndexOf(this.gameObject);
        UI_EnemyHp.EnemyHps.EnemyObjects.RemoveAt(index);
        Destroy(UI_EnemyHp.EnemyHps.hpObjects[index]);
        Destroy(UI_EnemyHp.EnemyHps.ShieldObjects[index]);
        UI_EnemyHp.EnemyHps.hpObjects.RemoveAt(index);
        UI_EnemyHp.EnemyHps.ShieldObjects.RemoveAt(index);
    }

    public void EndAttack()
    {
        isAttacked = false;
    }

    public bool isSetShield()
    {
        return setShield;
    }

}
