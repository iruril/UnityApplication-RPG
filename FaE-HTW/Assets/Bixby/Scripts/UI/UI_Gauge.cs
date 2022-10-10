using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gauge : MonoBehaviour
{
    [SerializeField] Slider hp;
    [SerializeField] Slider stamina;
    [SerializeField] PlayerContorl player;
    [SerializeField] Image staminaBack;
    float staminaBackAmount;
    public float timeTack;
    [SerializeField] Slider attackCharge;
    void Start()
    {
        staminaBackAmount = player.MyStartingStamina;
        timeTack = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.State == PlayerContorl.STATE.CHARGE_ATTACK)
        {
            attackCharge.gameObject.SetActive(true);
            attackCharge.value = player.StateTimer / 2f;
        }
        else
            attackCharge.gameObject.SetActive(false);
        timeTack += Time.deltaTime;
        hp.value = player.Health / player.MyStartingHealth;
        stamina.value = player.Stamina / player.MyStartingStamina;
        if (timeTack > 0.5f)
        {
            staminaBackAmount = Mathf.Abs(staminaBackAmount - player.Stamina) < 1 ? player.Stamina : Mathf.Lerp(player.Stamina, staminaBackAmount, 0.5f);
            staminaBack.fillAmount = staminaBackAmount / player.MyStartingStamina;
            timeTack = 0f;
        }
    }
}
