using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdleStatus : MonoBehaviour
{
    public float HurdleDamage = 10.0f;
    public float moveSpeed; //움직이는 장애물 속도
    public float downPlayerSpeed; //플레이어 줄어드는 속도

    public List<GameObject> traget;
    public int nowTraget = 0; //현재 번호
    public bool isMove = false; //현재 움직이고 있는지

    public List<GameObject> nearobj;


    public HURDLE_STATE State = HURDLE_STATE.NONE;


    public enum HURDLE_STATE
    {
        NONE = -1, //아무상태 아닌거
        HURDLE = 0, //장애물
        SLOW_HURDLE = 1, //느려지는 장애물
        NEAR_HURDLE = 2, //가까이 가면 생성되는 장애물
        MOVE_HURDLE = 3  //움직이는장애물
    }


    // Update is called once per frame
    void Update()
    {
        if (State == HURDLE_STATE.MOVE_HURDLE)
        {
            move();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //플레이어에게 데미지 입히기
        if (other.gameObject.tag == "Player" && State == HURDLE_STATE.HURDLE)
        {
            //이부분은 합칠때 다른 함수 사용해서 변경
            other.GetComponent<PlayerStatusControl>().Health -= HurdleDamage;
        }

        //플레이어 슬로우
        if (other.gameObject.tag == "Player" && State == HURDLE_STATE.SLOW_HURDLE)
        {
            //이부분은 합칠때 다른 함수 사용해서 변경
            other.GetComponent<PlayerStatusControl>().MyCurrentSpeed -= downPlayerSpeed;
        }
    }

    //일정 범위 안에 있으면 가까이 갔을 때 생기는 오브젝트 실행
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && State == HURDLE_STATE.NEAR_HURDLE)
        {
            for (int i = 0; i < nearobj.Count; i++)
            {
                nearobj[i].gameObject.SetActive(true);
            }
        }
    }

    //느려지는거 제거
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && State == HURDLE_STATE.SLOW_HURDLE)
        {
            other.GetComponent<PlayerStatusControl>().MyCurrentSpeed += downPlayerSpeed;
        }

        if (other.gameObject.tag == "Player" && State == HURDLE_STATE.NEAR_HURDLE)
        {
            for (int i = 0; i < nearobj.Count; i++)
            {
                nearobj[i].gameObject.SetActive(false);
            }
        }
    }

    private void move()
    {
        //현재 타겟의 포지션이 오브젝트의 포지션하고 같아지면
        if (transform.position == traget[nowTraget].transform.position)
        {
            nowTraget = (nowTraget + 1) % traget.Count;
        }

        //움직여라
        transform.position = Vector3.MoveTowards(transform.position, traget[nowTraget].transform.position, moveSpeed);
    }
}
