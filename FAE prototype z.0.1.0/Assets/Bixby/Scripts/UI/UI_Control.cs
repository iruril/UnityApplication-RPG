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
        windows = new List<GameObject>();
        windows.Add(inventory);
        windows.Add(Map);
        aimPoint = GameObject.Find("AimPoint");
    }
    GameObject optionObj;
    UI_Option option;
    GameObject inventory;
    public Speech Speech;
    public GameObject Map;
    List<GameObject> windows;
    GameObject openedWindow;
    GameObject aimPoint;
    void Start()
    {
        option.Set();
        Map.SetActive(false);
        openedWindow = null;
    }

    void Update()
    {
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
        if (openedWindow == null)
        {
            Cursor.lockState = CursorLockMode.Locked;
            aimPoint.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            aimPoint.SetActive(false);
        }
    }
    public void optionWindow()
    {
        foreach (var item in windows)//¿œ¥‹ √¢ ¥›±‚
        {
            if (item.activeSelf)
            {
                windowSet(item);
                return;
            }
        }
        windowSet(optionObj);
    }
    public void windowSet(GameObject window)//√¢ ø≠∞Ì ¥›±‚
    {
        if (!(openedWindow == null || openedWindow == window))
            return;
        window.SetActive(!window.activeSelf);
        option.senseSet(window.activeSelf);
        if (window.activeSelf)
            openedWindow = window;
        else
            openedWindow = null;
    }
}
