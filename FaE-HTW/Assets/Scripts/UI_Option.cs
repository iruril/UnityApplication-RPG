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
    void Start()
    {
        mouseSenseSliderX.value = 0.2f;
        mouseSenseSliderY.value = 0.2f;
    }

    void Update()
    {
        mouseSenseX = mouseSenseSliderX.value * 99 + 1f;
        mouseSenseY = mouseSenseSliderY.value * 99 + 1f;
        mouseSenseTextX.text = Mathf.FloorToInt(mouseSenseX).ToString();
        mouseSenseTextY.text = Mathf.FloorToInt(mouseSenseY).ToString();
    }
    public void senseSetup()
    {
        Camera.mouseSenseX = mouseSenseX * 0.05f;
        Camera.mouseSenseY = mouseSenseY * 0.05f; ;
    }
    public void senseStop()
    {
        Camera.mouseSenseX = 0;
        Camera.mouseSenseY = 0;
    }
}
