using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MinimapCam : MonoBehaviour
{
    GameObject player;
    GameObject camDirection;
    GameObject minimap;
    GameObject signObject;
    GameObject goalObject;
    QuestObject quest;
    Vector3 goalPosi;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        minimap = GameObject.Find("Minimap");
        camDirection = minimap.transform.GetChild(0).gameObject;
        signObject = this.transform.GetChild(0).gameObject;
        goalObject = this.transform.GetChild(1).gameObject;
        quest = GameObject.Find("GameManager").GetComponent<QuestObject>();
    }
    void Update()
    {
        Vector3 temp = player.transform.position;
        temp.y += 20f;
        RaycastHit hitinfo;
        temp.y = Physics.Linecast(player.transform.position, player.transform.position + Vector3.up * 20, out hitinfo, 1 << LayerMask.NameToLayer("Ground")) ? hitinfo.point.y : temp.y;
        this.transform.position = temp;
        Vector3 camRotate = GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles;
        camRotate.z = -camRotate.y;
        camRotate.y = camRotate.x = 0f;
        camDirection.transform.rotation = Quaternion.Euler(camRotate);
        if (UI_Control.Inst.OpenedWindow == null)
            minimap.transform.parent.gameObject.SetActive(true);
        else
            minimap.transform.parent.gameObject.SetActive(!(UI_Control.Inst.OpenedWindow.name == "Inventory"
                                    || UI_Control.Inst.OpenedWindow.name == "Shop"
                                    || UI_Control.Inst.OpenedWindow.transform.parent.gameObject.name == "COOK"));

        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    print("목적지" + this.gameObject.GetComponent<Camera>().WorldToScreenPoint(goalPosi));
        //    print("파티클" + this.gameObject.GetComponent<Camera>().WorldToScreenPoint(goalParti.transform.position));
        //    print("거리" + Vector3.Distance(this.gameObject.GetComponent<Camera>().WorldToScreenPoint(goalPosi), this.gameObject.GetComponent<Camera>().WorldToScreenPoint(player.transform.position)).ToString());
        //}
        //foreach (var item in GeometryUtility.CalculateFrustumPlanes(this.gameObject.GetComponent<Camera>()))
        //{
        //    if(item.GetDistanceToPoint(goalPosi))
        //}
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "FieldScene")
        {
            goalPosi = quest.GetPosition() == new Vector3(-999f, -999f, -999f) ? GameObject.Find(quest.GetNPCName()).transform.position : quest.GetPosition();
            Vector3 direction = this.transform.position + ((goalPosi - this.transform.position).normalized * this.gameObject.GetComponent<Camera>().orthographicSize * 1.025f);
            float dist = 600 / this.gameObject.GetComponent<Camera>().orthographicSize + 120;
            if (Vector3.Distance(this.gameObject.GetComponent<Camera>().WorldToScreenPoint(goalPosi), this.gameObject.GetComponent<Camera>().WorldToScreenPoint(player.transform.position)) > dist)
            {
                signObject.transform.position = direction;
                signObject.transform.localPosition = new Vector3(signObject.transform.localPosition.x, signObject.transform.localPosition.y, 1f);
                goalObject.transform.position = new Vector3(-999f, -999f, -999f);
            }
            else
            {
                goalObject.transform.position = goalPosi;
                goalObject.transform.localPosition = new Vector3(goalObject.transform.localPosition.x, goalObject.transform.localPosition.y, 1f);
                signObject.transform.position = new Vector3(-999f, -999f, -999f);
            }
        }
        else
        {
            signObject.transform.position = goalObject.transform.position = new Vector3(-999f, -999f, -999f);
        }
    }
}
