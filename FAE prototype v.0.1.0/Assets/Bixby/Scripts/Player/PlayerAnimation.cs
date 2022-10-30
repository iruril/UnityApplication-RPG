using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerContorl modelContorl; // 플레이어 모델의 PlayerContorl 스크립트
    Animator animator;          // Animator 컴포넌트

    // Start is called before the first frame update
    void Start()
    {
        modelContorl = this.gameObject.GetComponent<PlayerContorl>();
        animator = this.gameObject.transform.GetChild(1).GetComponent<Animator>();

        // Animator 변수 초기화
        initializeAnimParameter();
    }

    // Update is called once per frame
    void Update()
    {
        //if(체력이 다 달았을 때)
        //    {
        //    animator.SetBool("isDead", true);
        //}

        // 대시 재실행 설정
        reDashCheck();

        // 착지 대시 설정
        landingDashCheck();

        // 점프 관련 설정
        jumpCheck();

        // 충전 관련 설정
        chargeCheck();

        // 궁극기 관련 설정
        ultCheck();

        // 그 외 상태 별 설정
        switch (modelContorl.State)
        {
            case PlayerContorl.STATE.IDLE:                              // 비전투 상태
                animator.SetBool("isCombat", false);                    // 비전투 상태 활성화
                animator.SetBool("isAim", false);                       // 조준 애니메이션 정지
                break;

            case PlayerContorl.STATE.MOVE:                              // 이동 상태
                animator.SetBool("isCombat", true);                     // 비전투 상태 비활성화
                animator.SetBool("isAtk", false);                       // 공격 애니메이션 정지
                animator.SetBool("isAim", false);                       // 조준 애니메이션 정지
                animator.SetBool("isSwimming", false);                  // 수영 애니메이션 정지
                animator.SetBool("isDash", false);                      // 대시 애니메이션 정지
                animator.SetBool("isUlt", false);                       // 궁극기 애니메이션 정지

                moveCheck();

                break;

            case PlayerContorl.STATE.DASH:                              // 대시 상태
                animator.SetBool("isDash", true);                       // 대시 애니메이션 실행
                break;

            case PlayerContorl.STATE.RUN:                               // 달리기 상태
                animator.SetBool("isDash", false);                      // 대시 애니메이션 정지
                moveCheck();
                break;

            case PlayerContorl.STATE.SWIMMING:                          // 수영 상태
                animator.SetBool("isSwimming", true);                   // 수영 애니메이션 실행
                break;

            case PlayerContorl.STATE.ATTACK:                            // 약 공격 상태
                animator.SetBool("isCombat", true);                     // 비전투 상태 비활성화
                animator.SetBool("isAtk", true);                        // 공격 애니메이션 실행

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Normal Attack")                 // 공격 애니메이션이 실행중인 상태고,
                && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f                   // 애니메이션 진행 상황이 90&을 넘어갔고
                && Input.GetMouseButton(0))                                                         // 마우스 좌 클릭 입력이 활성화되어 있을 때
                {
                    animator.SetBool("isCharge", true);                 // 차지 상태 활성화
                }

                moveCheck();

                break;

            case PlayerContorl.STATE.CHARGE_ATTACK:                     // 강 공격 상태
                if(Input.GetMouseButtonUp(0))                           // 마우스 좌 클릭 입력이 해제됐다면
                    animator.SetBool("isCharge", false);                // 차지 상태 해제
                else animator.SetBool("isCharge", true);                // 차지 상태 유지

                moveCheck();

                break;

            case PlayerContorl.STATE.ELEMENT_ULT_SKILL:                 // 궁극기 사용 상태
                animator.SetBool("isCombat", true);                     // 비전투 상태 비활성화
                animator.SetBool("isUlt", true);                        // 궁극기 애니메이션 실행
                break;

            case PlayerContorl.STATE.AIM:                               // 조준 상태
                animator.SetBool("isAim", true);                        // 조준 애니메이션 실행
                animator.SetBool("isAtk", false);                       // 공격 애니메이션 정지

                moveCheck();
                break;
        }
    }

    void initializeAnimParameter()        // 애니메이션 패러미터 초기화 함수
    {
        animator.SetBool("isRun", false);
        animator.SetBool("isJump", false);
        animator.SetBool("isAtk", false);
        animator.SetBool("isFall", false);
        animator.SetBool("isCombat", false);
        animator.SetBool("isCharge", false);
        animator.SetBool("isAim", false);
        animator.SetBool("isLanding", false);
        animator.SetBool("isSwimming", false);
        animator.SetBool("isDash", false);
        animator.SetBool("isUlt", false);
    }


    void moveCheck()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)
        || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))  // WASD 입력이 발생할 때
        {
            animator.SetBool("isRun", true);                    // 이동 애니메이션 실행
        }
        else animator.SetBool("isRun", false);                  // 이동 애니메이션 정지

        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Aim"))   // 현재 조준 상태일 때
            animator.SetLayerWeight(1, 0.6f);                   // 가중치 감소
        else animator.SetLayerWeight(1, 0.9f);                   // 가중치 증가
    }

    void jumpCheck()
    {
        if (!modelContorl.GetIsGrounded() && !animator.GetCurrentAnimatorStateInfo(0).IsName("Landing") && // 공중에 뜬 상태이며 착지 상태가 아니고
            !animator.GetCurrentAnimatorStateInfo(2).IsName("Ultimate"))                                   // 궁극기 애니메이션이 실행되지 않는 상태일 때(발사 선후 애니메이션 삭제를 막기 위해 상태 조건과 나눠서 처리)

        {
            animator.SetBool("isCombat", true);
            animator.SetBool("isJump", true);   // Jump 애니메이션 실행

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")               // 공중에 떠 있으면서(상위 조건문) 점프 애니메이션이 실행중인 상태고,
                && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.475f)  // 애니메이션 진행 상황이 47.5&을 넘어갔을 때
            {
                animator.SetBool("isFall", true);   // Fall 애니메이션 실행
            }
        }
        else if (modelContorl.GetIsGrounded())                                        // 지상에 있는 상태일 때
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))               // 점프 애니메이션이 실행중인 상태일 때
            {
                animator.SetBool("isLanding", true);
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Falling"))      // 추락 애니메이션이 실행중인 상태일 때
            {
                animator.SetBool("isLanding", true);
            }
            else                                                                    // 지상에 있는 상태고, 점프, 추락 애니메이션이 아닐 때
            {
                animator.SetBool("isJump", false);
                animator.SetBool("isFall", false);
                animator.SetBool("isLanding", false);
            }
        }
    }

    void chargeCheck()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Charge") &&              // Charge 애니메이션이 실행되는 상태에서
                !Input.GetKey(KeyCode.Mouse0))                                         // 좌 클릭 입력이 진행 중이지 않을 때
        {
            animator.SetBool("isCharge", false);                                    // 애니메이션 굳는 버그 수정 용
        }
    }

    void landingDashCheck()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Landing") &&            // Landing 애니메이션이 실행중인 상태에서
        Input.GetKeyDown(KeyCode.LeftShift))                                        // LeftShift 입력이 발생할 때
        {
            animator.SetBool("isLanding", false);                                   // 착지 애니메이션 정지
            animator.SetBool("isDash", true);                                       // 대시 애니메이션 실행
            animator.Play("Dash", 0);                                               // Dash 애니메이션을 0부터 강제 실행
        }
    }

    void reDashCheck()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dash") &&                // Dash 애니메이션이 실행되는 상태에서
     animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f &&      // 애니메이션 진행 상황이 90%를 넘어갔고
    Input.GetKeyDown(KeyCode.LeftShift))                                    // LeftShift 입력이 발생할 때
        {
            animator.Play("Dash", 0);                                               // Dash 애니메이션을 0부터 실행
        }
    }

    void ultCheck()
    {
        //Debug.Log("현재 애니메이션 속도 : " + animator.speed);
        if (animator.GetCurrentAnimatorStateInfo(2).IsName("Ultimate"))         // 현재 궁극기 애니메이션을 재생 중일 때
        {
            if (modelContorl.State != PlayerContorl.STATE.ELEMENT_ULT_SKILL)     // 궁극기 사용 상태가 아닐 때(switch에 넣으면 중복이 많아서 분리함)
            {
                if(animator.speed == 0)                    // 애니메이션 속도가 0일 때(발사하지 않고 궁극기가 종료되버린 상태)
                {
                    animator.Play("none", 2);              // 애니메이션 None으로 변환
                    animator.speed = 1f;                       // 애니메이션 속도 정상화
                }
            }

            if (animator.GetCurrentAnimatorStateInfo(2).normalizedTime >= 0.45f && // 궁극기 애니메이션 진행 상황이 45%를 넘어갔고
                (animator.speed == 1f || animator.speed == 0f))                    // 애니메이션의 속도가 1f 혹은 0f일 때(이미 좌클릭을 누르지 않았을 때, Anim이 정지되어 있을 때)             
            {
                //Debug.Log("반 넘음");
                if (!Input.GetKeyDown(KeyCode.Mouse0))               // 마우스 좌클릭 입력이 발생하지 않는다면
                {
                    animator.speed = 0f;                            // 애니메이션 일시 정지
                }
                else                                                // 마우스 좌클릭 입력이 발생했다면
                {
                    //Debug.Log("발사");
                    animator.speed = 2.5f;                       // 발사 부분 애니메이션 재생
                }
            }
            else if (animator.GetCurrentAnimatorStateInfo(2).normalizedTime < 0.45f && // 궁극기 애니메이션 진행 상황이 45%보다 낮고
                Input.GetKeyDown(KeyCode.Mouse0))                                     // 마우스 좌클릭 입력이 발생했을 때
            {
                //Debug.Log("빨리발사");
                animator.Play("immediateUltimate", 2);                          // 즉발 애니메이션 실행
            }
        }
        else                                                                   // 궁극기 애니메이션을 실행 중이 아닐 때
        {
                animator.speed = 1f;                       // 애니메이션 속도 정상화
        }
    }
}
