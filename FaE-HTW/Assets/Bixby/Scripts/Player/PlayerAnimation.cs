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
        animator.SetBool("isRun", false);
        animator.SetBool("isJump", false);
        animator.SetBool("isAtk", false);
        animator.SetBool("isFall", false);
        animator.SetBool("isDead", false);
    }

    // Update is called once per frame
    void Update()
    {
        // 우선 구현을 위해 switch가 아닌 if를 사용했으니 State 관련 정리가 완료되면 switch 형태로 구현 지향할 것

        //if(체력이 다 달았을 때)
        //    {
        //    animator.SetBool("isDead", true);
        //}

        if (!modelContorl.GetIsGrounded())      // 공중에 뜬 상태일 때
        {
            animator.SetBool("isJump", true);   // Jump 애니메이션 실행

            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")               // 공중에 떠 있으면서(상위 조건문) 점프 애니메이션이 실행중인 상태고,
                && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)  // 애니메이션 진행 상황이 90&을 넘어갔을 때
            {
                //animator.SetBool("isFall", true);   // Fall 애니메이션 실행
            }
        }
        else
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isFall", false);
        }


        //if (modelContorl.State == PlayerContorl.STATE.MOVE) // Move 상태일 때
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) 
            || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))  // WASD 입력이 발생할 때
            animator.SetBool("isRun", true);                        // 이동 애니메이션 실행
        else animator.SetBool("isRun", false);

        // 아무튼 공격 상태일 때(모션이 아직 없어서 임의로 설정)
        if (modelContorl.State == PlayerContorl.STATE.ATTACK || modelContorl.State == PlayerContorl.STATE.CHARGE_ATTACK || modelContorl.State == PlayerContorl.STATE.ELEMENT_ULT_SKILL)
            animator.SetBool("isAtk", true);    // 공격 애니메이션 실행
        else animator.SetBool("isAtk", false);
    }
}
