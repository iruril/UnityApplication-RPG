using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    public static bool isPlayerFishing = false; //플레이어가 그 어떤 낚시 포인트에서 낚시를 하더라도 상태가 일관되게 적용된다.

    //낚시 게이지 바 띄우기
    [SerializeField] protected Slider gageSlider; //게이지 슬라이더
    [SerializeField] protected Slider minSlider; //최소값 슬라이더
    [SerializeField] protected Slider maxSlider; //최대값 슬라이더

    [SerializeField] protected Text fishingText; //성공,실패,시작할 때 숫자 텍스트

    //수치가 정해지면 초기화 나중에 Awake에서 실행
    [SerializeField] protected float min_min = 0.4f; //최소값의 최소값
    [SerializeField] protected float min_max = 0.7f; //최소값의 최대값
    [SerializeField] protected float sliderInterval = 0.3f; //최소값 최대값 사이 길이
    [SerializeField] protected float resultGageSpeed = 0.1f; //게이지 속도
    [SerializeField] protected float fishingTime = 5.0f; //낚시 시간
    [SerializeField] protected float fishingPower = 0.2f; //낚시 힘

    float dtime;

    [SerializeField] protected bool start = false; //시작했는지 아닌지
    [SerializeField] protected bool inputF = false; //f키 눌렀는지 아닌지
    bool endFishing; //낚시가 끝났는지 아닌지 판단.

    protected GameObject Player; //플레이어 오브젝트
    itemJsonData itemJsonData;//json데이터

    private void Awake()
    {
        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "Harvest");//json로드
        Player = GameObject.FindGameObjectWithTag("Player");

        gageSlider.gameObject.SetActive(false);
        minSlider.gameObject.SetActive(false);
        maxSlider.gameObject.SetActive(false);
        fishingText.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        dtime = Time.deltaTime;
        initialization();
    }

    // Update is called once per frame
    void Update()
    {
        Fishing_Start();

        if (Input.GetKeyDown(KeyCode.Space) && start && inputF && endFishing)
        {
            initialization();
        }
    }

    //초기화
    void initialization()
    {
        isPlayerFishing = false; //플레이어가 낚시중인가? No.
        endFishing = true;
        start = false;
        inputF = false;
        gageSlider.value = 0.5f;

        //UI제거
        //점프 못하는거 풀기

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            UI_F();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            UI_F();
        }
    }

    //낚시 F키 띄우기 함수
    void UI_F()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) <= 2.0f && !start && !inputF)
        {
            //f키 생성
            if (!inventoryObject.Inst.FieldFKey.activeSelf)
            {
                inventoryObject.Inst.FieldFKey.SetActive(true);
            }
        }
        else if (Vector3.Distance(Player.transform.position, transform.position) > 2.0f || start || inputF)
        {
            //f키 제거
            inventoryObject.Inst.FieldFKey.SetActive(false);
        }
    }

    //낚시 함수
    void Fishing_Start()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) <= 2.0f && !start && !inputF)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isPlayerFishing = true; //플레이어가 낚시중인가? Yes.
                endFishing = false;
                inputF = true;

                gageSlider.gameObject.SetActive(true);
                minSlider.gameObject.SetActive(true);
                maxSlider.gameObject.SetActive(true);
                fishingText.gameObject.SetActive(true);



                StartCoroutine(Check_Start());

                //시간안가게하기->windowSet에 case늘려서 사용해야 할듯함
                //Time.timeScale = 0f;
            }
        }

        if (start)
        {
            //낚시할때 점프 안하게 하기

            //게이지 감소
            gageSlider.value -= resultGageSpeed * dtime;
            //게이지 증가
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gageSlider.value += fishingPower;
            }
        }
    }

    int num = 3; //시간

    //시작 판단
    IEnumerator Check_Start()
    {
        //랜덤으로 값 주기
        minSlider.value = Random.Range(min_min, min_max);
        maxSlider.value = minSlider.value + sliderInterval;
        //UI 생성

        while (num >= 1 && !start)
        {
            fishingText.text = num.ToString();
            yield return new WaitForSeconds(1.0f);
            num--;
        }
        fishingText.text = "";
        start = true;
        num = 3;
        //낚시 시간
        StartCoroutine(StartFishing());
    }

    //아이템 데이터
    itemData fishData;

    //코루틴 사용해서 끝나는 시간 설정
    IEnumerator StartFishing()
    {
        yield return new WaitForSeconds(fishingTime);

        //아이템 획득
        if (gageSlider.value >= minSlider.value && gageSlider.value <= maxSlider.value)
        {
            fishingText.text = "success";

            fishData = new itemData();

            foreach (var item in itemJsonData.itemList)
            {
                if (item.itemID == 1003)
                {
                    fishData = item;
                }
            }

            Vector2 tempPos;
            tempPos = inventoryObject.Inst.emptyCell(fishData.xSize, fishData.ySize);
            inventoryObject.Inst.itemGet(fishData.xSize, fishData.ySize, tempPos.x, tempPos.y, fishData);

            //인벤토리 추가 및 제이슨 저장
            inventoryObject.Inst.jsonSave();
        }
        else
        {
            fishingText.text = "fail";
        }
        endFishing = true; //끝났다고 알려주기

        yield return new WaitForSeconds(1.0f);

        gageSlider.gameObject.SetActive(false);
        minSlider.gameObject.SetActive(false);
        maxSlider.gameObject.SetActive(false);
        fishingText.gameObject.SetActive(false);

        //시간안가게하기->windowSet에 case늘려서 사용해야 할듯함
        //Time.timeScale = 1f;

        //초기화
        //start = false; 
        //inputF = false;
        //gageSlider.value = 0.5f;

        //yield return new WaitForSeconds(1.0f);
    }
}
