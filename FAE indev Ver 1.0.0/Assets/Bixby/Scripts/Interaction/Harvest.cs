using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : MonoBehaviour
{
    public float respawnTime = 5.0f; //리스폰 시간
    public GameObject field; //스포너 범위 가져올 오브젝트

    public HARVESTSTATE state = HARVESTSTATE.NONE;

    public enum HARVESTSTATE
    {
        NONE = 0,
        FRUIT = 1, //과일
        GREENONION = 2, //파
        RICE = 3, //쌀
    };

    //json데이터
    itemJsonData itemJsonData;
    //아이템 데이터
    itemData harvestData;

    //private MeshRenderer meshTest; //매쉬 렌더러

    public bool testchack = true; //오브젝트 상태 체크
    bool isPlayerClose;

    private void Awake()
    {
        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "harvest");//json로드
    }

    void Start()
    {
        harvestData = new itemData();

        switch (state)
        {
            case HARVESTSTATE.NONE:
                break;
            case HARVESTSTATE.FRUIT:
                //아이템 지정 스크립트
                foreach (var item in itemJsonData.itemList)
                {
                    if (item.itemID == 1000)
                    {
                        harvestData = item;
                    }
                }
                break;
            case HARVESTSTATE.GREENONION:
                //아이템 지정 스크립트
                foreach (var item in itemJsonData.itemList)
                {
                    if (item.itemID == 1001)
                    {
                        harvestData = item;
                    }
                }
                break;
            case HARVESTSTATE.RICE:
                //아이템 지정 스크립트
                foreach (var item in itemJsonData.itemList)
                {
                    if (item.itemID == 1002)
                    {
                        harvestData = item;
                    }
                }
                break;
            default:
                break;
        }
        isPlayerClose = false;
    }

    private void Update()
    {
        if (isPlayerClose && inventoryObject.Inst.FieldFKey.activeSelf)
        {
            var wantedPos = Camera.main.WorldToScreenPoint(this.transform.position);
            inventoryObject.Inst.FieldFKey.transform.position = wantedPos + Vector3.right * 200f;
        }
        if (Input.GetKeyDown(KeyCode.F) && isPlayerClose)
        {
            Vector2 tempPos;
            //빈 공간 찾기
            tempPos = inventoryObject.Inst.emptyCell(harvestData.xSize, harvestData.ySize);
            inventoryObject.Inst.itemGet(harvestData.xSize, harvestData.ySize, tempPos.x, tempPos.y, harvestData);

            //인벤토리 추가 및 제이슨 저장
            inventoryObject.Inst.jsonSave();
            inventoryObject.Inst.FieldFKey.SetActive(false);

            isPlayerClose = false;
            this.gameObject.SetActive(false);
            testchack = false;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //f키 생성
            if (!inventoryObject.Inst.FieldFKey.activeSelf)
            {
                inventoryObject.Inst.FieldFKey.SetActive(true);
                isPlayerClose = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //f키 제거
            inventoryObject.Inst.FieldFKey.SetActive(false);
            isPlayerClose = false;
        }
    }
}
