using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonBoss : CombatStatus, IDamgeable
{
    public EnemyStatus Stat;
    public EnemyType Type;
    public ElementType Element;

    public STATE State;
    public enum STATE
    {
        IDLE,
        FIGHT
    }

    public LayerMask Mask = -1;
    public GameObject target;
    public GameObject Skill_1_Obj;

    public NavMeshAgent MyAgent;
    public Animator Anim;
    private Rigidbody rigid;
    private BoxCollider col;

    public float DealtDamage;
    public bool isAttacked;
    
    public float SkillCooldown;

    float genTerm; //현재 묻어있는 속성을 TextMeshPro로 나타내는 부분.

    private Shield shield;
    [SerializeField] private bool setShield;

    // Start is called before the first frame update
    void Start()
    {
        Stat = new EnemyStatus();
        Stat = Stat.SetEnemyStatus(EnemyType.FireBoss, Element);

        this.MyElement = Stat.element;
        isAttacked = false;
        
        SkillCooldown = 0.0f;

        target = null;
        
        State = STATE.IDLE;

        MyAgent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        
        //UI_EnemyHp.EnemyHps.hpObjects.Add(Instantiate(UI_Control.Inst.EnemyHp.getPrefab(true), GameObject.Find("UI").transform.GetChild(1)));
        //UI_EnemyHp.EnemyHps.enemies.Add(this);

        shield = this.gameObject.transform.Find("Shield").GetComponent<Shield>();
        setShield = false;

        Skill_1_Obj.transform.GetChild(0).GetComponent<Skill_1>().element = Stat.element;

        UI_EnemyHp.EnemyHps.hpObjects.Add(Instantiate(UI_Control.Inst.EnemyHp.getPrefab(true), GameObject.Find("UI").transform.GetChild(1)));
        UI_EnemyHp.EnemyHps.ShieldObjects.Add(Instantiate(UI_Control.Inst.EnemyHp.getPrefab(true), GameObject.Find("UI").transform.GetChild(1)));
        UI_EnemyHp.EnemyHps.EnemyObjects.Add(this.gameObject);
        UI_EnemyHp.EnemyHps.GaugeOff();
    }

    void FixedUpdate()
    {
        if (SkillCooldown > 0.0f)
            SkillCooldown -= Time.deltaTime;
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

        if (setShield)
        {
            shield.SetActive(true);
            col.enabled = false;
            //setShield = false;
        }
    }

    public void Died()
    {
        StartCoroutine(this.transform.Find("Cube_001").GetComponent<Dissolve>().Act(this.gameObject));
        //Destroy(gameObject);
    }

    private void findPlayer(float sight)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sight, 
                                                    Mask, QueryTriggerInteraction.Ignore);

        if(colliders.Length > 0)
        {    
            target = colliders[0].gameObject;
            State = STATE.FIGHT;
        }
    }
    
    public virtual void TakeHit(float damage)
    {        
        if (!setShield)
            Stat.hp -= damage * AdditionalDamage;
        else
            Stat.barrier -= damage * AdditionalDamage;
        UI_Control.Inst.damageSet((damage * AdditionalDamage).ToString(), this.gameObject);//대미지 UI 추가 코드
        DealtDamage = Mathf.Round(damage * 10) * 0.1f;

        if (Stat.barrier <= 0.0f)
        {
            setShield = false;
            shield.SetActive(false);
            col.enabled = true;
        }
        
    }

    public virtual void TakeElementHit(float damage, ElementRule.ElementType enemyElement) //���߿� ������ ���ɼ� ����.
    {
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
    
    public void EndAttack()
    {
        isAttacked = false;
    }

    public void NormalAttackAreaCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position+Vector3.forward*1.0f, Stat.attackRange, 
                                                    Mask, QueryTriggerInteraction.Ignore);

        if (colliders.Length > 0)
        {    
            setEnemyElement(colliders[0].GetComponent<PlayerContorl>().MyElement);
            colliders[0].GetComponent<PlayerContorl>().TakeElementHit(Stat.damage, Stat.element);
        }
    }

    public void DashAttackAreaCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Stat.attackRange*1.5f, 
                                                    Mask, QueryTriggerInteraction.Ignore);

        if (colliders.Length > 0)
        {    
            setEnemyElement(colliders[0].GetComponent<PlayerContorl>().MyElement);
            colliders[0].GetComponent<PlayerContorl>().TakeElementHit(Stat.damage, Stat.element);
        }
    }
    private void OnDestroy()
    {
        if (Stat.hp <= 0)
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
    public bool isSetShield()
    {
        return setShield;
    }
}
