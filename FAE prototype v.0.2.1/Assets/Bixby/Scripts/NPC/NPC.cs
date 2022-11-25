using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NPC : MonoBehaviour
{
    Speech speech;
    string npcName;
    bool playerClose;
    GameObject keyInst;
    string talkIndex;
    GameObject canvasObj;
    GameObject nameObj;
    Text nameUI;
    GameObject notify;
    Shop shop;
    //QuestObject quest;
    void Start()
    {
        npcName = this.gameObject.name;
        canvasObj = this.transform.GetChild(0).gameObject;
        nameObj = canvasObj.transform.GetChild(0).gameObject;
        speech = UI_Control.Inst.Speech;
        notify = this.transform.GetChild(1).gameObject;
        playerClose = false;
        talkIndex = "0";
        canvasObj.SetActive(true);
        nameUI = nameObj.transform.GetChild(0).GetComponent<Text>();
        nameUI.text = npcName;
        nameObj.SetActive(playerClose);
        shop = UI_Control.Inst.Shop;
        keyInst = Instantiate(inventoryObject.Inst.getObj("KeyF"), this.gameObject.transform.GetChild(0));
        keyInst.SetActive(false);
        //quest = GameObject.Find("GameManager").GetComponent<QuestObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // NPC Mark
        Vector3 camRotate = GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles;
        camRotate += Vector3.right * 90f + Vector3.forward * 180f;
        notify.transform.rotation = Quaternion.Euler(camRotate);

        // Inst F
        if(keyInst.activeSelf)
        {
            var wantedPos = Camera.main.WorldToScreenPoint(this.transform.position);
            keyInst.transform.position = wantedPos + Vector3.right * 200f;
        }

        // Map
        if (UI_Control.Inst.OpenedWindow != null)
        {
            if (UI_Control.Inst.OpenedWindow.name == "Map")
                nameObj.SetActive(false);
            else
                nameObj.SetActive(playerClose);
            keyInst.SetActive(false);
        }

        // NameTag
        if (nameObj.activeSelf)
            nameObj.transform.position = Camera.main.WorldToScreenPoint(this.transform.position + Vector3.up * 2f);

        // Quest Mark

        //if (npcName != quest.GetNPCName() || npcName == "shop")
        if (npcName != QuestObject.manager.GetNPCName() || npcName == "shop")
        {
            notify.SetActive(false);
        }
        else
        {
            //if (quest.GetIndex() % 2 == 0)
            if (QuestObject.manager.GetIndex() % 2 == 0)
            {
                if (!notify.activeSelf)
                    notify.SetActive(true);
                //notify.GetComponent<MeshRenderer>().material = quest.NPC_Plane_Marks[0];
                //notify.GetComponent<MeshRenderer>().material = QuestObject.manager.NPC_Plane_Marks[0];
            }
            //else if (quest.GetIsClear())
            else if (QuestObject.manager.GetIsClear())
            {
                if (!notify.activeSelf)
                    notify.SetActive(true);
                //notify.GetComponent<MeshRenderer>().material = quest.NPC_Plane_Marks[1];
                //notify.GetComponent<MeshRenderer>().material = QuestObject.manager.NPC_Plane_Marks[1];
            }
            else
                notify.SetActive(false);
        }

        // F Active
        if (playerClose && Input.GetKeyDown(KeyCode.F))
        {
            if (npcName == "shop")
            {
                shop.SetUp();
                nameObj.SetActive(false);
            }
            else
            {
                //if (npcName == quest.GetNPCName())
                if (npcName == QuestObject.manager.GetNPCName())
                {
                    //talkIndex = quest.GetIsClear() ? quest.GetIndex().ToString()+"o": quest.GetIndex().ToString() + "x";//퀘스트 NPC일때 퀘스트에 따라 인덱스 조정
                    talkIndex = QuestObject.manager.GetIsClear() ? 
                        QuestObject.manager.GetIndex().ToString() + "o" :
                        QuestObject.manager.GetIndex().ToString() + "x";    //퀘스트 NPC일때 퀘스트에 따라 인덱스 조정
                }
                speech.setUp(npcName, npcName + talkIndex);//그 외의 경우, 인덱스 조정 필요
            }
            keyInst.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !keyInst.activeSelf)
        {
            keyInst.SetActive(true);
            playerClose = true;
            nameObj.SetActive(playerClose);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            keyInst.SetActive(false);
            playerClose = false;
            nameObj.SetActive(playerClose);
        }
    }
    public void SetIndex(int value)
    {
        talkIndex = value.ToString();
    }
}

