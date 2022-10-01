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
    void Start()
    {
        staminaBackAmount = player.myStartingStamina;
        timeTack = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeTack += Time.deltaTime;
        hp.value = player.health / player.myStartingHealth;
        stamina.value = player.stamina / player.myStartingStamina;
        if (timeTack > 0.5f)
        {
            staminaBackAmount = Mathf.Abs(staminaBackAmount - player.stamina) < 1 ? player.stamina : Mathf.Lerp(player.stamina, staminaBackAmount, 0.5f);
            staminaBack.fillAmount = staminaBackAmount / player.myStartingStamina;
            timeTack = 0f;
        }
    }
}
