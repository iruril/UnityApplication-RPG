using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
[System.Serializable]
public class itemData
{
    public int itemID;
    public string[] tag;
    public string itemName;
    public float Left;
    public float Up;
    public float xSize;
    public float ySize;
    public bool isEquip;
    public int price;
    public bool isSell;
}
public class itemJsonData
{
    public List<itemData> itemList = new List<itemData>();
    public int gold;

    public itemJsonData()
    {
        itemData nullItem = new itemData();
        itemList.Add(nullItem);
        gold = 0;
    }
}
public class speechData
{
    public List<string> data;
}
public class speechJsonData
{
    //public List<speechData> speechDatas = new List<speechData>();
    public List<string> speechDatas = new List<string>();
    public speechJsonData()
    {
        speechDatas.Add("");
    }
}

public enum QuestKind     // 퀘스트 종류
{
    management,             // 디버깅용 변수(0)
    kill,                   // 처치(1)
    cook,                   // 요리(2)
    hunt,                   // 수렵(3)
    interactive,            // 상호작용(4)
    spot                    // 특정 위치 도달(5)
};

[System.Serializable]
public class questData
{
    //public List<QuestKind> questObject;
    public List<QuestKind> questObject;
    public List<int> objectId;        // 각 목적에 따라 사용할 변수(처치 적, 요리 아이템 및 상호작용 오브젝트 ID)
    public List<int> objectCnt;       // 각 목적에 따라 사용할 변수(카운트)
    public List<float> time;          // 제한 시간
    public string npcName;      // 퀘스트 조건 만족 후 완료를 위해 대화해야 하는 npc 이름(없으면 -1 등과 같은 특정 기호로 표시할 것)
    public List<Vector3> position;    // spot quest의 위치 변수

    public questData()
    {
        //questObject = QuestKind.management;
        //objectId = -1;
        //objectCnt = -1;
        //time = -1f;
        questObject = new List<QuestKind> { QuestKind.management };
        objectId = new List<int>{ -1 };
        objectCnt = new List<int> { -1 };
        time = new List<float> { -1f };
        npcName = "missingNo";
        position = new List<Vector3> { new Vector3(-999f, -999f, -999f) };
    }

    public void Log()
    {
        Debug.Log(questObject);
        //Debug.Log(objectId);
        //Debug.Log(objectCnt);
        //Debug.Log(time);
        //for (int i = 0; i < questObject.Count; i++) Debug.Log(questObject[i]);
        for (int i = 0; i < objectId.Count; i++) Debug.Log(objectId[i]);
        for (int i = 0; i < objectCnt.Count; i++) Debug.Log(objectCnt[i]);
        for (int i = 0; i < time.Count; i++) Debug.Log(time[i]);
        Debug.Log(npcName);
        //Debug.Log(position);
        for (int i = 0; i < position.Count; i++) Debug.Log(position[i]);
    }
}

public class questJsonData
{
    public List<questData> questList = new List<questData>();
    public int questIndex;      // 메인 퀘스트 플롯 순서 변수
    public questJsonData()
    {
        questData nullQuest = new questData();
        questList.Add(nullQuest);
        questIndex = 0;
    }
}

public class json
{
    public static string ObjectToJson(object obj) { return JsonUtility.ToJson(obj); }
    public static T JsonToObject<T>(string jsonData) { return JsonUtility.FromJson<T>(jsonData); }
    public static void CreateJsonFile(string createPath, string fileName, string jsonData) { File.WriteAllText(string.Format("{0}/Resources/json/{1}.json", createPath, fileName), jsonData); }
    public static T LoadJsonFile<T>(string loadPath, string fileName)
    {
        string temp = File.ReadAllText(string.Format("{0}/Resources/json/{1}.json", loadPath, fileName));
        T jsonData = JsonToObject<T>(temp);
        return jsonData;
    }
    public static bool FileExist(string loadPath, string fileName)
    {
        FileInfo info = new FileInfo(string.Format("{0}/Resources/json/{1}.json", loadPath, fileName));
        return info.Exists;
    }
}
