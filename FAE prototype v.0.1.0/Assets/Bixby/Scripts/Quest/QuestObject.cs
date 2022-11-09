using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    // Awake : 변수 Value 적용
    // Start : 데이터 Load / Create
    private void Awake()
    {
        isClear = false;
        objectIndex = 0;
    }

    questJsonData JsonData;         // Quest Json Data
    bool isClear;                   // Quest isClear bool var
    questData currentQuest;
    int objectIndex;                // Quest Object Index var;
    //GameObject tutorialImage;
    // Start is called before the first frame update
    void Start()
    {
        if (json.FileExist(Application.dataPath, "quests"))                                      // Quest 파일이 존재할 시
        {
            JsonData = json.LoadJsonFile<questJsonData>(Application.dataPath, "quests");   // Quest Load
            Debug.Log("Load Complete : " + JsonData.questList[0].npcName);
        }
        else                                                                                    // Quest 파일이 존재하지 않을 때
        {
            //JsonData = new questJsonData();                // new Class
            //JsonData.questList[0].Log();
            //questJsonData.questList = new List<questData>();    // new List
            //questJsonData.questIndex = -1;                      // Quest index -1로 설정

            questJsonData temp = new questJsonData();

            //foreach(var quest in JsonData.questList)
            //{
            //    temp.questList.Add(quest);
            //}


            string questStr = json.ObjectToJson(temp);        // ToString

            Debug.Log(questStr);

            json.CreateJsonFile(Application.dataPath, "quests", questStr);  // Create Json FIle
        }

        currentQuest = JsonData.questList[JsonData.questIndex];
        MissionSet();
        if (JsonData.questIndex == 0)
            UI_Control.Inst.Mission.misssionSet("튜토리얼", "villagerA에게 말을 거시오");
        //tutorialImage = GameObject.Find("TutorialImage");
    }

    // Update is called once per frame
    void Update()
    {
        //if (JsonData.questIndex == 0 && Input.anyKeyDown)
        //{
        //    tutorialImage.SetActive(false);
        //    UI_Control.Inst.Mission.misssionSet("튜토리얼", "villagerA에게 말을 거시오");
        //}
        // 퀘스트 진행 여부 확인
        switch (currentQuest.questObject)
        {
            case QuestKind.management:
                //Debug.Log("Developer's Kind");
                //UI_Control.Inst.Mission.misssionSet("튜토리얼", "임시텍스트. 퀘스트 무사 로드.");
                //SetNextQuest();
                break;
            default:
                CheckQuestCount();
                break;
        }
        // 이후 quest text print
        if (Input.GetKeyDown(KeyCode.Tab))//퀘스트 테스트용 임시 코드
        {
            itemData tempItem = new itemData();
            tempItem.itemID = currentQuest.objectId;
            tempItem.xSize = tempItem.ySize = 1f;
            System.Array.Resize(ref tempItem.tag, 2);
            tempItem.itemName = "퀘스트 음식";
            tempItem.tag[0] = "food";
            tempItem.tag[1] = "cooked";
            Vector2 itemPos = inventoryObject.Inst.emptyCell(1f, 1f);
            inventoryObject.Inst.itemGet(1f, 1f, itemPos.x, itemPos.y, tempItem);
        }

    }

    public int GetObjectId()
    {
        return currentQuest.objectId;
    }

    public float[] GetPosition()
    {
        return currentQuest.position;
    }


    public int GetObjectIndex()                                 // Get ObjectIndex Var Func
    {
        return objectIndex;
    }


    public void SetObjectIndex(int idx)                         // Set ObjectIndex Var Func
    {
        objectIndex = idx;
        MissionSet();
    }

    void CheckQuestCount()                                      // Quest Index Check Func
    {
        if (objectIndex == currentQuest.objectCnt)
        {
            SetIsClear(true);
        }
    }

    //void SetCookCount()                                         // 요리 여부 확인 함수
    //{
    //    if (objectIndex == currentQuest.objectVar)
    //        SetIsClear(true);
    //}

    //void SetHuntCount()                                         // 수렵 카운트 확인 함수
    //{
    //    if (objectIndex == currentQuest.objectVar)
    //        SetIsClear(true);
    //}

    //void SetInteractiveCount()                                  // 상호작용 여부 확인 함수
    //{
    //    if (objectIndex == currentQuest.objectVar)
    //        SetIsClear(true);
    //}

    public void SetNextQuest()                                         // 다음 퀘스트 이동 함수
    {
        JsonData.questIndex += 1;                               // Quest Index++
        isClear = false;
        SetObjectIndex(0);
        if (JsonData.questList.Count <= JsonData.questIndex)
        {
            GameObject.Find(currentQuest.npcName).GetComponent<NPC>().SetIndex(100);
            UI_Control.Inst.Mission.misssionSet("", "");
            return;
        }
        currentQuest = JsonData.questList[JsonData.questIndex]; // Update Current Quest
        MissionSet();
        GameObject.Find(currentQuest.npcName).GetComponent<NPC>().SetIndex(GameObject.Find(currentQuest.npcName).GetComponent<NPC>().GetIndex() + 1);
    }

    public bool GetIsClear()
    {
        return isClear;
    }

    public void SetIsClear(bool idx)
    {
        isClear = idx;

        if (idx)
        {
            if (currentQuest.npcName == "none")//NPC에게 가지않고 퀘스트가 클리어되는 경우
                SetNextQuest();
            else
                UI_Control.Inst.Mission.misssionSet(UI_Control.Inst.Mission.GetMissionTitle(), currentQuest.npcName + "에게 가시오");
        }
    }
    public string GetNPCName()
    {
        return currentQuest.npcName;
    }

    public void MissionSet()
    {
        string missionTitle = "";
        string missionText = "";
        string questPurpose = "";
        switch (currentQuest.questObject)//요리와 상호작용의 경우 여기에서 어떤 요리 혹은 어떤 상호작용인지 설정
        {
            case QuestKind.kill:
                questPurpose = currentQuest.objectCnt.ToString();
                missionText = "적을" + questPurpose + "마리 처치하기" + "(" + objectIndex.ToString() + "/" + currentQuest.objectCnt.ToString() + ")";
                break;
            case QuestKind.hunt:
                questPurpose = currentQuest.objectCnt.ToString();
                missionText = "적을" + questPurpose + "마리 사냥하기" + "(" + objectIndex.ToString() + "/" + currentQuest.objectCnt.ToString() + ")";
                break;
            case QuestKind.cook:
                if (currentQuest.objectId == 2001)
                    questPurpose = "과일주스";
                missionText = questPurpose + "을(를) 만들기";
                break;
            case QuestKind.interactive:
                break;
            default:
                break;
        }
        switch (JsonData.questIndex)
        {
            case 1:
                missionTitle = "Quest " + JsonData.questIndex.ToString();
                break;
            case 2:
            case 4:
                missionTitle = "";
                break;
            case 3:
                missionTitle = "Quest " + (JsonData.questIndex - 1).ToString();
                break;
            default:
                break;
        }

        UI_Control.Inst.Mission.misssionSet(missionTitle, missionText);
    }
    public int GetIndex()
    {
        return JsonData.questIndex;
    }
    public QuestKind GetQuestKind()
    {
        return currentQuest.questObject;
    }
}
