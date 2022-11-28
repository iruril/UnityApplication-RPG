using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_Cooltime : MonoBehaviour
{
    Image coolTimeImage;
    Text coolTimeCount;
    Image elementGauge;
    private PlayerContorl playerControl;
    [SerializeField] bool isUlt;
    void Start()
    {
        coolTimeImage = this.transform.GetChild(0).GetComponent<Image>();
        coolTimeCount = this.transform.GetChild(1).GetComponent<Text>();
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>();
        maxCoolTime = isUlt ? playerControl.UltimateSkillCoolDown : playerControl.SkillCoolDown;
        if (isUlt)
            elementGauge = this.transform.GetChild(2).GetComponent<Image>();
    }
    float coolTime;
    float maxCoolTime;
    void Update()
    {
        Color elementColor;
        switch (playerControl.MyElement)
        {
            case ElementRule.ElementType.FIRE:
                elementColor = ElementControl.FireSkillStartColor;
                break;
            case ElementRule.ElementType.ICE:
                elementColor = ElementControl.IceSkillStartColor;
                break;
            case ElementRule.ElementType.WATER:
                elementColor = ElementControl.WaterSkillStartColor;
                break;
            case ElementRule.ElementType.ELECTRICITY:
                elementColor = ElementControl.ElectroSkillStartColor;
                break;
            default:
                elementColor = Color.white;
                break;
        }
        elementColor.a = 0.5f;
        this.GetComponent<Image>().color = elementColor;

        coolTime = isUlt ? playerControl.RemainTimeForUltSkill : playerControl.RemainTimeForSkill;
        coolTimeImage.fillAmount = coolTime / maxCoolTime;
        coolTimeCount.text = coolTime <= 0 ? "" : Mathf.FloorToInt(coolTime + 1).ToString();
        if (isUlt)
        {
            elementGauge.fillAmount = 1 - (playerControl.ElementGauge / 100f);
        }
    }
}
