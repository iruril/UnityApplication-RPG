using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    itemJsonData itemJsonData;//json데이터
    List<itemData> shopData;
    GameObject shopWindow;
    [SerializeField] float shopXSize;
    [SerializeField] float shopYSize;
    List<GameObject> shopItems;
    void Awake()
    {
        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "shop");//json로드
        shopData = itemJsonData.itemList;
        shopWindow = GameObject.Find("Shop");
        shopWindow.SetActive(false);
    }
    private void Start()
    {
        shopWindow.GetComponent<RectTransform>().sizeDelta = new Vector2(shopXSize * inventoryObject.Inst.Cell, shopYSize * inventoryObject.Inst.Cell);//상점창 크기
        float Cell = inventoryObject.Inst.Cell;
        Vector3 zero = new Vector3(shopXSize * Cell / -2f, shopYSize * Cell / 2f, 0f);//json저장용 좌표 0,0의 실제 위치
        for (int i = 0; i < shopYSize; i++)
        {
            for (int j = 0; j < shopXSize; j++)
            {
                GameObject temp = Instantiate(inventoryObject.Inst.getObj("Cell"), shopWindow.transform.GetChild(0));
                temp.GetComponent<RectTransform>().sizeDelta = new Vector2(Cell * 0.9f, Cell * 0.9f);
                temp.transform.localPosition = new Vector3(j * Cell + (zero.x + Cell / 2f), i * -Cell + (zero.y - Cell / 2f), 0f);
            }
        }//칸 생성
        List<itemData> tempList = new List<itemData>();
        foreach (var item in shopData)
        {
            GameObject temp = Instantiate(inventoryObject.Inst.getObj("itemPrefab"), shopWindow.transform);//인스턴싱
            itemObject tempItem = temp.GetComponent<itemObject>();//오브젝트 변수
            
            float newXSize = item.xSize;
            float newYSize = item.ySize;
            List<Vector2> existCell = inventoryObject.Inst.existCells(tempList);
            List<Vector2> emptyCell = new List<Vector2>();
            for (int j = 0; j < shopYSize; j++) //칸 기준
            {
                for (int i = 0; i < shopXSize; i++)
                {
                    if (existCell.Contains(new Vector2(i, j)))
                        continue;
                    emptyCell.Add(new Vector2(i, j));
                }
            }
            foreach (var item_ in emptyCell)
            {
                bool isOkay = true;
                for (int i = 0; i < newYSize; i++)
                {
                    Vector2 tempPoint = item_ + Vector2.up * i;
                    for (int j = 0; j < newXSize; j++)
                    {
                        if (!emptyCell.Contains(tempPoint + Vector2.right * j))
                            isOkay = false;
                    }
                }
                if (isOkay)
                {
                    item.Left = item_.x;
                    item.Up = item_.y;
                }
            }
            tempItem.Setup(item.xSize, item.ySize, item.Left, item.Up, false, zero, item);
            tempList.Add(tempItem.ItemData);
        }//아이템 가시화
    }
    public GameObject getWindow()
    {
        return shopWindow;
    }
    public void SetUp()
    {
        UI_Control.Inst.windowClose();
        UI_Control.Inst.windowSet(shopWindow);
        shopItems = new List<GameObject>();
        foreach (Transform item in shopWindow.transform)
        {
            if (item.gameObject.name != "cells")
                shopItems.Add(item.gameObject);
        }
        foreach (var item in shopItems)
        {
            item.SetActive(true);
            if (item.transform.parent.name == "Shop")
                item.GetComponent<itemObject>().ItemData.isSell = true;
        }
    }
}
