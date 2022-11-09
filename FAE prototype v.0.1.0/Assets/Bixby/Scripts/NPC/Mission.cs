using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    GameObject missionWindow;
    UnityEngine.UI.Text missionTitle;
    UnityEngine.UI.Text missionText;
    void Awake()
    {
        missionWindow = GameObject.Find("Mission");
        missionTitle = missionWindow.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
        missionText = missionWindow.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (missionTitle.text == "")
            return;
        if (UI_Control.Inst.OpenedWindow == null)
            missionWindow.SetActive(true);
        else
            missionWindow.SetActive(!(UI_Control.Inst.OpenedWindow.name == "Inventory"
                                    || UI_Control.Inst.OpenedWindow.name == "Shop"
                                    || UI_Control.Inst.OpenedWindow.name == "Speech"));
    }
    public void misssionSet(string missionTitle, string missionText)
    {
        this.missionTitle.text = missionTitle;
        this.missionText.text = missionText;
    }
    public string GetMissionTitle()
    {
        return missionTitle.text;
    }
}
