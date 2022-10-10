using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
[System.Serializable]
public class itemData
{
    public int itemID;
    public string itemName;
    public float Left;
    public float Up;
    public float xSize;
    public float ySize;
    public bool isEquip;
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
public class json
{
    public static string ObjectToJson(object obj) { return JsonUtility.ToJson(obj); }
    public static T JsonToObject<T>(string jsonData) { return JsonUtility.FromJson<T>(jsonData); }
    public static void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/Bixby/Resources/json/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }
    public static T LoadJsonFile<T>(string loadPath, string fileName)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/Bixby/Resources/json/{1}.json", loadPath, fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }
    public static bool FileExist(string loadPath, string fileName)
    {
        FileInfo info = new FileInfo(string.Format("{0}/Bixby/Resources/json/{1}.json", loadPath, fileName));
        return info.Exists;
    }
}
