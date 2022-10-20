using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NPC : MonoBehaviour
{
    Speech speech;
    [SerializeField] string npcName;
    bool playerClose;
    GameObject keyInst;
    [SerializeField] int talkIndex;
    GameObject canvasObj;
    GameObject nameObj;
    Text nameUI;
    GameObject notify;
    Shop shop;
    void Start()
    {
        canvasObj = this.transform.GetChild(0).gameObject;
        nameObj = canvasObj.transform.GetChild(0).gameObject;
        speech = UI_Control.Inst.Speech;
        notify = this.transform.GetChild(1).gameObject;
        playerClose = false;
        talkIndex = 0;
        canvasObj.SetActive(true);
        nameUI = nameObj.transform.GetChild(0).GetComponent<Text>();
        nameUI.text = npcName;
        nameObj.SetActive(playerClose);
        shop = UI_Control.Inst.Shop;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camRotate = GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles;
        camRotate -= Vector3.right * 90f;
        notify.transform.rotation = Quaternion.Euler(camRotate);
        if (nameObj.activeSelf)
            nameObj.transform.position = Camera.main.WorldToScreenPoint(this.transform.position + Vector3.up * 2f);
        if (playerClose && Input.GetKeyDown(KeyCode.F))
        {
            if (npcName == "shop")
                shop.SetUp();
            else
                speech.setUp(npcName, talkIndex);
            Destroy(keyInst);
            keyInst = null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && keyInst == null)
        {
            keyInst = Instantiate(inventoryObject.Inst.getObj("KeyF"), GameObject.Find("Canvas").transform);
            var wantedPos = Camera.main.WorldToScreenPoint(this.transform.position);
            keyInst.transform.position = wantedPos + Vector3.right * 200f;
            playerClose = true;
            nameObj.SetActive(playerClose);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(keyInst);
            keyInst = null;
            playerClose = false;
            nameObj.SetActive(playerClose);
        }
    }
}

