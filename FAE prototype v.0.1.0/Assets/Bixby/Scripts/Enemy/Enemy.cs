using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStatus Stat;
    public EnemyType Type;
    public ElementRule.ElementType Element;

    public float EnemySight;
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

    public Animator Anim;
    private Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        Stat = new EnemyStatus();
        Stat = Stat.SetEnemyStatus(Type, Element);

        Timer = 0.0f;

        State = STATE.IDLE;

        rigid = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();

        UI_EnemyHp.EnemyHps.hpObjects.Add(Instantiate(UI_Control.Inst.EnemyHp.getPrefab(), GameObject.Find("UI").transform.GetChild(1)));
        UI_EnemyHp.EnemyHps.enemies.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        rigid.velocity = Vector3.zero;

        findPlayer();

        if(State != STATE.CHASE && State != STATE.RUNAWAY)
            stateChange();

        if(Stat.hp % Stat.maxHp == 1)
            runChance = true;

        if(runChance && Stat.hp % Stat.maxHp <= 0.34)
        {
            runChance = false;
            State = STATE.RUNAWAY;
        }

        if(isHitted)
            State = STATE.CHASE;

        //died();
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

    private void findPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, EnemySight, 
                                                    Mask, QueryTriggerInteraction.Ignore);

        if(colliders.Length > 0)
        {    
            target = colliders[0].gameObject;
            State = STATE.CHASE;
        }
    }

    private void died()
    {
        if(Stat.hp <= 0.0f)
            Destroy(gameObject);
    }



    public void TakeDamage(float value)
    {
        isHitted = true;
        UI_Control.Inst.damageSet(value.ToString(), this.gameObject);//대미지 UI 추가 부분
        Stat.hp -= value;
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
    private void OnDestroy()//에너미 HP바 해제 및 파괴 추가 부분
    {
        int index = UI_EnemyHp.EnemyHps.enemies.IndexOf(this);
        UI_EnemyHp.EnemyHps.enemies.RemoveAt(index);
        Destroy(UI_EnemyHp.EnemyHps.hpObjects[index]);
        UI_EnemyHp.EnemyHps.hpObjects.RemoveAt(index);
    }
}
