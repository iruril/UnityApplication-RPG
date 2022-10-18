using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CharacterFrame : MonoBehaviour
{
    PlayerContorl playerContorl;
    [SerializeField] int ableElementCount;//사용 가능한 속성 수, 이후 개발함에 따라 현 스크립트에서는 제거 예정
    List<GameObject> avaters;
    void Start()
    {
        //인스펙터 처리함수
        playerContorl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>();
        avaters = new List<GameObject>();
        foreach (Transform item in this.transform)
            avaters.Add(item.gameObject);
        SetAvatars();
    }

    void Update()
    {
        if (Input.anyKeyDown)//아바타 변경 키를 눌렀을 때로 변경가능, 퍼블릭이라 플레이어 컨트롤에서 불러도 됨
            NowAvater();
    }
    public void SetAvatars()//가능한 아바타들 가시화, 불가능한 아바타 비가시화
    {
        float height = avaters[0].GetComponent<RectTransform>().rect.height * 1.5f;
        int i = 0;
        foreach (var item in avaters)
        {
            if (i < ableElementCount)
            {
                item.SetActive(true);
                item.transform.localPosition = Vector3.zero + Vector3.up * (0.5f * ableElementCount - (i + 0.5f)) * height;
            }
            else
                item.SetActive(false);
            i++;
        }
        NowAvater();
    }
    public void NowAvater()//현재 설정중인 아바타 테두리 색 다르게 하는거. 스위치 쓰면 속성마다 색 다르게 할 수 도 있음
    {
        foreach (var item in avaters)
        {
            UnityEngine.UI.Image avatarImage = item.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>();
            UnityEngine.UI.Text avatarText = item.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>();
            if (avaters.IndexOf(item) == ((int)playerContorl.MyElement))
                item.GetComponent<UnityEngine.UI.Image>().color = avatarImage.color = avatarText.color = Color.white;
            else
            {
                avatarImage.color = Color.clear;
                Color tempColor = Color.white; tempColor.a = 0.5f;
                item.GetComponent<UnityEngine.UI.Image>().color = avatarText.color = tempColor;
            }
                
        }
    }
}
