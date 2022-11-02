using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Control : MonoBehaviour
{
    public static UI_Control Inst { get; private set; }
    private void Awake()
    {
        Inst = this;
        optionObj = GameObject.Find("Option");
        option = optionObj.GetComponent<UI_Option>();
        inventory = GameObject.Find("Inventory");
        Speech = this.gameObject.GetComponent<Speech>();
        Map = GameObject.Find("Map");
        aimPoint = GameObject.Find("AimPoint");
        Shop = this.gameObject.GetComponent<Shop>();
        Mission = this.gameObject.GetComponent<Mission>();
        player = GameObject.FindGameObjectWithTag("Player");
        EnemyHp = this.gameObject.GetComponent<UI_EnemyHp>();
    }
    GameObject optionObj;
    UI_Option option;
    GameObject inventory;
    public Speech Speech;
    public GameObject Map;
    List<GameObject> windows;
    public GameObject OpenedWindow;
    GameObject aimPoint;
    public Shop Shop;
    public Mission Mission;
    [SerializeField] GameObject damagePrefab;
    float elementalLast;
    GameObject player;
    public UI_EnemyHp EnemyHp;
    void Start()
    {
        windows = new List<GameObject>();
        windows.Add(inventory);
        windows.Add(Map);
        windows.Add(Shop.getWindow());
        option.Set();
        Map.SetActive(false);
        OpenedWindow = null;
        Cursor.lockState = CursorLockMode.Locked;
        elementalLast = 0f;
    }

    void Update()
    {
        elementalLast += Time.deltaTime;
        if (player.GetComponent<PlayerContorl>().ElementStack.Count != 0 && elementalLast > 0.5f)
        {
            switch (player.GetComponent<PlayerContorl>().ElementStack.Peek())
            {
                case ElementRule.ElementType.FIRE:
                    damageSet("열기", player);
                    break;
                case ElementRule.ElementType.ICE:
                    damageSet("냉기", player);
                    break;
                case ElementRule.ElementType.WATER:
                    damageSet("습기", player);
                    break;
                case ElementRule.ElementType.ELECTRICITY:
                    damageSet("전기", player);
                    break;
            }
            elementalLast = 0f;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            optionWindow();
        if (Input.anyKeyDown)
        {
            string temp = Input.inputString;

            switch (temp)
            {
                case "i":
                    windowSet(inventory);
                    break;
                case "m":
                    windowSet(Map);
                    break;
            }
        }
    }
    void optionWindow()
    {
        if (!windowClose())
            windowSet(optionObj);
    }
    public void windowSet(GameObject window)//창 열고 닫기
    {
        if (!(OpenedWindow == null || OpenedWindow == window))
            return;
        window.SetActive(!window.activeSelf);
        option.senseSet(window.activeSelf);
        if (window.activeSelf)
        {
            OpenedWindow = window;
            switch (window.name)
            {
                case "Shop":
                    inventory.SetActive(true);
                    break;
            }
            Cursor.lockState = CursorLockMode.None;
            aimPoint.SetActive(false);
            Time.timeScale = 0f;
        }
        else
        {
            OpenedWindow = null;
            switch (window.name)
            {
                case "Shop":
                    inventory.SetActive(false);
                    inventory.transform.GetChild(2).gameObject.SetActive(false);
                    break;
                case "Inventory":
                    inventory.transform.GetChild(2).gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
            Cursor.lockState = CursorLockMode.Locked;
            aimPoint.SetActive(true);
            Time.timeScale = 1f;
        }
    }
    public bool windowClose()//그냥 창 닫기
    {
        bool result = false;
        foreach (var item in windows)
        {
            if (item.activeSelf)
            {
                windowSet(item);
                result = true;
            }
        }
        return result;
    }

    public void damageSet(string content, GameObject subject)
    {
        GameObject temp = Instantiate(damagePrefab, subject.transform);
        temp.GetComponent<TMPro.TextMeshPro>().text = content;
    }
}
