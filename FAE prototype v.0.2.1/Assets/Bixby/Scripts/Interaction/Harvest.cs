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



    //아이템 데이터
    itemData harvestData;

    private MeshRenderer meshTest; //매쉬 렌더러
    bool isPlayerClose;
    void Start()
    {
        harvestData = new itemData();

        switch (state)
        {
            case HARVESTSTATE.NONE:
                break;
            case HARVESTSTATE.FRUIT:
                //아이템 지정 스크립트
                harvestData.itemID = 1000; harvestData.tag = new string[] { "food", "harvest" }; harvestData.itemName = "과일";
                harvestData.Left = -1; harvestData.Up = -1; harvestData.xSize = 1; harvestData.ySize = 1;
                harvestData.isEquip = false; harvestData.isSell = false;
                harvestData.price = 1; //나중에 가격 변경
                break;
            case HARVESTSTATE.GREENONION:
                //아이템 지정 스크립트
                harvestData.itemID = 1001; harvestData.tag = new string[] { "food", "harvest" }; harvestData.itemName = "파";
                harvestData.Left = -1; harvestData.Up = -1; harvestData.xSize = 1; harvestData.ySize = 1;
                harvestData.isEquip = false; harvestData.isSell = false;
                harvestData.price = 1; //나중에 가격 변경
                break;
            case HARVESTSTATE.RICE:
                //아이템 지정 스크립트
                harvestData.itemID = 1002; harvestData.tag = new string[] { "food", "harvest" }; harvestData.itemName = "쌀";
                harvestData.Left = -1; harvestData.Up = -1; harvestData.xSize = 1; harvestData.ySize = 1;
                harvestData.isEquip = false; harvestData.isSell = false;
                harvestData.price = 1; //나중에 가격 변경
                break;
            default:
                break;
        }

        


        meshTest = gameObject.GetComponent<MeshRenderer>();
        isPlayerClose = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && meshTest.enabled == true&& isPlayerClose)
        {
            //Debug.Log("cccc");
            this.meshTest.enabled = false;

            StartCoroutine(respawn());
            //아이템 획득 -> 인벤토리랑 연동

            Vector2 tempPos;
            //빈 공간 찾기
            tempPos = inventoryObject.Inst.emptyCell(harvestData.xSize, harvestData.ySize);
            inventoryObject.Inst.itemGet(harvestData.xSize, harvestData.ySize, tempPos.x, tempPos.y, harvestData);

            //인벤토리 추가 및 제이슨 저장
            inventoryObject.Inst.jsonSave();
            inventoryObject.Inst.FieldFKey.SetActive(false);
        }
    }
    //동시에 획득하는거 수정해야함 -> 가장 가까이 있는 것 만 제거 어케하지?
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.name);

        if (other.gameObject.tag == "Player")
        {
            //Debug.Log(other.name);

            //f키 생성
            if (!inventoryObject.Inst.FieldFKey.activeSelf && this.meshTest.enabled == true)
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
            inventoryObject.Inst.FieldFKey.SetActive(false);
        }
    }


    IEnumerator respawn()
    {
        //gameObject.SetActive(false);
        yield return new WaitForSeconds(respawnTime);

        //위치 랜덤 이동
        transform.position = field.GetComponent<HarvestField>().Return_RandomPosition();

        //gameObject.SetActive(true);
        this.meshTest.enabled = true;

    }
}
