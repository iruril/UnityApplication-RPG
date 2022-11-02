using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DamageObject : MonoBehaviour
{
    float timeLast;//지속된 시간
    Vector3 basePos;
    TMPro.TextMeshPro TMP;
    float lastTime;//지속할 시간
    float upSpeed;//올라가는 속도
    float height;//시작 및 기준높이
    [SerializeField] List<Material> fontMaterials;
    void Start()
    {
        timeLast = 0;
        TMP = this.GetComponent<TMPro.TextMeshPro>();
        height = 4f;
        lastTime = 1f;
        upSpeed = 2.5f;
        int temp;
        if (this.transform.root.tag == "Player")//플레이어의 경우
        {
            TMP.fontSize = 6f;
            upSpeed = 1f;
        }
        if (!int.TryParse(TMP.text, out temp))//대미지가 아닐 경우
        {
            TMP.fontMaterial = fontMaterials[0];
            basePos = this.transform.root.position + Vector3.up * 2.5f;
            basePos.y = basePos.y + timeLast * upSpeed;
            this.transform.position = basePos;
            lastTime = 5f;
            this.gameObject.AddComponent<Rigidbody>();
            Vector3 upper = Vector3.up * 100f;
            if (Random.RandomRange(0, 2) == 0)
                upper += GameObject.FindGameObjectWithTag("MainCamera").transform.right * 100f;
            else
                upper += GameObject.FindGameObjectWithTag("MainCamera").transform.right * -100f;
            bool isSynergy = false;
            switch (TMP.text)
            {
                case "열기":  TMP.color = ElementControl.FireSkillStartColor;
                    break;
                case "냉기":  TMP.color = ElementControl.IceSkillStartColor;
                    break;
                case "습기":  TMP.color = ElementControl.WaterSkillStartColor;
                    break;
                case "전기":  TMP.color = ElementControl.ElectroSkillStartColor;
                    break;
                default:
                    isSynergy = true;
                    break;
            }
            if(isSynergy)
            {
                switch (TMP.text)
                {
                    case "융해":  TMP.color = ElementControl.IceSkillStartColor;
                        break;
                    case "빙결":  TMP.color = ElementControl.IceSkillStartColor;
                        break;
                    case "전도":  TMP.color = ElementControl.ElectroSkillStartColor;
                        break;
                    case "폭발":  TMP.color = ElementControl.FireSkillStartColor;
                        break;
                    case "감전":  TMP.color = ElementControl.ElectroSkillStartColor;
                        break;
                    case "증발":  TMP.color = ElementControl.WaterSkillStartColor;
                        break;
                    default:
                        break;
                }
                string tmp = TMP.text;
                TMP.text = "<b>" + tmp + "</b>";
                TMP.fontSize = 10f;
                lastTime = 1f;
                this.gameObject.GetComponent<Rigidbody>().useGravity = false;
                upper = Vector3.up * 100;
            }
            this.gameObject.GetComponent<Rigidbody>().AddForce(upper);

        }
        else
            TMP.color = Color.yellow;
        //switch (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>().MyElement)
    }
    void Update()
    {
        Vector3 camRotate = GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles;
        //camRotate -= Vector3.right * 90f;
        timeLast += Time.deltaTime;
        if (this.gameObject.GetComponent<Rigidbody>() == null)
        {
            basePos = this.transform.root.position + Vector3.up * height;
            basePos.y = basePos.y + timeLast * upSpeed;
            this.transform.position = basePos;
        }
        this.transform.rotation = Quaternion.Euler(camRotate);
        Color tempColor = TMP.color;
        tempColor.a = (lastTime - timeLast) / lastTime;
        TMP.color = tempColor;
        if (TMP.color.a <= 0.1f)
            Destroy(this.gameObject);
    }
}
