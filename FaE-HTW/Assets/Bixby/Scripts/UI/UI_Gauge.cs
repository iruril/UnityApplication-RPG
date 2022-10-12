using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gauge : MonoBehaviour
{
    Slider hp;
    Slider stamina;
    PlayerContorl playerControl;
    GameObject staminaBack;
    float staminaBackAmount;
    float timeTack;
    Slider attackCharge;
    CamControl cameraControl;
    void Start()
    {
        hp = this.transform.GetChild(0).GetComponent<Slider>();
        stamina = this.transform.GetChild(1).GetComponent<Slider>();
        attackCharge = this.transform.GetChild(2).GetComponent<Slider>();
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>();
        staminaBack = this.transform.GetChild(1).GetChild(1).gameObject;
        cameraControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamControl>();
        staminaBackAmount = playerControl.MyStartingStamina;
        timeTack = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControl.State == PlayerContorl.STATE.ATTACK && cameraControl.step == CamControl.STATE.AIM)
        {
            attackCharge.gameObject.SetActive(true);
            attackCharge.value = playerControl.StateTimer / playerControl.SwitchToChargeTime;
        }
        else
            attackCharge.gameObject.SetActive(false);
        timeTack += Time.deltaTime;
        hp.value = playerControl.Health / playerControl.MyStartingHealth;
        stamina.value = playerControl.Stamina / playerControl.MyStartingStamina;
        if (timeTack > 0.5f)
        {
            staminaBackAmount = Mathf.Abs(staminaBackAmount - playerControl.Stamina) < 1 ? playerControl.Stamina : Mathf.Lerp(playerControl.Stamina, staminaBackAmount, 0.5f);
            staminaBack.GetComponent<RectTransform>().sizeDelta = new Vector2(staminaBackAmount / playerControl.MyStartingStamina * 200f, 0);
            timeTack = 0f;
        }
    }
}
