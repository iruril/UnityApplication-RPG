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
    private WaitForSeconds shotDuration = new WaitForSeconds(0.25f); //����ü ���� ���� �ð�
    private Vector3 rayOrigin;
    private RaycastHit hitInfo;
    private RaycastHit[] hitInfo_all;
    private bool isAimAttack;

    private bool isOnAFK = false;
    private float latestActionTime = 0f;
    private float myAFKTime = 5.0f; //5�� �� Idle�� ��ȯ��.

    float genTerm; //���� �����ִ� �Ӽ��� TextMeshPro�� ��Ÿ���� �κ�.

    public STATE State = STATE.NONE; // ���� ����.
    public STATE NextState = STATE.NONE; // ���� ����.
    public STATE PrevState = STATE.NONE; // ���� ����.
    public float StateTimer = 0.0f; // Ÿ�̸�
    public enum STATE
    {
        NONE = -1, // ���� ����. �ʱ�ȭ ����.
        IDLE = 0, // ���
        MOVE = 1, // �̵�
        AIM = 2, // ����
        DASH = 3, // ���ֱ�
        RUN = 4, // ����
        ATTACK = 5, // �����
        CHARGE_ATTACK = 6, // ������
        ELEMENT_SKILL = 7, // ���ҽ�ų
        ELEMENT_ULT_SKILL = 8, // ���ұñر�
        SWIMMING = 9, // ���� ��
        FISHING = 10, // ����
        DEAD = 11, // ���

        NUM = 12, // ���� ����
    };

    PlayerMesh playerMesh;      // Ŭ���� ü���� ������ ���� ������Ʈ ����

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
        //���� �����ִ� �Ӽ��� TextMeshPro�� ��Ÿ���� �κ�.
        this.genTerm += Time.deltaTime;
        if (genTerm >= 0.5f)
        {
            UI_Control.Inst.ElementStateGen(this.gameObject, this.genTerm);
            genTerm = 0.0f;
        }
        //���� �����ִ� �Ӽ��� TextMeshPro�� ��Ÿ���� �κ�.

        if (this.State == STATE.MOVE || this.State == STATE.RUN || this.State == STATE.IDLE || this.State == STATE.AIM)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if(QuestObject.manager.GetIndex() < 5 || (QuestObject.manager.GetIndex() == 5 && !QuestObject.manager.GetIsClear()))  // �̰��� ����
                {
                    Debug.Log("���� �ش� ���Ұ� ������� �ʾҽ��ϴ�." + QuestObject.manager.GetIndex());
                }
                else
                {
                    this.mySkillStartColor = FireSkillStartColor;
                    this.mySkillEndColor = FireSkillEndColor;
                    this.MyElement = ElementType.FIRE;
                    Debug.Log("current element : FIRE");

                    playerMesh.ChangeClass((int)this.MyElement);            // �޽� ����
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (QuestObject.manager.GetIndex() < 10)  // �̰��� ����
                {
                    Debug.Log("���� �ش� ���Ұ� ������� �ʾҽ��ϴ�." + QuestObject.manager.GetIndex());
                }
                else
                {
                    this.mySkillStartColor = IceSkillStartColor;
                    this.mySkillEndColor = IceSkillEndColor;
                    this.MyElement = ElementType.ICE;
                    Debug.Log("current element : ICE");

                    playerMesh.ChangeClass((int)this.MyElement);            // �޽� ����
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (QuestObject.manager.GetIndex() < 14)  // �̰��� ����
                {
                    Debug.Log("���� �ش� ���Ұ� ������� �ʾҽ��ϴ�." + QuestObject.manager.GetIndex());
                }
                else
                {
                    this.mySkillStartColor = WaterSkillStartColor;
                    this.mySkillEndColor = WaterSkillEndColor;
                    this.MyElement = ElementType.WATER;
                    Debug.Log("current element : WATER");

                    playerMesh.ChangeClass((int)this.MyElement);            // �޽� ����
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (QuestObject.manager.GetIndex() < 18)  // �̰��� ����
                {
                    Debug.Log("���� �ش� ���Ұ� ������� �ʾҽ��ϴ�." + QuestObject.manager.GetIndex());
                }
                else
                {
                    this.mySkillStartColor = ElectroSkillStartColor;
                    this.mySkillEndColor = ElectroSkillEndColor;
                    this.MyElement = ElementType.ELECTRICITY;
                    Debug.Log("current element : ELECTRICITY");

                    playerMesh.ChangeClass((int)this.MyElement);            // �޽� ����
                }
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
                        //Water Tag�� ���� �����ϸ� ������Ʈ�� Move�� ����.
                        //���������� ���¹̳� �Ҹ� ���Ѿ� ��.
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
                        //���� ����� ������, �ٷ� Move�� ���� �̵��ؾ���.
                        if (Fishing.isPlayerFishing == false)
                        {
                            this.PrevState = this.State;
                            this.NextState = STATE.MOVE;
                        }
                        break;
                    case STATE.DEAD:
                        //������ ������.
                        break;
                }
            }

            // ���°� ��ȭ���� ��------------.
            while (this.NextState != STATE.NONE)
            { // ���°� NONE�̿� = ���°� ��ȭ�ߴ�.
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
            // �� ��Ȳ���� �ݺ��� ��----------.
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
                    //���� �߿��� �ƹ� �ൿ�� ���Ѵ�.
                    break;
                case STATE.DEAD:
                    if (this.gameObject.transform.GetChild(1).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f
                        && !Dead)
                    {
                        Dead = true;
                        LoadingSceneController.Instance.ReloadScene();
                    }
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

    private void move() //�Ϲ������� �̵��� �� ����ϴ� ������ ���.
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
    private void swimming() //���� ��.
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

    private void aimModeMove() //���ظ�� ������ ���� �Ұ�. 50%�� �ӵ��� �̵���.
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

    private void dash() //����
    {
        StartCoroutine(dashCoroutine());
        isDashed = true;
    }
    private IEnumerator dashCoroutine() 
    {
        Vector3 snapGround = Vector3.zero;
        if (player.isGrounded) snapGround = Vector3.down;
        float startTime = Time.time; //�뽬�� ���� �ð�
        while (Time.time < startTime + 0.3f) //�뽬�� ���ӵ� �ð�, 0.3��
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
        projectileLine.startColor = Color.yellow; //�⺻������ �Ӽ����� ����, �������� ������.
        projectileLine.endColor = Color.white; //�⺻������ �Ӽ����� ����, �������� ������.
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

                    //���������Ʈ�� ��ȣ�ۿ� �κ� �߰�
                    if (hitInfo.collider.gameObject.tag == "pattern")
                    {
                        hitInfo.collider.GetComponent<Pattern>().GetP(hitInfo.point, hitInfo.normal);
                    }
                    else if (hitInfo.collider.gameObject.tag == "connect")
                    {
                        hitInfo.collider.GetComponent<Connect>().GetP(hitInfo.point, hitInfo.normal);
                    }

                    //���� ������Ʈ�� ��ȣ�ۿ� �κ� �߰�
                    if (hitInfo.collider.tag == "animal")
                    {
                        if (hitInfo.collider.GetComponent<Deer>().GetComeBack() || hitInfo.collider.GetComponent<Deer>().GetIsDead())
                        {
                            //�ƹ��͵� ���Ѵ�. ���ݾȹ���
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
        projectileLine.startColor = mySkillStartColor; //���� �Ӽ��� ���� ��ų ������ �ٲ��.
        projectileLine.endColor = mySkillEndColor; //���� �Ӽ��� ���� ��ų ������ �ٲ��.
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
            if (MyElement != ElementType.NONE) // ���Ӽ��� �ƴҶ� �󼺹��� ����
            {
                if (hitInfo.collider.tag == "Enemy")
                {
                    setEnemyElement(hitInfo.collider.GetComponent<Enemy>().Stat.element);
                    hitInfo.collider.GetComponent<Enemy>().TakeElementHit(ChargedAttackDamage, MyElement);
                }
                else if (hitInfo.collider.tag == "DungeonBoss")
                {
                    setEnemyElement(hitInfo.collider.GetComponent<DungeonBoss>().Stat.element);
                    hitInfo.collider.GetComponent<DungeonBoss>().TakeElementHit(ChargedAttackDamage, MyElement);
                }
                else if (hitInfo.collider.tag == "FinalBoss")
                {
                    setEnemyElement(hitInfo.collider.GetComponent<FinalBoss>().Stat.element);
                    hitInfo.collider.GetComponent<FinalBoss>().TakeElementHit(ChargedAttackDamage, MyElement);
                }
                else if (hitInfo.collider.tag == "FireBall")
                {
                    //setEnemyElement(hitInfo.collider.GetComponent<FireBallBullet>().Stat.element);
                    hitInfo.collider.GetComponent<FireBallBullet>().TakeElementHit(ChargedAttackDamage, MyElement);
                }
                else if (hitInfo.collider.gameObject.tag == "Shield")
                {
                    StartCoroutine(hitInfo.collider.GetComponent<Shield>().CreateRipple(hitInfo.point));

                    hitInfo.collider.GetComponent<Shield>().TakeElementHit(ChargedAttackDamage, MyElement);
                }
                ElementGauge += ElementGaugeChargeAmount; //���Ұ����� ++
                if (ElementGauge > 100.0f) ElementGauge = 100.0f;
            }
            else
            {
                if (hitInfo.collider.tag == "Enemy")
                {
                    setEnemyElement(hitInfo.collider.GetComponent<Enemy>().Stat.element);
                    hitInfo.collider.GetComponent<Enemy>().TakeHit(ChargedAttackDamage);
                }
                else if (hitInfo.collider.tag == "DungeonBoss")
                {
                    setEnemyElement(hitInfo.collider.GetComponent<DungeonBoss>().Stat.element);
                    hitInfo.collider.GetComponent<DungeonBoss>().TakeHit(ChargedAttackDamage);
                }
                else if (hitInfo.collider.tag == "FinalBoss")
                {
                    setEnemyElement(hitInfo.collider.GetComponent<FinalBoss>().Stat.element);
                    hitInfo.collider.GetComponent<FinalBoss>().TakeHit(ChargedAttackDamage);
                }
                else if (hitInfo.collider.tag == "FireBall")
                {
                    //setEnemyElement(hitInfo.collider.GetComponent<FireBallBullet>().Stat.element);
                    hitInfo.collider.GetComponent<FireBallBullet>().TakeHit(ChargedAttackDamage);
                }
            }

            //���������Ʈ�� ��ȣ�ۿ� �߰�
            if (hitInfo.collider.gameObject.tag == "etrigger")
            {
                hitInfo.collider.GetComponent<Etrigger>().GetElement(MyElement);
            }

            //���� ������Ʈ�� ��ȣ�ۿ� �κ� �߰�
            if (hitInfo.collider.tag == "animal")
            {
                if (hitInfo.collider.GetComponent<Deer>().GetComeBack() || hitInfo.collider.GetComponent<Deer>().GetIsDead())
                {
                    //�ƹ��͵� ���Ѵ�. ���ݾȹ���
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
            ElementGauge += ElementGaugeChargeAmount; //���Ұ����� ++
        }
        if (ElementGauge > 100.0f) ElementGauge = 100.0f;

        StartCoroutine(skillCoolDownCalc());
        StartCoroutine(fallBack()); //����
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
        projectileLine.startColor = mySkillStartColor; //���� �Ӽ��� ���� ��ų ������ �ٲ��.
        projectileLine.endColor = mySkillEndColor; //���� �Ӽ��� ���� ��ų ������ �ٲ��.
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