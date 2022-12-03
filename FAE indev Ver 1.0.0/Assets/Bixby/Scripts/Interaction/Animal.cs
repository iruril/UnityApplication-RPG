using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [SerializeField] protected string animalName; // 동물의 이름
    [SerializeField] protected int hp;  // 동물의 체력 -> 공격 2번 받으면으로 변경

    [SerializeField] protected float walkSpeed;  // 걷기 속력
    [SerializeField] protected float runSpeed;  // 달리기 속력
    [SerializeField] protected float comebackSpeed;  // 돌아오는속도
    protected float applySpeed;

    protected Vector3 destination;  // 목적지
    [SerializeField] protected Vector3 spawnPosition; //스폰한 위치

    // 상태 변수
    protected bool isAction;  // 행동 중인지 아닌지 판별
    protected bool isWalking; // 걷는지, 안 걷는지 판별
    protected bool isRunning; // 달리는지 판별
    protected bool isDead;   // 죽었는지 판별
    public bool isComeBack; //돌아오고있는지 판별
    public bool testchack = true; //오브젝트 상태 체크

    [SerializeField] protected float walkTime;  // 걷기 시간
    [SerializeField] protected float waitTime;  // 대기 시간
    [SerializeField] protected float runTime;  // 뛰기 시간
    protected float currentTime;


    // 필요한 컴포넌트
    [SerializeField] protected Animator anim;

    protected NavMeshAgent nav;


    protected GameObject Player;
    itemJsonData itemJsonData;//json데이터

    private void Awake()
    {
        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "Harvest");//json로드
    }

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();
        currentTime = waitTime;   // 대기 시작
        isAction = true;   // 대기도 행동
        spawnPosition = this.transform.position; //현재 위치를 저장 이거 바꿔야한다.
    }

    //가까이 있는지 체크하는 변수
    public bool checkNear = false;

    // Update is called once per frame
    void Update()
    {
        //안죽었으면
        if (!isDead)
        {
            Move();
            ElapseTime();
        }

        //죽었으면
        if (isDead && Vector3.Distance(Player.transform.position, transform.position) <= 2.0f)
        {
            checkNear = true;

            //f키 생성
            if (!inventoryObject.Inst.FieldFKey.activeSelf)
            {
                inventoryObject.Inst.FieldFKey.SetActive(true);
            }
        }
        else if (isDead && Vector3.Distance(Player.transform.position, transform.position) > 2.0f && checkNear == true)
        {
            //f키 제거
            inventoryObject.Inst.FieldFKey.SetActive(false);

            checkNear = false;
        }

        if (checkNear && Input.GetKeyDown(KeyCode.F))
        {
            //아이템 획득
            animalData = new itemData();
            //아이템 지정 스크립트
            animalData.itemID = 2005; animalData.tag = new string[] { "food", "harvest" }; animalData.itemName = "동물고기";
            animalData.Left = -1; animalData.Up = -1; animalData.xSize = 1; animalData.ySize = 1;
            animalData.isEquip = false; animalData.isSell = false;
            animalData.price = 2; //나중에 가격 변경

            Vector2 tempPos;
            tempPos = inventoryObject.Inst.emptyCell(animalData.xSize, animalData.ySize);
            inventoryObject.Inst.itemGet(animalData.xSize, animalData.ySize, tempPos.x, tempPos.y, animalData);

            //인벤토리 추가 및 제이슨 저장
            inventoryObject.Inst.jsonSave();

            //동물 제거
            //Destroy(gameObject);
            this.gameObject.SetActive(false);
            testchack = false;

            //f키 제거
            inventoryObject.Inst.FieldFKey.SetActive(false);

            checkNear = false;
        }
    }

    //아이템 데이터
    itemData animalData;

    protected void Move()
    {
        if (isWalking || isRunning)
            nav.SetDestination(transform.position + destination * 3f);
        else if (isComeBack)
            nav.SetDestination(destination);


        if (isWalking)
        {
            //이동범위제한
            float x = Mathf.Clamp(transform.position.x, spawnPosition.x - 10.0f, spawnPosition.x + 10.0f);
            float z = Mathf.Clamp(transform.position.z, spawnPosition.z - 10.0f, spawnPosition.z + 10.0f);

            transform.position = new Vector3(x, transform.position.y, z);
        }
    }

    bool _delay = true; //딜레이 체크

    protected void ElapseTime()
    {
        if (isAction)
        {
            if (isRunning && _delay)
            {
                StartCoroutine(Delay());
                return;
            }
            else if (isComeBack)
            {
                if (Vector3.Distance(transform.position, spawnPosition) <= 1.0f)
                {
                    isComeBack = false;
                    ReSet();
                    return;
                }
            }
            else if (!isRunning && !isComeBack)
            {
                currentTime -= Time.deltaTime;
                if (currentTime <= 0)  // 랜덤하게 다음 행동을 개시
                    ReSet();
            }
        }
    }

    protected virtual void ReSet()  // 다음 행동 준비
    {
        isAction = true;

        nav.ResetPath();

        //isWalking = false;
        //anim.SetBool("Walking", isWalking);
        //isRunning = false;
        //anim.SetBool("Running", isRunning);

        isWalking = false;
        isRunning = false;
        isComeBack = false;
        anim.SetBool("Move", false);

        nav.speed = walkSpeed;



        destination.Set(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)); //랜덤위치
    }

    protected void TryWalk()  // 걷기
    {
        currentTime = walkTime;
        isWalking = true;
        //anim.SetBool("Walking", isWalking);
        anim.SetBool("Move", isWalking);
        nav.speed = walkSpeed;
        //Debug.Log("걷기");
    }

    public virtual void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;

            if (hp <= 0)
            {
                Dead();
                return;
            }

            //PlaySE(sound_Hurt); //->소리같음
            //anim.SetTrigger("Hurt");
            //Run(_targetPos);
        }
    }

    protected void Dead()
    {
        //PlaySE(sound_Dead); //->소리같음

        isWalking = false;
        isRunning = false;
        isDead = true;

        anim.SetTrigger("Dead");

        //초기화하고 오브젝트 끄기
        //아이템 획득
    }


    private IEnumerator Delay()
    {
        _delay = false;
        yield return new WaitForSeconds(5.0f);
        _delay = true;
        Debug.Log("이거실행");
        ComeBack();

        //yield return new WaitForSeconds(5.0f);

        //어떤거 추가
        //ReSet();
        //
    }

    public bool GetComeBack()
    {
        return isComeBack;
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public void SetSpawnPosition(Vector3 spawnposition)
    {
        spawnPosition = spawnposition;
    }

    public void ComeBack()
    {
        destination = spawnPosition;
        //currentTime = runTime;
        isWalking = false;
        isRunning = false;
        isComeBack = true;
        nav.speed = comebackSpeed; //돌아오는속도

        anim.SetBool("Move", isRunning);

        hp = 2; //hp 초기화
    }

    //초기화
    public void initialization()
    {
        isAction = true;  // 행동 중인지 아닌지 판별
        isWalking = false; // 걷는지, 안 걷는지 판별
        isRunning = false; // 달리는지 판별
        isDead = false;   // 죽었는지 판별
        isComeBack = false; //돌아오고있는지 판별
        currentTime = waitTime;   // 대기 시작
        _delay = true;
        hp = 2;
        anim.SetBool("Move", false);
        nav.speed = walkSpeed;
        spawnPosition = this.transform.position; //현재 위치를 저장 이거 바꿔야한다.

        destination.Set(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)); //랜덤위치
    }
}
