using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Option : MonoBehaviour
{
    private void Awake()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Title")
        {
            cameraControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamControl>();
            mouseSenseSliderX = this.transform.GetChild(0).GetComponent<Slider>();
            mouseSenseSliderY = this.transform.GetChild(1).GetComponent<Slider>();
            mouseSenseTextX = this.transform.GetChild(3).GetComponent<Text>();
            mouseSenseTextY = this.transform.GetChild(4).GetComponent<Text>();
            Button closeBtn = this.transform.GetChild(2).GetComponent<Button>();
            closeBtn.onClick.AddListener(() => UI_Control.Inst.windowSet(this.gameObject));
        }
    }
    CamControl cameraControl;
    Slider mouseSenseSliderX;
    Slider mouseSenseSliderY;
    Text mouseSenseTextX;
    Text mouseSenseTextY;
    float mouseSenseX;
    float mouseSenseY;
    public void Set()
    {
        mouseSenseSliderX.value = 0.2f;
        mouseSenseSliderY.value = 0.2f;
        mouseSenseX = mouseSenseSliderX.value * 99 + 1f;
        mouseSenseY = mouseSenseSliderY.value * 99 + 1f;
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Title")
        {
            mouseSenseX = mouseSenseSliderX.value * 99 + 1f;
            mouseSenseY = mouseSenseSliderY.value * 99 + 1f;
            mouseSenseTextX.text = Mathf.FloorToInt(mouseSenseX).ToString();
            mouseSenseTextY.text = Mathf.FloorToInt(mouseSenseY).ToString();
        }
    }
    public void senseSet(bool isStop)
    {
        cameraControl.mouseSenseX = isStop ? 0 : mouseSenseX * 0.05f;
        cameraControl.mouseSenseY = isStop ? 0 : mouseSenseY * 0.05f;
    }

    public void TitleSet(bool start)
    {
        if (start)
            UnityEngine.SceneManagement.SceneManager.LoadScene("FieldScene");
        else
            Application.Quit();
    }
    public void toTitle()
    {
        UI_Control.Inst.windowSet(this.gameObject);
        Cursor.lockState = CursorLockMode.None;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
        QuestObject.manager.SetIndex(0);
        Time.timeScale = 1f;
    }
}
