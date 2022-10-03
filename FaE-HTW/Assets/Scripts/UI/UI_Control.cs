using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Control : MonoBehaviour
{
    public static UI_Control Inst { get; private set; }
    private void Awake() => Inst = this;
    [SerializeField] GameObject optionObj;
    public UI_Option option;
    [SerializeField] GameObject inventory;
    public Speech speech;
    public GameObject map;
    [SerializeField] List<GameObject> windows;
    GameObject openedWindow;
    [SerializeField] GameObject AimPoint;
    void Start()
    {
        option.Set();
        map.SetActive(false);
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
                    windowSet(map);
                    break;
            }
        }
        if (openedWindow == null)
        {
            Cursor.lockState = CursorLockMode.Locked;
            AimPoint.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            AimPoint.SetActive(false);
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
