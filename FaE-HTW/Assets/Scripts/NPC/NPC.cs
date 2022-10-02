using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NPC : MonoBehaviour
{
    Speech speech;
    [SerializeField] string Name;
    bool playerClose;
    [SerializeField] GameObject keyF;
    GameObject keyInst;
    [SerializeField] int talkIndex;
    [SerializeField] GameObject canvasObj;
    [SerializeField] GameObject nameObj;
    Text nameUI;
    void Start()
    {
        speech = UI_Control.Inst.speech;
        playerClose = false;
        talkIndex = 0;
        canvasObj.SetActive(true);
        nameUI = nameObj.transform.GetChild(0).GetComponent<Text>();
        nameUI.text = Name;
    }

    // Update is called once per frame
    void Update()
    {
        nameObj.SetActive(!UI_Control.Inst.map.activeSelf);
        if(nameObj.activeSelf)
            nameObj.transform.position = Camera.main.WorldToScreenPoint(this.transform.position + Vector3.up * 2f);
        if (playerClose && Input.GetKeyDown(KeyCode.F))
        {
            speech.setUp(Name, talkIndex);
            Destroy(keyInst);
            keyInst = null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && keyInst == null)
        {
            keyInst = Instantiate(keyF, GameObject.Find("Canvas").transform);
            var wantedPos = Camera.main.WorldToScreenPoint(this.transform.position);
            keyInst.transform.position = wantedPos + Vector3.right * 200f;
            playerClose = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(keyInst);
            keyInst = null;
            playerClose = false;
        }
    }
}
