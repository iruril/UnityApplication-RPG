using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class itemObject : MonoBehaviour
{
    [SerializeField] GameObject image;//setup에서 설정 필요
    bool isHover;
    
    Vector3 zeroPos;
    Vector3 size;
    public Vector3 OriginPos;
    bool isEquip;
    GameObject equip;
    public itemData ItemData;
    public void Setup(float sizeX, float sizeY, float posX, float posY, bool isEquip, Vector3 zero, itemData itemData)
    {
        this.zeroPos = zero;
        float cell = inventoryObject.Inst.Cell;

        this.size = new Vector3(sizeX * cell, sizeY * cell, 1f);
        this.GetComponent<RectTransform>().sizeDelta = this.size;

        this.gameObject.transform.localPosition = new Vector3(
                    zeroPos.x + posX * cell + size.x / 2f,
                    zeroPos.y - posY * cell - size.y / 2f);
        this.OriginPos = this.transform.localPosition;

        equip = this.transform.GetChild(0).gameObject;
        this.isEquip = isEquip;
        equip.SetActive(isEquip);

        this.ItemData = itemData;
        Color imageColor = new Color();
        switch (this.ItemData.itemID)
        {
            case 0:
                 imageColor = Color.red;
                break;
            case 1:
                imageColor = Color.green;
                break;
            case 2:
                imageColor = Color.blue;
                break;
            case 3:
                imageColor = Color.yellow;
                break;
            default:
                imageColor = Color.black;
                break;
        }
        imageColor.a = 0.5f;
        this.gameObject.GetComponent<Image>().color = imageColor;
    }
    #region 이벤트트리거
    public void Drag()
    {
        if (ItemData.isSell)
            return;//판매 물품은 드래그 되지않음
        if (Input.GetMouseButton(1))
            return;//우클릭은 드래그로 취급안함
        this.transform.position = Input.mousePosition;//Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public void Up()
    {
        #region 위치계산
        float xPos = this.transform.localPosition.x;
        float yPos = this.transform.localPosition.y;
        float xSiz = this.size.x / 2f;
        float ySiz = this.size.y / 2f;
        float Cell = inventoryObject.Inst.Cell;
        float XSize = inventoryObject.Inst.XSize;
        float YSize = inventoryObject.Inst.YSize;
        float tempL = ((xPos - xSiz) + (Cell / 2f)) % Cell;
        Vector3 maxPos = zeroPos + (Vector3.right * XSize * Cell) + (Vector3.down * YSize * Cell);
        xPos -= tempL;

        if (tempL < -(Cell / 2f))
            xPos -= Cell;

        else if (tempL > (Cell / 2f))
            xPos += Cell;
        xPos = zeroPos.x % Cell == 0 ? xPos - Cell / 2f : xPos;

        float tempU = ((yPos + ySiz) + (Cell / 2f)) % Cell;
        yPos -= tempU;

        if (tempU < -(Cell / 2f))
            yPos -= Cell;

        else if (tempU > (Cell / 2f))
            yPos += Cell;
        yPos = zeroPos.y % Cell == 0 ? yPos + Cell / 2f : yPos;
        #endregion
        #region 구매
        if (ItemData.isSell)
        {
            if (Input.GetMouseButtonUp(0))
                return;//좌클릭이었을 경우에는 Down에서 한 함수만이 실행됨
            if (Mathf.FloorToInt(ItemData.price * 1.5f) < inventoryObject.Inst.Gold)
            {
                Vector2 tempPos = inventoryObject.Inst.emptyCell(ItemData.xSize, ItemData.ySize);
                if (tempPos != Vector2.zero - Vector2.one)
                {
                    inventoryObject.Inst.itemGet(ItemData.xSize, ItemData.ySize, tempPos.x, tempPos.y, ItemData);
                    inventoryObject.Inst.Gold -= Mathf.FloorToInt(ItemData.price * 1.5f);
                    inventoryObject.Inst.setGold();
                }
                //알림창 만들면 else에서 칸 부족하다고 알릴 것
            }
            inventoryObject.Inst.setItemPos(this.gameObject, OriginPos);
            return;
        }
        #endregion
        #region 판매
        if (UI_Control.Inst.Shop.getWindow().activeSelf
            )
        {
            Vector3 thisPos = this.transform.position;
            Vector3 thisSize = Vector3.zero + Vector3.right * this.GetComponent<RectTransform>().rect.width + Vector3.up * this.GetComponent<RectTransform>().rect.height;
            Vector3 shopPos = UI_Control.Inst.Shop.getWindow().transform.position;
            Vector3 shopSize = Vector3.zero + Vector3.right * UI_Control.Inst.Shop.getWindow().GetComponent<RectTransform>().rect.width + Vector3.up * UI_Control.Inst.Shop.getWindow().GetComponent<RectTransform>().rect.height;
            float itemMinX = thisPos.x - thisSize.x / 2f;            float itemMaxX = thisPos.x + thisSize.x / 2f;
            float shopMinX = shopPos.x - shopSize.x / 2f;            float shopMaxX = shopPos.x + shopSize.x / 2f;
            float itemMinY = thisPos.y - thisSize.y / 2f;            float itemMaxY = thisPos.y + thisSize.y / 2f;
            float shopMinY = shopPos.y - shopSize.y / 2f;            float shopMaxY = shopPos.y + shopSize.y / 2f;
            if (shopMinX < itemMinX && itemMaxX < shopMaxX
                && shopMinY < itemMinY && itemMaxY < shopMaxY)
            {
                //알림창 만들면 여기에서 팔건지 물어볼 것
                inventoryObject.Inst.Gold += this.ItemData.price;
                inventoryObject.Inst.throwItem(this.gameObject, false);
                inventoryObject.Inst.setGold();
            }
            else
            {
                if (
            (xPos - xSiz) < zeroPos.x
            || (yPos + ySiz) > zeroPos.y
            || (xPos + xSiz) > maxPos.x
            || (yPos - ySiz) < maxPos.y
            )
                    inventoryObject.Inst.setItemPos(this.gameObject, OriginPos);
                else
                {
                    inventoryObject.Inst.setItemPos(this.gameObject, new Vector3(xPos, yPos, 0f));
                }
            }

        }
        #endregion
        else
        {
            if (
            (xPos - xSiz) < zeroPos.x
            || (yPos + ySiz) > zeroPos.y
            || (xPos + xSiz) > maxPos.x
            || (yPos - ySiz) < maxPos.y
            )
                inventoryObject.Inst.throwItem(this.gameObject, true);
            else
            {
                inventoryObject.Inst.setItemPos(this.gameObject, new Vector3(xPos, yPos, 0f));
            }
        }
    }
    public void Hover()
    {
        isHover = true;
        inventoryObject.Inst.itemHover(this);
    }
    public void Exit()
    {
        isHover = false;
        inventoryObject.Inst.itemExit();
    }
    public void Down()//마우스 클릭
    {
        if (Input.GetMouseButtonDown(0))
            inventoryObject.Inst.itemLeftDown(this);
        else if (Input.GetMouseButtonDown(1))
            inventoryObject.Inst.itemRightDown(this);
    }
    #endregion
    void Update()
    {   
        if (isHover)
        {
            inventoryObject.Inst.itemSummaryMove();
        }
    }
    public void equipItem()
    {
        isEquip = !isEquip;
        equip.SetActive(isEquip);
    }
}
