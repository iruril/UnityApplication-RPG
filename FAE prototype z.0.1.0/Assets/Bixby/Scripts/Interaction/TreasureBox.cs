using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    public bool boxState; //상자 활성화 비활성화 여부
    public Material[] mBox; //0.비활성화, 1.활성화, 상자 머테리얼 나중에 쉐이더로 변경

    public bool open; //열렸는지 아닌지

    public GameObject[] contents; //내용물 데이터 베이스가 어떻게 들어오는지가 중요

    public GameObject Player; //플레이어 오브젝트
    public GameObject coin; //코인 오브젝트

    private void Awake()
    {
        // Player = GameObject.FindGameObjectWithTag("Player");
        open = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //상자가 활성화 됬을 때 하는거 전부다.

        BoxMaterial(); //쉐이더or머테리얼 변경
        BoxOpen(); //상자 열기
    }

    void BoxOpen()
    {
        //가까이 갔을 때
        if (Vector3.Distance(Player.transform.position, transform.position) <= 2.0f && boxState)
        {
            //UI 띄우기 f키 누르기 아이콘

            if (Input.GetKeyDown("g"))
            {
                //상자열기
                open = true;
                //아이템 획득, UI?
                

                for (int i = 0; i < 5; i++)
                {
                    float RandomX = Random.Range(-3 + transform.position.x, 3 + transform.position.x);
                    float RandomZ = Random.Range(-3 + transform.position.z, 3 + transform.position.z);

                    Vector3 RandomPos = new Vector3(RandomX, transform.position.y, RandomZ);

                    Instantiate(coin, RandomPos, Quaternion.identity);
                }


                //상자 제거?
                Destroy(gameObject);
            }
        }
    }

    void BoxMaterial()
    {
        if (boxState) //활성화
        {
            //보물상자 쉐이더 변경으로 바꿔야한다.
            GetComponent<MeshRenderer>().material = mBox[1];
        }
        else if (!boxState) //비활성화
        {
            //보물상자 쉐이더 변경으로 바꿔야한다.
            GetComponent<MeshRenderer>().material = mBox[0];
        }
    }
}
