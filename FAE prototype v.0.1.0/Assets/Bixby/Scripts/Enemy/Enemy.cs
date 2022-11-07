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
    public bool isHitted = false;
    public bool runChance = true;

    public NavMeshAgent MyAgent;
    public Animator Anim;
    private Rigidbody rigid;

    public float DealtDamage;

    float genTerm; //현재 묻어있는 속성을 TextMeshPro로 나타내는 부분.

    // Start is called before the first frame update
    void Start()
    {
        Stat = new EnemyStatus();
        Stat = Stat.SetEnemyStatus(Type, Element);

        this.MyElement = Stat.element;

        Timer = 0.0f;

        State = STATE.IDLE;

        MyAgent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        UI_EnemyHp.EnemyHps.hpObjects.Add(Instantiate(UI_Control.Inst.EnemyHp.getPrefab(), GameObject.Find("UI").transform.GetChild(1)));
        UI_EnemyHp.EnemyHps.enemies.Add(this);
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

    private void died()
    {
        Destroy(gameObject);
    }

    public void MeleeAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Stat.attackRange, 
                                                    Mask, QueryTriggerInteraction.Ignore);
            
        if(colliders.Length > 0)
        {
            setEnemyElement(colliders[0].GetComponent<PlayerContorl>().MyElement);
            colliders[0].GetComponent<PlayerContorl>().TakeElementHit(Stat.damage, Stat.element);
            Debug.Log("Hit");
        }
    }

    public virtual void TakeHit(float damage)
    {
        if (!isHitted)
        {
            isHitted = true;
            findPlayer(100.0f);
        }
        Stat.hp -= damage * AdditionalDamage;
        UI_Control.Inst.damageSet((damage * AdditionalDamage).ToString(), this.gameObject);//����� UI �߰� �ڵ�
        DealtDamage = Mathf.Round(damage * 10) * 0.1f;
        
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
    private void OnDestroy()//���ʹ� HP�� ���� �� �ı� �߰� �κ�
    {
        QuestObject questObject = GameObject.Find("GameManager").GetComponent<QuestObject>();
        switch (questObject.GetQuestKind())
        {
            case QuestKind.kill:
                //if(questObject.GetObjectId()) 에너미 ID 설정 후 조정할 것
                int sum = questObject.GetObjectIndex() + 1;
                questObject.SetObjectIndex(sum);
                break;
            default:
                break;
        }
        questObject = null;

        int index = UI_EnemyHp.EnemyHps.enemies.IndexOf(this);
        UI_EnemyHp.EnemyHps.enemies.RemoveAt(index);
        Destroy(UI_EnemyHp.EnemyHps.hpObjects[index]);
        UI_EnemyHp.EnemyHps.hpObjects.RemoveAt(index);
    }
}
