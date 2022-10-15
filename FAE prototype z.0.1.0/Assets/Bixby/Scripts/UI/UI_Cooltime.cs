using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_Cooltime : MonoBehaviour
{
    Image coolTimeImage;
    Text coolTimeCount;
    void Start()
    {
        coolTimeImage = this.transform.GetChild(0).GetComponent<Image>();
        coolTimeCount = this.transform.GetChild(1).GetComponent<Text>();
    }
    [SerializeField] float coolTime;//이후 플레이어에서 변수 가져올 것
    [SerializeField] float maxCoolTime;//이후 플레이어에서 변수 가져올 것
    void Update()
    {
        if (coolTime > 0)
            coolTime -= Time.deltaTime;//실제 쿨타임 줄이는 건 아마 플레이어에서 할테니 변수를 가져온 후에는 제거
        coolTimeImage.fillAmount = coolTime / maxCoolTime;
        coolTimeCount.text = coolTime <= 0 ? "" : Mathf.FloorToInt(coolTime + 1).ToString();
    }
}
