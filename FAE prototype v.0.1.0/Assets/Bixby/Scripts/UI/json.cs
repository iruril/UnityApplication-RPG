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
    interactive             // 상호작용(4)
};

[System.Serializable]
public class questData
{
    public QuestKind questObject;
    public int objectVar;       // 각 목적에 따라 사용할 변수(처치 적 수, 요리 아이템 및 상호작용 오브젝트 ID 등
    public float time;          // 제한 시간
    public string npcName;      // 퀘스트 조건 만족 후 완료를 위해 대화해야 하는 npc 이름(없으면 -1 등과 같은 특정 기호로 표시할 것)

    public questData()
    {
        questObject = QuestKind.management;
        objectVar = -1;
        time = -1f;
        npcName = "missingNo";
    }

    public void Log()
    {
        Debug.Log(questObject);
        Debug.Log(objectVar);
        Debug.Log(time);
        Debug.Log(npcName);
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
        questIndex = -1;
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
