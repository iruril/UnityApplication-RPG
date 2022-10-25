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

        if (!modelContorl.GetIsGrounded() && !animator.GetCurrentAnimatorStateInfo(0).IsName("Landing"))      // 공중에 뜬 상태이며 착지 상태가 아닐 때
        {
            animator.SetBool("isCombat", true);
            animator.SetBool("isJump", true);   // Jump 애니메이션 실행

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")               // 공중에 떠 있으면서(상위 조건문) 점프 애니메이션이 실행중인 상태고,
                && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.475f)  // 애니메이션 진행 상황이 47.5&을 넘어갔을 때
            {
                animator.SetBool("isFall", true);   // Fall 애니메이션 실행
            }
        }
        else                                    // 지상에 있는 상태일 때
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))               // 점프 애니메이션이 실행중인 상태일 때
            {
                animator.SetBool("isLanding", true);
            }
            else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Falling"))      // 추락 애니메이션이 실행중인 상태일 때
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

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Dash") &&                // Dash 애니메이션이 실행되는 상태에서
            Input.GetKeyDown(KeyCode.LeftShift))                                    // LeftShift 입력이 발생할 때
        {
            animator.Play("Dash", 0);
        }

        if(animator.GetCurrentAnimatorStateInfo(1).IsName("Charge") &&              // Charge 애니메이션이 실행되는 상태에서
            !Input.GetKey(KeyCode.Mouse0))                                         // 좌 클릭 입력이 진행 중이지 않을 때
        {
            animator.SetBool("isCharge", false);                                    // 애니메이션 굳는 버그 수정 용
        }

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
                animator.SetBool("isAtk", true);                        // 공격 애니메이션 실행
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
}
