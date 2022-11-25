using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Speech : MonoBehaviour
{
    private void Awake()
    {
        speechWindow = GameObject.Find("Speech");
        talker = speechWindow.transform.GetChild(1).GetComponent<Text>();
        speech = speechWindow.transform.GetChild(0).GetComponent<Text>();
    }
    Text talker;//대화상대
    Text speech;//대화내용
    List<string> speechList;//대화내용 리스트
    int speechIndex;//리스트중 현재 대화내용의 순서
    GameObject speechWindow;//대화창
    speechJsonData speechJsonData;
    //QuestObject quest;
    void Start()
    {
        speechWindow.SetActive(false);
        Button nextSpeech = speechWindow.transform.GetChild(2).GetComponent<Button>();
        nextSpeech.onClick.AddListener(() => speechNext());
        //quest = GameObject.Find("GameManager").GetComponent<QuestObject>();
    }
    public void setUp(string name, string content)//말 걸었을 때      // name 일치 및 isClear == true일 때 text 변경
    {
        speechList = new List<string>();//대화 리스트 초기화
        this.talker.text = name;//가시화
        if (json.FileExist(Application.dataPath, content))//해당 이름의 json파일 존재 확인
            speechJsonData = json.LoadJsonFile<speechJsonData>(Application.dataPath, content);//로드해옴
        foreach (var item in speechJsonData.speechDatas)//로드한 json데이터의 speechDatas의 내용을
        {
            speechList.Add(item);//대화 리스트에 넣음
        }
        speechIndex = 0;//대화 인덱스로 쓸것임
        UI_Control.Inst.windowSet(speechWindow);
        //if (quest.GetNPCName() == this.talker.text
        //    && (quest.GetIndex() % 2 == 0)
        //    )
        //    quest.SetIsClear(true);

        if (QuestObject.manager.GetNPCName() == this.talker.text
            && (QuestObject.manager.GetIndex() % 2 == 0)
    )
            QuestObject.manager.SetIsClear(true);
    }
    void Update()
    {
        if (speechWindow.activeSelf)//대화창이 열려있는 동안은
            speech.text = speechList[speechIndex];//대화내용은 리스트 중 해당 인덱스의 내용
    }

    public void speechNext()//대화 넘기기 버튼             // name 일치 및 isClear == true일 때 questindex 변경
    {
        if (speechIndex + 1 == speechList.Count)//마지막일때
        {
            //if (quest.GetIsClear()
            //    && this.talker.text == quest.GetNPCName())
            if (QuestObject.manager.GetIsClear()
                && this.talker.text == QuestObject.manager.GetNPCName())
            {
                //GameObject.Find(this.talker.text).GetComponent<NPC>().SetIndex(GameObject.Find(this.talker.text).GetComponent<NPC>().GetIndex() + 1);
                
                //switch (quest.GetQuestKind())
                switch (QuestObject.manager.GetQuestKind())
                {
                    case QuestKind.kill:
                        break;
                    case QuestKind.cook:
                        foreach (var item in inventoryObject.Inst.itemObjects)
                        {
                            //if (item.GetComponent<itemObject>().ItemData.itemID == quest.GetObjectId())
                            if (item.GetComponent<itemObject>().ItemData.itemID == QuestObject.manager.GetObjectId())
                            {
                                inventoryObject.Inst.throwItem(item, false);
                                break;
                            }
                        }
                        break;
                    case QuestKind.hunt:
                        break;
                    case QuestKind.interactive:
                        break;
                    case QuestKind.spot:
                        break;
                }
                //quest.SetNextQuest();
                QuestObject.manager.SetNextQuest();
            }
            speechIndex = 0;//초기화
            UI_Control.Inst.windowSet(speechWindow);
        }
        else
            speechIndex++;//아니면 넘기기
    }
}
