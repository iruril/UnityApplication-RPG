using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gauge : MonoBehaviour
{
    Slider hp;
    Slider stamina;
    PlayerContorl playerControl;
    GameObject hpBack;
    GameObject staminaBack;
    float staminaBackAmount;
    float hpBackAmount;
    float timeTack;
    Slider attackCharge;
    CamControl cameraControl;
    Vector3 staminaOriginPos;
    GameObject hpObj;
    GameObject staminaObj;
    void Start()
    {
        hp = this.transform.GetChild(0).GetComponent<Slider>();
        stamina = this.transform.GetChild(1).GetComponent<Slider>();
        hpObj = hp.gameObject;
        staminaObj = stamina.gameObject;
        attackCharge = this.transform.GetChild(2).GetComponent<Slider>();
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>();
        hpBack = hpObj.transform.GetChild(1).gameObject;
        staminaBack = staminaObj.transform.GetChild(1).gameObject;
        cameraControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamControl>();
        hpBackAmount = playerControl.MyStartingHealth;
        staminaBackAmount = playerControl.MyStartingStamina;
        timeTack = 0f;
        staminaOriginPos = staminaObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        

        staminaObj.SetActive(stamina.value != 1);
        staminaObj.transform.position = cameraControl.step == CamControl.STATE.AIM ? staminaOriginPos :
            Vector3.Lerp(staminaObj.transform.position, Camera.main.WorldToScreenPoint(playerControl.gameObject.transform.position + Vector3.up * 1.3f) + Vector3.right * 200f, Time.deltaTime * 15.0f);

        staminaObj.transform.localScale = cameraControl.step == CamControl.STATE.AIM ? Vector3.one : new Vector3(0.7f, 1, 1);
        attackCharge.gameObject.SetActive(playerControl.State == PlayerContorl.STATE.ATTACK && cameraControl.step == CamControl.STATE.AIM);
        if (attackCharge.gameObject.activeSelf)
            attackCharge.value = playerControl.StateTimer / playerControl.SwitchToChargeTime;
        timeTack += Time.deltaTime;
        hp.value = playerControl.Health / playerControl.MyStartingHealth;
        stamina.value = playerControl.Stamina / playerControl.MyStartingStamina;
        if (timeTack > 0.5f)
        {
            staminaBackAmount = Mathf.Abs(staminaBackAmount - playerControl.Stamina) < 1 ? playerControl.Stamina : Mathf.Lerp(playerControl.Stamina, staminaBackAmount, 0.5f);
            staminaBack.GetComponent<RectTransform>().sizeDelta = new Vector2(staminaBackAmount / playerControl.MyStartingStamina * 200f, 0);
            hpBackAmount = Mathf.Abs(hpBackAmount - playerControl.Health) < 1 ? playerControl.Health : Mathf.Lerp(playerControl.Health, hpBackAmount, 0.5f);
            hpBack.GetComponent<RectTransform>().sizeDelta = new Vector2(hpBackAmount / playerControl.MyStartingHealth * 600f, 0);
            timeTack = 0f;
        }
    }
}