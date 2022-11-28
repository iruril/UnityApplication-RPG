using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UltEffect : MonoBehaviour
{
    GameObject magicCircleA;
    GameObject magicCircleB;
    PlayerContorl playerControl;
    float circleAngleA;
    float circleAngleB;
    void Start()
    {
        this.gameObject.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>();
        magicCircleA = magicCircleB = null;
        foreach (Transform item in this.transform)
        {
            if (magicCircleA == null)
                magicCircleA = item.gameObject;
            else
                magicCircleB = item.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        magicCircleA.GetComponent<Image>().fillAmount = playerControl.State == PlayerContorl.STATE.ELEMENT_ULT_SKILL ? 1f : 0f;
        magicCircleB.GetComponent<Image>().fillAmount = playerControl.State == PlayerContorl.STATE.ELEMENT_ULT_SKILL ? 1f : 0f;
        if (magicCircleA.GetComponent<Image>().fillAmount == 1f)
        {
            circleAngleA += 60 * Time.deltaTime*1.5f;
            magicCircleA.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, circleAngleA));
        }
        if (magicCircleB.GetComponent<Image>().fillAmount == 1f)
        {
            circleAngleB -= 60 * Time.deltaTime*1.5f;
            magicCircleB.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, circleAngleB));
        }
        Color circleColor;
        switch (playerControl.MyElement)
        {
            case ElementRule.ElementType.FIRE:
                circleColor = Color.red;
                circleColor.a = 0.25f;
                break;
            case ElementRule.ElementType.ICE:
                circleColor = Color.cyan;
                circleColor.a = 0.25f;
                break;
            case ElementRule.ElementType.WATER:
                circleColor = Color.blue;
                circleColor.a = 0.25f;
                break;
            case ElementRule.ElementType.ELECTRICITY:
                circleColor = new Color(0.5f, 0f, 1f);
                circleColor.a = 0.25f;
                break;
            default:
                circleColor=Color.white;
                circleColor.a = 0.25f;
                break;
        }
        magicCircleA.GetComponent<Image>().color = magicCircleB.GetComponent<Image>().color = circleColor;
    }
}
