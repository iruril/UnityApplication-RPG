using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Option : MonoBehaviour
{
    [SerializeField] CamControl Camera;
    [SerializeField] Slider mouseSenseSliderX;
    [SerializeField] Slider mouseSenseSliderY;
    [SerializeField] Text mouseSenseTextX;
    [SerializeField] Text mouseSenseTextY;
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
        mouseSenseX = mouseSenseSliderX.value * 99 + 1f;
        mouseSenseY = mouseSenseSliderY.value * 99 + 1f;
        mouseSenseTextX.text = Mathf.FloorToInt(mouseSenseX).ToString();
        mouseSenseTextY.text = Mathf.FloorToInt(mouseSenseY).ToString();
    }
    public void senseSet(bool isStop)
    {
        Camera.mouseSenseX = isStop ? 0 : mouseSenseX * 0.05f;
        Camera.mouseSenseY = isStop ? 0 : mouseSenseY * 0.05f;
    }
}
