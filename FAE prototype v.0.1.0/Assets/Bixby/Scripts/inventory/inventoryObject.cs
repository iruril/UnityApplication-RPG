using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventoryObject : MonoBehaviour
{
    public static inventoryObject Inst { get; private set; }
    private void Awake()
    {
        Inst = this;
        items = (itemSO)ScriptableObject.CreateInstance("itemSO");
        //itemPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Bixby/Prefab/UI/item.prefab", typeof(GameObject)) as GameObject;
        inventoryCanvas = GameObject.Find("Inventory");
        inventoryObj = inventoryCanvas.transform.GetChild(0).gameObject;
        //cellObj = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Bixby/Prefab/UI/cell.prefab", typeof(GameObject)) as GameObject;
        goldObj = inventoryCanvas.transform.GetChild(1).gameObject;
        //fieldItemPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Bixby/Prefab/Item/fieldItem.prefab", typeof(GameObject)) as GameObject;
        itemSummary = inventoryCanvas.transform.GetChild(2).gameObject;
        itemDescription = inventoryCanvas.transform.GetChild(3).gameObject;
        closeBtn = inventoryCanvas.transform.GetChild(4).gameObject.GetComponent<Button>();
        closeBtn.onClick.AddListener(() => UI_Control.Inst.windowClose());

        itemJsonData = json.LoadJsonFile<itemJsonData>(Application.dataPath, "items");//json로드
        Gold = itemJsonData.gold;//골드 값 로드
        goldObj.GetComponent<RectTransform>().sizeDelta = new Vector2(XSize * Cell, 50f);//골드창 크기
        goldObj.transform.localPosition = new Vector3(0f, -1 * (inventoryObj.GetComponent<RectTransform>().rect.height / 2f + 25f), 0f);//골드창 위치
        setGold();
        items.items = new List<itemData>();
        foreach (var item in itemJsonData.itemList)
            items.items.Add(item);//itemSO 데이터 추가
    }
    itemSO items;//itemSO, 인벤토리 내 아이템들을 이 안의 items에 저장해둔다
    [SerializeField] GameObject itemPrefab;//아이템 프리팹, 인벤토리 열때, 혹은 아이템 획득할때 인스턴트에 사용
    itemJsonData itemJsonData;//json데이터
    GameObject inventoryCanvas;
    GameObject inventoryObj;
    [SerializeField] GameObject cellObj;
    GameObject goldObj;
    public float Cell;
    public float XSize;
    public float YSize;
    List<GameObject> itemObjects;
    Vector3 zero;
    public int Gold;
    [SerializeField] GameObject fieldItemPrefab;
    public GameObject FieldFKey;
    [SerializeField] GameObject keyF;
    GameObject itemSummary;
    GameObject itemDescription;
    Button closeBtn;
    void Start()
    {
        inventoryObj.GetComponent<RectTransform>().sizeDelta = new Vector2(XSize * Cell, YSize * Cell);//인벤창 크기
        closeBtn.gameObject.transform.position
            = inventoryObj.transform.position
            + Vector3.up * (inventoryObj.GetComponent<RectTransform>().rect.height / 2f + Cell / 2f)
            + Vector3.right * (inventoryObj.GetComponent<RectTransform>().rect.width / 2f + Cell / 2f);
        zero = new Vector3(XSize * Cell / -2f, YSize * Cell / 2f, 0f);//json저장용 좌표 0,0의 실제 위치
        for (int i = 0; i < YSize; i++)
        {
            for (int j = 0; j < XSize; j++)
            {
                GameObject temp = Instantiate(cellObj, inventoryObj.transform);
                temp.GetComponent<RectTransform>().sizeDelta = new Vector2(Cell * 0.9f, Cell * 0.9f);
                temp.transform.localPosition = new Vector3(j * Cell + (zero.x + Cell / 2f), i * -Cell + (zero.y - Cell / 2f), 0f);
            }
        }//칸 생성
        itemObjects = new List<GameObject>();
        foreach (var item in items.items)
        {
            GameObject temp = Instantiate(itemPrefab, inventoryCanvas.transform);
            itemObject tempItem = temp.GetComponent<itemObject>();
            tempItem.Setup(item.xSize, item.ySize, item.Left, item.Up, item.isEquip, zero, item);
            itemObjects.Add(temp);
        }//아이템 가시화
        inventoryCanvas.SetActive(false);//시작시 인벤창 닫혀있음
        FieldFKey = null;
        itemSummary.SetActive(false);
        itemDescription.SetActive(false);
    }

    public void setItemPos(GameObject item, Vector3 newOrigonPos)//인벤 내 아이템 위치 이동시 실행(겹침확인)
    {
        itemObject itemObj = item.GetComponent<itemObject>();
        float left = ((newOrigonPos.x - item.GetComponent<RectTransform>().rect.width / 2f) / Cell) + (XSize / 2f);
        float up = ((newOrigonPos.y + item.GetComponent<RectTransform>().rect.height / 2f) / Cell) - (YSize / 2f);
        up *= -1f;
        int itemIndex = items.items.IndexOf(itemObj.ItemData);
        foreach (var Item in items.items)
        {
            if (items.items.IndexOf(Item) != itemIndex)
            {
                if (isCrash(Item.Left, Item.xSize, left, itemObj.ItemData.xSize, Item.Up, Item.ySize, up, itemObj.ItemData.ySize))
                {
                    item.transform.localPosition = item.GetComponent<itemObject>().OriginPos;
                    return;
                }
            }
        }
        item.transform.localPosition = item.GetComponent<itemObject>().OriginPos = newOrigonPos;
        foreach (var Item in items.items)
        {
            if (items.items.IndexOf(Item) == itemIndex)
            {
                Item.Left = left;
                Item.Up = up;
            }
        }
        jsonSave();
    }
    bool isCrash(float leftA, float xSizeA, float leftB, float xSizeB, float upA, float ySizeA, float upB, float ysizeB)//충돌확인,기존 아이템이 A
    {
        float tempL = leftA < leftB ? leftB : leftA;
        float tempR = leftA + xSizeA > leftB + xSizeB ? leftB + xSizeB : leftA + xSizeA;
        float tempU = upA < upB ? upB : upA;
        float tempD = upA + ySizeA > upB + ysizeB ? upB + ysizeB : upA + ySizeA;
        return tempL < tempR && tempU < tempD;
    }
    void jsonSave()//json저장
    {
        itemJsonData temp = new itemJsonData();
        temp.itemList = new List<itemData>();
        foreach (var Item in items.items)
        {
            temp.itemList.Add(Item);
        }
        setGold();
        int gold = this.Gold;
        temp.gold = gold;
        string tempStr = json.ObjectToJson(temp);
        json.CreateJsonFile(Application.dataPath, "items", tempStr);
    }
    public void getFieldItem(GameObject newItem)//필드에서 신규 아이템 획득
    {
        itemData newData = newItem.GetComponent<fieldItem>().ItemData;
        float newXSize = newData.xSize;
        float newYSize = newData.ySize;
        Vector2 tempPos = emptyCell(newXSize, newYSize);
        if (tempPos != Vector2.zero - Vector2.one)
        {
            itemGet(newXSize, newYSize, tempPos.x, tempPos.y, newData);
            Destroy(newItem);
            jsonSave();
            Destroy(FieldFKey);
            FieldFKey = null;
        }
        //알림창 만들면 else에서 아이템 획득 불가 알릴 것
    }
    public List<Vector2> existCells(List<itemData> itemList)//아이템 리스트 내 아이템 존재 칸 확인
    {
        List<Vector2> result = new List<Vector2>();
        foreach (var item in itemList)
        {
            List<Vector2> existItem = new List<Vector2>();
            for (float j = item.Up; j <= item.Up + item.ySize; j++)
            {
                for (float i = item.Left; i <= item.Left + item.xSize; i++)
                {
                    existItem.Add(new Vector2(i, j));
                }
            }
            foreach (var cells in existItem)
            {
                if (existItem.Contains(new Vector2(cells.x, cells.y))
                    && existItem.Contains(new Vector2(cells.x + 1f, cells.y))
                    && existItem.Contains(new Vector2(cells.x, cells.y + 1f))
                    && existItem.Contains(new Vector2(cells.x + 1f, cells.y + 1f)))
                    result.Add(cells);
            }
        }
        return result;
    }
    public void throwItem(GameObject itemObj, bool isLeft)//인벤토리에서 아이템 버릴 때
    {
        items.items.Remove(itemObj.GetComponent<itemObject>().ItemData);
        itemObjects.Remove(itemObj.gameObject);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject temp = isLeft ? MakeFieldItem(itemObj.GetComponent<itemObject>().ItemData, player.transform.position) : null;
        Destroy(itemObj);
        jsonSave();
        itemSummary.SetActive(false);
    }
    public GameObject MakeFieldItem(itemData data, Vector3 position)//필드에 아이템 생성
    {
        GameObject temp = Instantiate(fieldItemPrefab, position, Quaternion.identity);
        temp.GetComponent<fieldItem>().setup(data);
        return temp;
    }
    public void itemGet(float itemSizeX, float itemSizeY, float itemX, float itemY, itemData itemData)//아이템 획득
    {
        GameObject temp = Instantiate(itemPrefab, inventoryCanvas.transform);
        itemObject tempItem = temp.GetComponent<itemObject>();
        itemData newData = new itemData();
        newData.itemID = itemData.itemID; newData.tag = itemData.tag; newData.itemName = itemData.itemName;
        newData.Left = itemX;newData.Up = itemY;newData.xSize = itemSizeX;newData.ySize = itemSizeY;
        newData.isEquip = false;newData.price = itemData.price;newData.isSell = false;
        tempItem.Setup(itemSizeX, itemSizeY, itemX, itemY, false, zero, newData);
        itemObjects.Add(temp);
        tempItem.ItemData.Left = itemX;
        tempItem.ItemData.Up = itemY;
        items.items.Add(tempItem.ItemData);
    }
    public Vector2 emptyCell(float newXSize, float newYSize)//인벤토리에서 해당 크기의 아이템이 들어올 수 있는 위치
    {
        List<Vector2> existCell = new List<Vector2>();
        existCell = existCells(items.items);
        List<Vector2> emptyCell = new List<Vector2>();
        for (int j = 0; j < YSize; j++) //칸 기준
        {
            for (int i = 0; i < XSize; i++)
            {
                if (existCell.Contains(new Vector2(i, j)))
                    continue;
                emptyCell.Add(new Vector2(i, j));
            }
        }
        foreach (var item in emptyCell)
        {
            bool isOkay = true;
            for (int i = 0; i < newYSize; i++)
            {
                Vector2 tempPoint = item + Vector2.up * i;
                for (int j = 0; j < newXSize; j++)
                {
                    if (!emptyCell.Contains(tempPoint + Vector2.right * j))
                        isOkay = false;
                }
            }
            if (isOkay)
            {
                return new Vector2(item.x, item.y);
            }
        }
        return Vector2.zero - Vector2.one;
    }
    #region 아이템 마우스 조작
    public void itemHover(itemObject itemObj)//아이템에 마우스 올렸을 때
    {
        itemSummary.SetActive(true);
        itemSummary.transform.SetAsLastSibling();
        itemSummary.transform.GetChild(0).GetComponent<Text>().text = itemObj.ItemData.itemName;
    }
    public void itemExit()//마우스가 아이템에서 나갔을 때
    {
        itemSummary.SetActive(false);
    }
    public void itemSummaryMove()//itemSummary 이동
    {
        itemSummary.transform.position = Input.mousePosition + Vector3.right * 150f;
    }
    public void itemLeftDown(itemObject itemObj)//아이템을 좌클릭 했을 때
    {
        #region 설명
        itemDescription.SetActive(true);
        itemDescription.transform.GetChild(0).GetComponent<Text>().text = itemObj.ItemData.itemName;
        bool isFood = false;
        string description = "";
        foreach (var item in itemObj.ItemData.tag)
        {
            if (item == "food")
            {
                isFood = true;
                break;
            }
        }
        if (isFood)
            switch (itemObj.ItemData.itemID)
            {
                case 3:
                    description = "스태미나가 회복될 것 같다";
                    break;
            }
        else
            switch (itemObj.ItemData.itemID)
            {
                default:
                    break;
            }
        if (itemObj.ItemData.isSell)
            description = description + "\n" + Mathf.FloorToInt(itemObj.ItemData.price * 1.5f);
        itemDescription.transform.GetChild(1).GetComponent<Text>().text = description;
        #endregion
    }
    public void itemRightDown(itemObject itemObj)//아이템을 우클릭 했을 때
    {
        if (itemObj.ItemData.isSell)
            return;//판매 물품은 우클릭해도 아이템 기능을 실행하지 않음
        foreach (var item in itemObj.ItemData.tag)
        {
            if (item == "equip")
            {
                itemObj.equipItem();
                break;
            }
            if (item == "food")
            {
                switch (itemObj.ItemData.itemID)
                {//id에 따라 food 효과 조정
                    case 3:
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>().Stamina += 10f;
                        break;
                    default:
                        break;
                }
                throwItem(itemObj.gameObject, false);
            }
        }
    }
    #endregion
    #region get;set;
    public GameObject getObj(string objectKind)
    {
        switch (objectKind)
        {
            case "KeyF":
                return keyF;
            case "Cell":
                return cellObj;
            case "itemPrefab":
                return itemPrefab;
            default:
                return null;
        }
    }
    public void setGold()
    {
        goldObj.transform.GetChild(0).GetComponent<Text>().text = "G " + Gold.ToString();//골드 값 적용
    }
    #endregion
}
