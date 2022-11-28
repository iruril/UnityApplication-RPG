using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContorl : PlayerStatusControl
{
    private GameObject m_camera;
    private Vector3 cameraForward;
    private Vector3 cameraRight;

    private Vector3 playerForward;
    private Vector3 playerRight;

    private CharacterController player;

    private Vector3 playerDirection = Vector3.forward;
    private bool isJumpPressed = false;
    private bool isSwimming = false;

    public Transform ProjectileStart;

    private LineRenderer projectileLine;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.25f); //투사체 라인 유지 시간
    private Vector3 rayOrigin;
    private RaycastHit hitInfo;
    private RaycastHit[] hitInfo_all;
    private bool isAimAttack;

    private bool isOnAFK = false;
    private float latestActionTime = 0f;
    private float myAFKTime = 5.0f; //5초 후 Idle로 전환함.

    float genTerm; //현재 묻어있는 속성을 TextMeshPro로 나타내는 부분.

    public STATE State = STATE.NONE; // 현재 상태.
    public STATE NextState = STATE.NONE; // 다음 상태.
    public STATE PrevState = STATE.NONE; // 이전 상태.
    public float StateTimer = 0.0f; // 타이머
    public enum STATE
    {
        NONE = -1, // 상태 없음. 초기화 전용.
        IDLE = 0, // 대기
        MOVE = 1, // 이동
        AIM = 2, // 조준
        DASH = 3, // 도주기
        RUN = 4, // 질주
        ATTACK = 5, // 약공격
        CHARGE_ATTACK = 6, // 강공격
        ELEMENT_SKILL = 7, // 원소스킬
        ELEMENT_ULT_SKILL = 8, // 원소궁극기
        SWIMMING = 9, // 수영 중
        FISHING = 10, // 낚시
        DEAD = 11, // 사망

        NUM = 12, // 상태 종류
    };

    PlayerMesh playerMesh;      // 클래스 체인지 변경을 위한 컴포넌트 변수

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        this.isJumpPressed = false;
        this.isAimAttack = false;
        this.State = STATE.NONE;
        this.NextState = STATE.IDLE;

        m_camera = GameObject.FindGameObjectWithTag("MainCamera");
        player = this.GetComponent<CharacterController>();
        projectileLine = this.GetComponent<LineRenderer>();
        projectileLine.enabled = false;
        genTerm = 0f;

        playerMesh = transform.GetChild(1).GetChild(0).gameObject.GetComponent<PlayerMesh>();   // Player -> FemaleMage_A -> Margot
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

        if (this.State == STATE.MOVE || this.State == STATE.RUN || this.State == STATE.IDLE || this.State == STATE.AIM)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                this.mySkillStartColor = FireSkillStartColor;
                this.mySkillEndColor = FireSkillEndColor;
                this.MyElement = ElementType.FIRE;
                Debug.Log("current element : FIRE");

                playerMesh.ChangeClass((int)this.MyElement);            // 메쉬 변경
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                this.mySkillStartColor = IceSkillStartColor;
                this.mySkillEndColor = IceSkillEndColor;
                this.MyElement = ElementType.ICE;
                Debug.Log("current element : ICE");

                playerMesh.ChangeClass((int)this.MyElement);            // 메쉬 변경
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                this.mySkillStartColor = WaterSkillStartColor;
                this.mySkillEndColor = WaterSkillEndColor;
                this.MyElement = ElementType.WATER;
                Debug.Log("current element : WATER");

                playerMesh.ChangeClass((int)this.MyElement);            // 메쉬 변경
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                this.mySkillStartColor = ElectroSkillStartColor;
                this.mySkillEndColor = ElectroSkillEndColor;
                this.MyElement = ElementType.ELECTRICITY;
                Debug.Log("current element : ELECTRICITY");

                playerMesh.ChangeClass((int)this.MyElement);            // 메쉬 변경
            }
        }

        vectorAlign();
        this.StateTimer += Time.deltaTime;
        if (UI_Control.Inst.OpenedWindow == null)
        {
            if (this.NextState == STATE.NONE)
            {
                if (isSwimming == true)
                {
                    this.PrevState = this.State;
                    this.NextState = STATE.SWIMMING;
                }
                if (Fishing.isPlayerFishing == true)
                {
                    this.PrevState = this.State;
                    this.NextState = STATE.FISHING;
                }
                switch (this.State)
                {
                    case STATE.IDLE:
                        if ((Input.GetAxis("Vertical") != 0) || (Input.GetAxis("Horizontal") != 0))
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.MOVE;
                        }
                        if (Input.GetMouseButton(1))
                            this.NextState = STATE.AIM;
                        if (Input.GetMouseButton(0) && Time.time > nextFire)
                        {
                            this.isAimAttack = false;
                            this.PrevState = this.State;
                            this.NextState = STATE.ATTACK;
                        }
                        if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina > DashStaminaAmount && player.isGrounded)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.DASH;
                        }
                        if (Input.GetKeyDown(KeyCode.E) && player.isGrounded && !isSkillCoolDown)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.ELEMENT_SKILL;
                        }
                        if (Input.GetKeyDown(KeyCode.Q) && player.isGrounded && !isUltimateSkillCoolDown
                            && ElementGauge >= 100.0f && this.MyElement != ElementType.NONE)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.ELEMENT_ULT_SKILL;
                        }
                        break;
                    case STATE.MOVE:
                        if (Input.GetMouseButton(1))
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.AIM;
                        }
                        if (Input.GetMouseButton(0) && Time.time > nextFire)
                        {
                            this.isAimAttack = false;
                            this.PrevState = this.State;
                            this.NextState = STATE.ATTACK;
                        }
                        if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina > DashStaminaAmount && player.isGrounded)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.DASH;
                        }
                        if (Input.GetKeyDown(KeyCode.E) && player.isGrounded && !isSkillCoolDown)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.ELEMENT_SKILL;
                        }
                        if (Input.GetKeyDown(KeyCode.Q) && player.isGrounded && !isUltimateSkillCoolDown 
                            && ElementGauge >= 100.0f && this.MyElement != ElementType.NONE)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.ELEMENT_ULT_SKILL;
                        }
                        if (isAFK())
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.IDLE;
                        }
                        break;
                    case STATE.AIM:
                        if (!Input.GetMouseButton(1))
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.MOVE;
                        }
                        if (Input.GetMouseButton(0) && Time.time > nextFire)
                        {
                            this.isAimAttack = true;
                            this.PrevState = this.State;
                            this.NextState = STATE.ATTACK;
                        }
                        if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina > DashStaminaAmount && player.isGrounded)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.DASH;
                        }
                        if (Input.GetKeyDown(KeyCode.E) && player.isGrounded && !isSkillCoolDown)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.ELEMENT_SKILL;
                        }
                        if (Input.GetKeyDown(KeyCode.Q) && player.isGrounded && !isUltimateSkillCoolDown
                            && ElementGauge >= 100.0f && this.MyElement != ElementType.NONE)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.ELEMENT_ULT_SKILL;
                        }
                        break;
                    case STATE.DASH:
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.RUN;
                        }
                        else
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.MOVE;
                        }
                        break;
                    case STATE.RUN:
                        if (!Input.GetKey(KeyCode.LeftShift) || Stamina <= 0)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.MOVE;
                        }
                        if (Input.GetKeyDown(KeyCode.E) && player.isGrounded && !isSkillCoolDown)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.ELEMENT_SKILL;
                        }
                        if (Input.GetKeyDown(KeyCode.Q) && player.isGrounded && !isUltimateSkillCoolDown && ElementGauge >= 100.0f)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.ELEMENT_ULT_SKILL;
                        }
                        break;
                    case STATE.ATTACK:
                        if (this.StateTimer >= SwitchToChargeTime)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.CHARGE_ATTACK;
                        }
                        if (!Input.GetMouseButton(0))
                        {
                            nextFire = Time.time + FireRate;
                            attack();
                            if (this.PrevState == STATE.AIM)
                            {
                                this.PrevState = this.State;
                                this.NextState = STATE.AIM;
                            }
                            else
                            {
                                this.PrevState = this.State;
                                this.NextState = STATE.MOVE;
                            }
                        }
                        if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina > DashStaminaAmount && player.isGrounded)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.DASH;
                        }
                        break;
                    case STATE.CHARGE_ATTACK:
                        if (!Input.GetMouseButton(0))
                        {
                            if (this.Stamina >= ChargeAttackStaminaAmount)
                            {
                                nextFire = Time.time + FireRate;
                                StaminaUse(ChargeAttackStaminaAmount);
                                chargedAttack();
                            }
                            else
                            {
                                this.PrevState = this.State;
                                this.NextState = STATE.MOVE;
                            }
                            this.PrevState = this.State;
                            this.NextState = STATE.AIM;
                        }
                        if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina > DashStaminaAmount && player.isGrounded)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.DASH;
                        }
                        break;
                    case STATE.ELEMENT_SKILL:
                       
                        break;
                    case STATE.ELEMENT_ULT_SKILL:
                        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Q))
                        {
                            nextFire = Time.time + FireRate;
                            elementUltSkill();
                            StartCoroutine(fallBack());
                            StartCoroutine(ultSkillCoolDownCalc());
                            ElementGauge = 0.0f;
                            this.PrevState = this.State;
                            this.NextState = STATE.MOVE;
                        }
                        if (this.StateTimer >= 3.0f)
                        {
                            StartCoroutine(ultSkillCoolDownCalc());
                            ElementGauge = 0.0f;
                            this.PrevState = this.State;
                            this.NextState = STATE.MOVE;
                        }
                        break;
                    case STATE.SWIMMING:
                        //Water Tag에 접촉 해제하면 스테이트를 Move로 변경.
                        //지속적으로 스태미나 소모 시켜야 함.
                        if (isSwimming == false)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.MOVE;
                        }
                        if(Stamina <= 0)
                        {
                            //die();
                        }
                        break;
                    case STATE.FISHING:
                        //스턴 모션이 끝나면, 바로 Move로 상태 이동해야함.
                        if (Fishing.isPlayerFishing == false)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.MOVE;
                        }
                        break;
                    case STATE.DEAD:
                        //죽으면 리스폰.
                        break;
                }
            }

            // 상태가 변화했을 때------------.
            while (this.NextState != STATE.NONE)
            { // 상태가 NONE이외 = 상태가 변화했다.
                var offset = m_camera.transform.forward;

                this.State = this.NextState;
                this.NextState = STATE.NONE;
                switch (this.State)
                {
                    case STATE.IDLE:

                        break;
                    case STATE.MOVE:
                        MyCurrentSpeed = Speed;
                        m_camera.GetComponent<CamControl>().isOnAim = false;
                        break;
                    case STATE.AIM:
                        offset.y = 0;
                        m_camera.GetComponent<CamControl>().isOnAim = true;
                        break;
                    case STATE.DASH:
                        m_camera.GetComponent<CamControl>().isOnAim = false;
                        StaminaUse(DashStaminaAmount);
                        isDashed = false;
                        break;
                    case STATE.RUN:
                        MyCurrentSpeed = RunSpeed;
                        m_camera.GetComponent<CamControl>().isOnAim = false;
                        break;
                    case STATE.ATTACK:
                        MyCurrentSpeed = Speed;
                        switch (this.isAimAttack)
                        {
                            case true:
                                m_camera.GetComponent<CamControl>().isOnAim = true;
                                break;
                            case false:
                                break;
                        }
                        break;
                    case STATE.CHARGE_ATTACK:
                        MyCurrentSpeed = Speed;
                        m_camera.GetComponent<CamControl>().isOnAim = true;
                        break;
                    case STATE.ELEMENT_SKILL:
                        offset.y = 0;
                        transform.LookAt(player.transform.position + offset);
                        break;
                    case STATE.ELEMENT_ULT_SKILL:
                        offset.y = 0;
                        transform.LookAt(player.transform.position + offset);
                        m_camera.GetComponent<CamControl>().isOnAim = true;
                        break;
                    case STATE.SWIMMING:
                        MyCurrentSpeed = SwimSpeed;
                        m_camera.GetComponent<CamControl>().isOnAim = false;
                        break;
                    case STATE.FISHING:
                        m_camera.GetComponent<CamControl>().isOnAim = true;
                        break;
                    case STATE.DEAD:

                        break;
                }
                this.StateTimer = 0.0f;
            }
            // 각 상황에서 반복할 것----------.
            switch (this.State)
            {
                case STATE.IDLE:
                    move();
                    StaminaRegerenate();
                    break;
                case STATE.MOVE:
                    move();
                    StaminaRegerenate();
                    break;
                case STATE.AIM:
                    aimModeMove();
                    StaminaRegerenate();
                    break;
                case STATE.DASH:
                    dash();
                    break;
                case STATE.RUN:
                    move();
                    StaminaTickUse(RunStaminaAmount);
                    break;
                case STATE.ATTACK:
                    switch (this.isAimAttack)
                    {
                        case true:
                            aimModeMove();
                            break;
                        case false:
                            if (this.StateTimer < 0.3f)
                            {
                                move();
                            }
                            else
                            {
                                m_camera.GetComponent<CamControl>().isOnAim = true;
                                aimModeMove();
                            }
                            break;
                    }
                    break;
                case STATE.CHARGE_ATTACK:
                    aimModeMove();
                    break;
                case STATE.ELEMENT_SKILL:
                    elementSkill();
                    break;
                case STATE.ELEMENT_ULT_SKILL:
                    ultLockOn();
                    break;
                case STATE.SWIMMING:
                    swimming();
                    if (Stamina >= 0)
                    {
                        StaminaTickUse(SwimStaminaAmount);
                    }
                    break;
                case STATE.FISHING:
                    //낚시 중에는 아무 행동도 못한다.
                    break;
                case STATE.DEAD:
                    playerDirection.y -= GravityForce * Time.deltaTime;
                    break;
            }
        }

        if (isHitted)
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            isSwimming = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            isSwimming = false;
        }
    }

    protected override void die()
    {
        base.die();
        this.PrevState = this.State;
        this.NextState = STATE.DEAD;
    }

    private bool isAFK()
    {
        latestActionTime += Time.deltaTime;
        if (Input.anyKey)
        {
            isOnAFK = false;
            latestActionTime = 0;
        }
        else
        {
            isOnAFK = true;
        }

        if ((latestActionTime >= myAFKTime) && isOnAFK)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //private bool isCheckGrounded()
    //{
    //    if (player.isGrounded) return true;
    //    var ray = new Ray(this.transform.position + Vector3.up * 0.1f, Vector3.down);
    //    var maxDistance = 1.5f;
    //    Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * maxDistance, Color.red);
    //    return Physics.Raycast(ray, maxDistance, LayerMask.GetMask("Ground"));
    //}

    private void vectorAlign()
    {
        cameraForward = m_camera.transform.forward;
        cameraRight = m_camera.transform.right;

        playerForward = cameraForward;
        playerForward.y = 0;
        playerRight = cameraRight;
        playerRight.y = 0;
    }

    private void move() //일반적으로 이동할 때 사용하는 움직임 방식.
    {
        Vector3 snapGround = Vector3.zero;

        if ((Input.GetKey(KeyCode.A)) && (Input.GetKey(KeyCode.S)))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation(-(playerRight + playerForward).normalized),
               Time.deltaTime * rotationSpeed);
        }

        else if ((Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.W)))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation((playerRight + playerForward).normalized),
               Time.deltaTime * rotationSpeed);
        }

        else if((Input.GetKey(KeyCode.A)) && (Input.GetKey(KeyCode.W)))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation(-(playerRight - playerForward).normalized),
               Time.deltaTime * rotationSpeed);
        }

        else if((Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.S)))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation(-(-playerRight + playerForward).normalized),
               Time.deltaTime * rotationSpeed);
        }

        else if(Input.GetKey(KeyCode.W))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
                Quaternion.LookRotation(playerForward),
                Time.deltaTime * rotationSpeed);
        }

        else if(Input.GetKey(KeyCode.S))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
                Quaternion.LookRotation(-playerForward),
                Time.deltaTime * rotationSpeed);
        }

        else if(Input.GetKey(KeyCode.D))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
                Quaternion.LookRotation(playerRight),
                Time.deltaTime * rotationSpeed);
        }

        else if(Input.GetKey(KeyCode.A))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation(-playerRight),
               Time.deltaTime * rotationSpeed);
        }

        if (player.isGrounded)
        {
            if ((Input.GetAxis("Vertical") != 0) || (Input.GetAxis("Horizontal") != 0))
                playerDirection = new Vector3(0, 0, 1);
            else playerDirection = new Vector3(0, 0, 0);

            playerDirection = player.transform.TransformDirection(playerDirection);
            playerDirection *= MyCurrentSpeed * SpeedMultiply;

            if (isJumpPressed == false && Input.GetButton("Jump"))
            {
                isJumpPressed = true;
                playerDirection.y = JumpPower;
            }
        }
        else
        {
            playerDirection.y -= GravityForce * Time.deltaTime;
        }

        if (!Input.GetButton("Jump"))
        {
            if (player.isGrounded) snapGround = Vector3.down;
            isJumpPressed = false;
        }

        player.Move(playerDirection * Time.deltaTime + snapGround);
    }
    private void swimming() //수영 중.
    {
        Vector3 snapGround = Vector3.down;

        if ((Input.GetKey(KeyCode.A)) && (Input.GetKey(KeyCode.S)))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation(-(playerRight + playerForward).normalized),
               Time.deltaTime * rotationSpeed);
        }

        else if ((Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.W)))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation((playerRight + playerForward).normalized),
               Time.deltaTime * rotationSpeed);
        }

        else if ((Input.GetKey(KeyCode.A)) && (Input.GetKey(KeyCode.W)))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation(-(playerRight - playerForward).normalized),
               Time.deltaTime * rotationSpeed);
        }

        else if ((Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.S)))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation(-(-playerRight + playerForward).normalized),
               Time.deltaTime * rotationSpeed);
        }

        else if (Input.GetKey(KeyCode.W))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
                Quaternion.LookRotation(playerForward),
                Time.deltaTime * rotationSpeed);
        }

        else if (Input.GetKey(KeyCode.S))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
                Quaternion.LookRotation(-playerForward),
                Time.deltaTime * rotationSpeed);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
                Quaternion.LookRotation(playerRight),
                Time.deltaTime * rotationSpeed);
        }

        else if (Input.GetKey(KeyCode.A))
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
               Quaternion.LookRotation(-playerRight),
               Time.deltaTime * rotationSpeed);
        }

        if (player.isGrounded)
        {
            if ((Input.GetAxis("Vertical") != 0) || (Input.GetAxis("Horizontal") != 0))
                playerDirection = new Vector3(0, 0, 1);
            else playerDirection = new Vector3(0, 0, 0);

            playerDirection = player.transform.TransformDirection(playerDirection);
            playerDirection *= MyCurrentSpeed * SpeedMultiply;
        }
        else
        {
            playerDirection.y -= GravityForce * Time.deltaTime;
        }

        player.Move(playerDirection * Time.deltaTime + snapGround);
    }

    private void aimModeMove() //조준모드 에서는 점프 불가. 50%의 속도로 이동함.
    {
        Vector3 snapGround = Vector3.zero;

        var offset = m_camera.transform.forward;
        offset.y = 0;
        transform.LookAt(player.transform.position + offset);

        if (player.isGrounded)
        {
            playerDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            playerDirection = player.transform.TransformDirection(playerDirection);
            playerDirection *= (float)(Speed / 2) * SpeedMultiply;
        }
        else
            playerDirection.y -= GravityForce * Time.deltaTime;

        if (player.isGrounded) snapGround = Vector3.down;

        player.Move(playerDirection * Time.deltaTime + snapGround);
    }

    private void dash() //질주
    {
        StartCoroutine(dashCoroutine());
        isDashed = true;
    }
    private IEnumerator dashCoroutine() 
    {
        Vector3 snapGround = Vector3.zero;
        if (player.isGrounded) snapGround = Vector3.down;
        float startTime = Time.time; //대쉬를 누른 시간
        while (Time.time < startTime + 0.3f) //대쉬가 지속될 시간, 0.3초
        {
            player.Move(this.transform.forward * DashSpeed * Time.deltaTime + snapGround);
            yield return null;
        }
    }

    public bool GetIsGrounded()
    {
        return player.isGrounded;
    }

    private void attack()
    {
        projectileLine.startColor = Color.yellow; //기본공격은 속성값이 없어, 고정값만 가진다.
        projectileLine.endColor = Color.white; //기본공격은 속성값이 없어, 고정값만 가진다.
        projectileLine.startWidth = 1.0f;
        projectileLine.endWidth = 1.0f;

        switch (this.isAimAttack)
        {
            case true:
                rayOrigin = m_camera.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
                
                projectileLine.SetPosition(0, ProjectileStart.position);
                if(Physics.Raycast(rayOrigin, m_camera.transform.forward, out hitInfo, ShootDistance))
                {
                    projectileLine.SetPosition(1, hitInfo.point);
                    this.gameObject.GetComponent<PlayerLineSkill>().ShowAttackEffect((int)this.State, (int)this.MyElement, ProjectileStart);
                    this.gameObject.GetComponent<PlayerLineSkill>().ShowHitEffect(hitInfo.point);
                    StartCoroutine(shootEffect());
                    if (hitInfo.collider.tag == "Enemy")
                    {
                        hitInfo.collider.GetComponent<Enemy>().TakeHit(AttackDamage);
                    }
                    else if (hitInfo.collider.tag == "DungeonBoss")
                    {
                        hitInfo.collider.GetComponent<DungeonBoss>().TakeHit(AttackDamage);
                    }
                    else if (hitInfo.collider.tag == "FinalBoss")
                    {
                        hitInfo.collider.GetComponent<FinalBoss>().TakeHit(AttackDamage);
                    }

                    else if (hitInfo.collider.gameObject.tag == "Shield")
                    {
                        StartCoroutine(hitInfo.collider.GetComponent<Shield>().CreateRipple(hitInfo.point));
                    }

                    //퍼즐오브젝트와 상호작용 부분 추가
                    if (hitInfo.collider.gameObject.tag == "pattern")
                    {
                        hitInfo.collider.GetComponent<Pattern>().GetP(hitInfo.point, hitInfo.normal);
                    }
                    else if (hitInfo.collider.gameObject.tag == "connect")
                    {
                        hitInfo.collider.GetComponent<Connect>().GetP(hitInfo.point, hitInfo.normal);
                    }

                    //동물 오브젝트와 상호작용 부분 추가
                    if (hitInfo.collider.tag == "animal")
                    {
                        if (hitInfo.collider.GetComponent<Deer>().GetComeBack() || hitInfo.collider.GetComponent<Deer>().GetIsDead())
                        {
                            //아무것도 안한다. 공격안받음
                        }
                        else if (!hitInfo.collider.GetComponent<Deer>().GetComeBack())
                        {
                            hitInfo.collider.GetComponent<Deer>().Damage(1, transform.position);
                        }
                    }
                }
                else
                {
                    projectileLine.SetPosition(1, rayOrigin + (m_camera.transform.forward * ShootDistance));
                    this.gameObject.GetComponent<PlayerLineSkill>().ShowAttackEffect((int)this.State, (int)this.MyElement, ProjectileStart);
                    StartCoroutine(shootEffect());
                }
                break;
            case false:
                rayOrigin = ProjectileStart.position;
                projectileLine.SetPosition(0, ProjectileStart.position);
                if (Physics.Raycast(rayOrigin, player.transform.forward, out hitInfo, ShootDistance))
                {
                    projectileLine.SetPosition(1, hitInfo.point);
                    this.gameObject.GetComponent<PlayerLineSkill>().ShowAttackEffect((int)this.State, (int)this.MyElement, ProjectileStart);
                    this.gameObject.GetComponent<PlayerLineSkill>().ShowHitEffect(hitInfo.point);
                    StartCoroutine(shootEffect());
                    if (hitInfo.collider.tag == "Enemy")
                    {
                        hitInfo.collider.GetComponent<Enemy>().TakeHit(AttackDamage);
                    }
                    else if (hitInfo.collider.tag == "DungeonBoss")
                    {
                        hitInfo.collider.GetComponent<DungeonBoss>().TakeHit(AttackDamage);
                    }
                    else if (hitInfo.collider.tag == "FinalBoss")
                    {
                        hitInfo.collider.GetComponent<FinalBoss>().TakeHit(AttackDamage);
                    }                    
                }
                else
                {
                    projectileLine.SetPosition(1, ProjectileStart.position + (player.transform.forward * ShootDistance));
                    this.gameObject.GetComponent<PlayerLineSkill>().ShowAttackEffect((int)this.State, (int)this.MyElement, ProjectileStart);
                    StartCoroutine(shootEffect());
                }
                break;
        }
    }

    private void chargedAttack()
    {
        projectileLine.startColor = mySkillStartColor; //현재 속성에 따라서 스킬 색깔이 바뀐다.
        projectileLine.endColor = mySkillEndColor; //현재 속성에 따라서 스킬 색깔이 바뀐다.
        projectileLine.startWidth = 2.5f;
        projectileLine.endWidth = 2.5f;

        rayOrigin = m_camera.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        projectileLine.SetPosition(0, ProjectileStart.position);
        if (Physics.Raycast(rayOrigin, m_camera.transform.forward, out hitInfo, ShootDistance))
        {
            projectileLine.SetPosition(1, hitInfo.point);
            this.gameObject.GetComponent<PlayerLineSkill>().ShowAttackEffect((int)this.State, (int)this.MyElement, ProjectileStart);
            this.gameObject.GetComponent<PlayerLineSkill>().ShowHitEffect(hitInfo.point);
            StartCoroutine(shootEffect());
            if (hitInfo.collider.tag == "Enemy")
            {
                setEnemyElement(hitInfo.collider.GetComponent<Enemy>().Stat.element);
                hitInfo.collider.GetComponent<Enemy>().TakeElementHit(ChargedAttackDamage, MyElement);

                if (MyElement != ElementType.NONE)
                {
                    ElementGauge += ElementGaugeChargeAmount; //원소게이지 ++
                }
                if (ElementGauge > 100.0f) ElementGauge = 100.0f;
            }
            else if (hitInfo.collider.tag == "DungeonBoss")
            {
                setEnemyElement(hitInfo.collider.GetComponent<DungeonBoss>().Stat.element);
                hitInfo.collider.GetComponent<DungeonBoss>().TakeElementHit(ChargedAttackDamage, MyElement);

                if (MyElement != ElementType.NONE)
                {
                    ElementGauge += ElementGaugeChargeAmount; //원소게이지 ++
                }
                if (ElementGauge > 100.0f) ElementGauge = 100.0f;
            }
            else if (hitInfo.collider.tag == "FinalBoss")
            {
                setEnemyElement(hitInfo.collider.GetComponent<FinalBoss>().Stat.element);
                hitInfo.collider.GetComponent<FinalBoss>().TakeElementHit(ChargedAttackDamage, MyElement);

                if (MyElement != ElementType.NONE)
                {
                    ElementGauge += ElementGaugeChargeAmount; //원소게이지 ++
                }
                if (ElementGauge > 100.0f) ElementGauge = 100.0f;
            }
            else if (hitInfo.collider.tag == "FireBall")
            {
                //setEnemyElement(hitInfo.collider.GetComponent<FireBallBullet>().Stat.element);
                hitInfo.collider.GetComponent<FireBallBullet>().TakeElementHit(ChargedAttackDamage, MyElement);

                if (MyElement != ElementType.NONE)
                {
                    ElementGauge += ElementGaugeChargeAmount; //원소게이지 ++
                }
                if (ElementGauge > 100.0f) ElementGauge = 100.0f;
            }
            else if (hitInfo.collider.gameObject.tag == "Shield")
            {
                StartCoroutine(hitInfo.collider.GetComponent<Shield>().CreateRipple(hitInfo.point));

                hitInfo.collider.GetComponent<Shield>().TakeElementHit(ChargedAttackDamage, MyElement);

                if (MyElement != ElementType.NONE)
                {
                    ElementGauge += ElementGaugeChargeAmount; //원소게이지 ++
                }
                if (ElementGauge > 100.0f) ElementGauge = 100.0f;                
            }

            //퍼즐오브젝트와 상호작용 추가
            if (hitInfo.collider.gameObject.tag == "etrigger")
            {
                hitInfo.collider.GetComponent<Etrigger>().GetElement(MyElement);
            }

            //동물 오브젝트와 상호작용 부분 추가
            if (hitInfo.collider.tag == "animal")
            {
                if (hitInfo.collider.GetComponent<Deer>().GetComeBack() || hitInfo.collider.GetComponent<Deer>().GetIsDead())
                {
                    //아무것도 안한다. 공격안받음
                }
                else if (!hitInfo.collider.GetComponent<Deer>().GetComeBack())
                {
                    hitInfo.collider.GetComponent<Deer>().Damage(1, transform.position);
                }
            }
        }
        else
        {
            projectileLine.SetPosition(1, rayOrigin + (m_camera.transform.forward * ShootDistance));
            this.gameObject.GetComponent<PlayerLineSkill>().ShowAttackEffect((int)this.State, (int)this.MyElement, ProjectileStart);
            StartCoroutine(shootEffect());
        }
    }

    private void elementSkill()
    {
        this.gameObject.GetComponent<PlayerLineSkill>().ShowSkillEffect((int)this.MyElement, ProjectileStart);

        if (MyElement != ElementType.NONE)
        {
            ElementGauge += ElementGaugeChargeAmount; //원소게이지 ++
        }
        if (ElementGauge > 100.0f) ElementGauge = 100.0f;

        StartCoroutine(skillCoolDownCalc());
        StartCoroutine(fallBack()); //후퇴
        this.NextState = STATE.MOVE;
    }

    private void ultLockOn()
    {
        Vector3 snapGround = Vector3.zero;
        var offset = m_camera.transform.forward;
        offset.y = 0;
        transform.LookAt(player.transform.position + offset);
        player.Move( snapGround);
    }

    private void elementUltSkill()
    {
        projectileLine.startColor = mySkillStartColor; //현재 속성에 따라서 스킬 색깔이 바뀐다.
        projectileLine.endColor = mySkillEndColor; //현재 속성에 따라서 스킬 색깔이 바뀐다.
        projectileLine.startWidth = 4.0f;
        projectileLine.endWidth = 4.0f;

        rayOrigin = m_camera.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        projectileLine.SetPosition(0, ProjectileStart.position);

        hitInfo_all = Physics.RaycastAll(rayOrigin, m_camera.transform.forward, ShootDistance);
        for(int i = 0; i < hitInfo_all.Length; i++)
        {
            hitInfo = hitInfo_all[i];
            this.gameObject.GetComponent<PlayerLineSkill>().ShowAttackEffect((int)this.State, (int)this.MyElement, ProjectileStart);
            this.gameObject.GetComponent<PlayerLineSkill>().ShowHitEffect(hitInfo.point);
            StartCoroutine(shootEffect());
            if (hitInfo.collider.tag == "Enemy")
            {
                setEnemyElement(hitInfo.collider.GetComponent<Enemy>().Stat.element);
                hitInfo.collider.GetComponent<Enemy>().TakeElementHit(UltDamage, MyElement);
            }
            else if (hitInfo.collider.tag == "DungeonBoss")
            {
                setEnemyElement(hitInfo.collider.GetComponent<DungeonBoss>().Stat.element);
                hitInfo.collider.GetComponent<DungeonBoss>().TakeElementHit(UltDamage, MyElement);
            }
            else if (hitInfo.collider.tag == "FinalBoss")
            {
                setEnemyElement(hitInfo.collider.GetComponent<FinalBoss>().Stat.element);
                hitInfo.collider.GetComponent<FinalBoss>().TakeElementHit(UltDamage, MyElement);
            }
        }
        projectileLine.SetPosition(1, rayOrigin + (m_camera.transform.forward * ShootDistance));
        StartCoroutine(shootEffect());
    }

    private IEnumerator fallBack()
    {
        Vector3 snapGround = Vector3.zero;
        if (player.isGrounded) snapGround = Vector3.down;
        float startTime = Time.time;
        while (Time.time < startTime + 0.25f)
        {
            player.Move(this.transform.forward * -20.0f * Time.deltaTime + snapGround);
            yield return null;
        }
    }

    private IEnumerator shootEffect()
    {
        projectileLine.enabled = true;
        yield return shotDuration;
        projectileLine.enabled = false;
    }
}