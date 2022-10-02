using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fieldItem : MonoBehaviour
{
    public itemData itemData;
    float angle;
    [SerializeField] GameObject keyF;
    GameObject keyInst;
    bool playerClose;
    void Start()
    {
        this.transform.localScale = new Vector3(itemData.xSize, itemData.ySize, 1f);
        Vector3 tempPos = this.transform.position;
        tempPos.y = itemData.ySize / 2f;
        this.transform.position = tempPos;
        playerClose = false;
    }
    public void setup(itemData data)
    {
        itemData = data;
        angle = 0f;
        keyInst = null;
        this.transform.localScale = new Vector3(itemData.xSize, itemData.ySize, 1f);
        Vector3 tempPos = this.transform.position;
        tempPos.y = itemData.ySize / 2f;
        this.transform.position = tempPos;
    }
    // Update is called once per frame
    void Update()
    {
        angle += 60 * Time.deltaTime;
        this.transform.localEulerAngles = new Vector3(0f, angle, 15f);
        if (playerClose && Input.GetKeyDown(KeyCode.F))
            inventoryObject.Inst.getFieldItem(this.gameObject);
    }
    private void OnDestroy()
    {
        Destroy(keyInst);
        keyInst = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && keyInst == null)
        {
            keyInst = Instantiate(keyF, GameObject.Find("Canvas").transform);
            var wantedPos = Camera.main.WorldToScreenPoint(this.transform.position);
            keyInst.transform.position = wantedPos + Vector3.right * 200f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag=="Player")
        {
            Destroy(keyInst);
            keyInst = null;
            playerClose = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Player")
        {
            playerClose = true;
        }
    }
}
