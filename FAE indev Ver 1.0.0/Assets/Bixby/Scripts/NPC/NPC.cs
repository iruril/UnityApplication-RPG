using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NPC : MonoBehaviour
{
    Speech speech;
    string NpcName;
    bool playerClose;
    GameObject keyInst;
    string talkIndex;
    GameObject canvasObj;
    GameObject nameObj;
    Text nameUI;
    GameObject notify;
    Shop shop;
    SkinnedMeshRenderer mesh;
    PlayerMesh characterMeshes;
    void Start()
    {
        NpcName = this.gameObject.name;
        canvasObj = this.transform.GetChild(0).gameObject;
        nameObj = canvasObj.transform.GetChild(0).gameObject;
        speech = UI_Control.Inst.Speech;
        notify = this.transform.GetChild(1).gameObject;
        playerClose = false;
        talkIndex = "0";
        canvasObj.SetActive(true);
        nameUI = nameObj.transform.GetChild(0).GetComponent<Text>();
        nameUI.text = speech.NameChanger(NpcName);
        this.gameObject.name = nameUI.text;
        nameObj.SetActive(playerClose);
        shop = UI_Control.Inst.Shop;
        keyInst = Instantiate(inventoryObject.Inst.getObj("KeyF"), this.gameObject.transform.GetChild(0));
        keyInst.SetActive(false);
        mesh = this.gameObject.transform.GetChild(this.gameObject.transform.childCount - 1).GetChild(0).GetComponent<SkinnedMeshRenderer>();
        characterMeshes = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).GetChild(0).GetComponent<PlayerMesh>();
        switch (NpcName)
        {
            case "shop":
                notify.SetActive(true);
                notify.GetComponent<MeshRenderer>().material = speech.NPC_Plane_Marks[2];
                break;
            case "partnerA":
                mesh.sharedMesh =
                characterMeshes.CharacterMesh[1];
                mesh.material =
                characterMeshes.CharacterMaterial[1];
                notify.transform.position += Vector3.up * 1f;
                break;
            case "partnerB":
                mesh.sharedMesh =
                characterMeshes.CharacterMesh[2];
                mesh.material =
                characterMeshes.CharacterMaterial[2];
                notify.transform.position += Vector3.up * 1f;
                break;
            case "partnerC":
                mesh.sharedMesh =
                characterMeshes.CharacterMesh[3];
                mesh.material =
                characterMeshes.CharacterMaterial[3];
                notify.transform.position += Vector3.up * 1f;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //onOff
        npcOnOff();

        // Inst F
        if (keyInst.activeSelf)
        {
            var wantedPos = Camera.main.WorldToScreenPoint(this.transform.position + Vector3.up * 1.5f);
            keyInst.transform.position = wantedPos + Vector3.right * 200f;
        }

        // NameTag
        nameTagSet();

        // Quest Mark
        markSet();

        // F Active
        if (playerClose && Input.GetKeyDown(KeyCode.F) && !speech.Tutorial.ElemeneClearText.gameObject.activeSelf)
        {
            npcInteract();
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

    void markSet()
    {
        Vector3 camRotate = GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles;
        camRotate += Vector3.right * 90f + Vector3.forward * 180f;
        notify.transform.rotation = Quaternion.Euler(camRotate);
        notify.transform.localScale = Vector3.one * 0.15f;
        if (NpcName == "shop")
        {
            float dist = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, this.gameObject.transform.position);
            if (dist > 15)
                notify.transform.localScale = Vector3.one * 0.45f;
            else if (dist>10)
                notify.transform.localScale = Vector3.one * 0.3f;
            return;
        }
        if (NpcName != QuestObject.manager.GetNPCName()
            || QuestObject.manager.GetIndex() > 19)
        {
            notify.SetActive(false);
        }
        else
        {
            if (QuestObject.manager.GetIndex() % 2 == 0)
            {
                if (!notify.activeSelf)
                    notify.SetActive(true);
                notify.GetComponent<MeshRenderer>().material = speech.NPC_Plane_Marks[0];
            }
            else if (QuestObject.manager.GetIsClear())
            {
                if (!notify.activeSelf)
                    notify.SetActive(true);
                notify.GetComponent<MeshRenderer>().material = speech.NPC_Plane_Marks[1];
            }
            else
                notify.SetActive(false);
        }
    }
    void nameTagSet()
    {
        if (UI_Control.Inst.OpenedWindow != null)
        {
            if (UI_Control.Inst.OpenedWindow.name == "Map")
                nameObj.SetActive(false);
            else
                nameObj.SetActive(playerClose);
            keyInst.SetActive(false);
        }
        if (nameObj.activeSelf)
            nameObj.transform.position = NpcName.Contains("partner") ? Camera.main.WorldToScreenPoint(this.transform.position + Vector3.up * 3f) : Camera.main.WorldToScreenPoint(this.transform.position + Vector3.up * 2f);
    }
    void npcInteract()
    {
        if (NpcName == "shop")
        {
            shop.SetUp();
            nameObj.SetActive(false);
        }
        else
        {
            talkIndex = QuestObject.manager.GetIsClear() ?
                    QuestObject.manager.GetIndex().ToString() + "o" :
                    QuestObject.manager.GetIndex().ToString() + "x";    //퀘스트에 따라 인덱스 조정
            speech.setUp(NpcName, NpcName + talkIndex);
        }
    }

    void npcOnOff()
    {
        switch (NpcName)
        {
            case "partnerA":
                if (QuestObject.manager.GetIndex() > 9)
                {
                    this.gameObject.SetActive(false);
                    GameObject.Find("Avatars").GetComponent<UI_CharacterFrame>().SetAvatarIndex((QuestObject.manager.GetIndex() - 1) / 4);
                }
                break;
            case "partnerB":
                if (QuestObject.manager.GetIndex() > 13)
                {
                    this.gameObject.SetActive(false);
                    GameObject.Find("Avatars").GetComponent<UI_CharacterFrame>().SetAvatarIndex((QuestObject.manager.GetIndex() - 1) / 4);
                }
                break;
            case "partnerC":
                if (QuestObject.manager.GetIndex() > 17)
                {
                    this.gameObject.SetActive(false);
                    GameObject.Find("Avatars").GetComponent<UI_CharacterFrame>().SetAvatarIndex((QuestObject.manager.GetIndex() - 1) / 4);
                }
                break;
            default:
                break;
        }
    }

    public string NpcGetName()
    {
        return NpcName;
    }
}
