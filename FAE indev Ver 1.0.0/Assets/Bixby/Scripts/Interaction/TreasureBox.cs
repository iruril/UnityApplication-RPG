using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    public bool boxState; //상자 활성화 비활성화 여부
    public bool open; //열렸는지 아닌지 
    public bool checkNear; //가까이 있는지 체크하는 변수

    public int coinCount; //나오는 골드 수

    public Material[] mBox; //0.비활성화, 1.활성화, 상자 머테리얼 나중에 쉐이더로 변경

    public GameObject[] contents; //내용물 데이터 베이스가 어떻게 들어오는지가 중요 -> 이부분이 제이슨 파일의 데이터를 가져와야 할 거 같음

    public GameObject Player; //플레이어 오브젝트
    public GameObject coin; //코인 오브젝트

    private GameObject top; //상자 머리부분

    itemJsonData itemJsonData;//json데이터

    private void Awake()
    {
        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "shop");//json로드
        Player = GameObject.FindGameObjectWithTag("Player");
        open = false;

        top = transform.GetChild(0).gameObject;
        Quaternion currentRotation = top.transform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //상자가 활성화 됬을 때 하는거 전부다.

        //BoxMaterial(); //쉐이더or머테리얼 변경
        BoxOpen(); //상자 열기
    }

    void BoxOpen()
    {
        //가까이 갔을 때
        if (Vector3.Distance(Player.transform.position, transform.position) <= 2.0f && boxState && !open)
        {
            checkNear = true;

            //f키 생성
            if (!inventoryObject.Inst.FieldFKey.activeSelf)
            {
                inventoryObject.Inst.FieldFKey.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.F) && !open)
            {
                //상자열기
                open = true;
                //아이템 획득, UI?

                //아이템 생성 contents 에 이 상자를 열었을 때 획득 할 수 있는 아이템 집어넣기 확률계산 이런건 나중에
                //GameObject TreasureItem = inventoryObject.Inst.MakeFieldItem(itemJsonData.itemList[0], transform.position);
                //TreasureItem.GetComponent<fieldItem>().setup(itemJsonData.itemList[0], true);

                //코인 생성
                for (int i = 0; i < coinCount; i++)
                {
                    int RandomX = Random.Range(-40, 40);
                    int RandomY = Random.Range(0, 360);
                    int RandomZ = Random.Range(50, 130);

                    GameObject _coin = Instantiate(coin, transform.position + new Vector3(0, 2, 0), Quaternion.Euler(new Vector3(RandomX, RandomY, RandomZ)));

                    _coin.GetComponent<Rigidbody>().AddForce(Vector3.up * 3, ForceMode.Impulse); //위로 힘주기
                    _coin.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 5, ForceMode.Impulse); //보고있는 방향으로 힘주기
                }

                //열린상태로 놔두기
                StartCoroutine(BoxAni());

                //f키 제거
                inventoryObject.Inst.FieldFKey.SetActive(false);
                checkNear = false;
            }
        }
        else if (Vector3.Distance(Player.transform.position, transform.position) > 2.0f && boxState && checkNear == true)
        {
            //f키 제거
            inventoryObject.Inst.FieldFKey.SetActive(false);

            checkNear = false;
        }
    }

    //이펙트 키는걸로 변경
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

    //박스 열리는 거 코루틴
    IEnumerator BoxAni()
    {
        for (int i = 0; i < 60; i++)
        {
            top.transform.Rotate(new Vector3(-1, 0, 0));
            yield return null;
        }
    }
}
