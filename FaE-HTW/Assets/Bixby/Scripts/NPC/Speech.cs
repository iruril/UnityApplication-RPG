using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Speech : MonoBehaviour
{
    [SerializeField] Text talker;//대화상대
    [SerializeField] Text speech;//대화내용
    List<string> speechList;//대화내용 리스트
    int speechIndex;//리스트중 현재 대화내용의 순서
    public GameObject speechWindow;//대화창
    speechJsonData speechJsonData;
    [SerializeField] string Name;//대화자 이름
    void Start()
    {
        speechWindow.SetActive(false);
    }
    public void setUp(string name, int index)//말 걸었을 때
    {
        speechList = new List<string>();//대화 리스트 초기화
        this.Name = name;//대화상대 이름
        this.talker.text = this.Name;//가시화
        if (json.FileExist(Application.dataPath, Name + index.ToString()))//해당 이름의 json파일 존재 확인
            speechJsonData = json.LoadJsonFile<speechJsonData>(Application.dataPath, Name + index.ToString());//로드해옴
        foreach (var item in speechJsonData.speechDatas)//로드한 json데이터의 speechDatas의 내용을
        {
            speechList.Add(item);//대화 리스트에 넣음
        }
        speechIndex = 0;//대화 인덱스로 쓸것임
        UI_Control.Inst.windowSet(speechWindow);
    }
    void Update()
    {
        if (speechWindow.activeSelf)//대화창이 열려있는 동안은
            speech.text = speechList[speechIndex];//대화내용은 리스트 중 해당 인덱스의 내용
    }

    public void speechNext()//대화 넘기기 버튼
    {
        if (speechIndex + 1 == speechList.Count)//마지막일때
        {
            speechIndex = 0;//초기화
            UI_Control.Inst.windowSet(speechWindow);
        }
        else
            speechIndex++;//아니면 넘기기
    }
}
