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
    //QuestObject quest;
    Vector3 goalPosi;
    GameObject minimapContour;
    Material goalBaseMaterial;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        minimap = GameObject.Find("Minimap");
        minimapContour = GameObject.Find("minimapContour");
        camDirection = minimap.transform.GetChild(0).gameObject;
        signObject = this.transform.GetChild(0).gameObject;
        goalObject = this.transform.GetChild(1).gameObject;
        goalBaseMaterial = goalObject.GetComponent<MeshRenderer>().material;
    }
    void Update()
    {
        camPositionSet();

        directionSet();

        minimapOnOff();

        minimapSign();
    }
    void camPositionSet()
    {
        Vector3 temp = player.transform.position;
        temp.y += 20f;
        RaycastHit hitinfo;
        temp.y = Physics.Linecast(player.transform.position, player.transform.position + Vector3.up * 20, out hitinfo, 1 << LayerMask.NameToLayer("Ground")) ? hitinfo.point.y : temp.y;
        this.transform.position = temp;
    }
    void directionSet()
    {
        Vector3 camRotate = GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles;
        camRotate.z = -camRotate.y;
        camRotate.y = camRotate.x = 0f;
        camDirection.transform.rotation = Quaternion.Euler(camRotate);
    }
    void minimapOnOff()
    {
        if (UI_Control.Inst.OpenedWindow == null)
            minimap.transform.parent.gameObject.SetActive(true);
        else
            minimap.transform.parent.gameObject.SetActive(!(UI_Control.Inst.OpenedWindow.name == "Inventory"
                                    || UI_Control.Inst.OpenedWindow.name == "Shop"
                                    || UI_Control.Inst.OpenedWindow.transform.parent.gameObject.name == "COOK"));
        minimapContour.SetActive(minimap.transform.parent.gameObject.activeSelf);
    }
    void minimapSign()
    {
        bool isField = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "FieldScene";
        if (QuestObject.manager.GetIndex() % 2 == 0 && isField)
        {
            goalPosi = GameObject.Find(UI_Control.Inst.Speech.NameChanger(QuestObject.manager.GetNPCName())).transform.position;
            //signObject.transform.position = goalObject.transform.position = Vector3.one * -999f;
            //speech.NPC_Plane_Marks[0];
            mapObjectSet();
            return;
        }
        if (QuestObject.manager.GetQuestKind() != QuestKind.spot && !QuestObject.manager.GetIsClear())
        {
            goalPosi = signObject.transform.position = goalObject.transform.position = Vector3.one * -999f;
            return;
        }
        if (isField)
        {
            if (QuestObject.manager.GetIsClear())
                goalPosi = GameObject.Find(UI_Control.Inst.Speech.NameChanger(QuestObject.manager.GetNPCName())).transform.position;
            else
                goalPosi = QuestObject.manager.GetPosition() == Vector3.one * -999f ? GameObject.Find(UI_Control.Inst.Speech.NameChanger(QuestObject.manager.GetNPCName())).transform.position : QuestObject.manager.GetPosition();
        }
        else
            goalPosi = goalPosi = QuestObject.manager.GetPosition();
        mapObjectSet();
        
    }
    public Vector3 getGoalPos()
    {
        return goalPosi;
    }

    void mapObjectSet()
    {
        Vector3 direction = this.transform.position + ((goalPosi - this.transform.position).normalized * this.gameObject.GetComponent<Camera>().orthographicSize * 1.025f);
        float dist = 600 / this.gameObject.GetComponent<Camera>().orthographicSize + 120;
        if (QuestObject.manager.GetIsClear())
            goalObject.GetComponent<MeshRenderer>().material = UI_Control.Inst.Speech.NPC_Plane_Marks[1];
        else if (QuestObject.manager.GetIndex() % 2 == 0)
            goalObject.GetComponent<MeshRenderer>().material = UI_Control.Inst.Speech.NPC_Plane_Marks[0];
        else
            goalObject.GetComponent<MeshRenderer>().material = goalBaseMaterial;
        if (Vector3.Distance(this.gameObject.GetComponent<Camera>().WorldToScreenPoint(goalPosi), this.gameObject.GetComponent<Camera>().WorldToScreenPoint(player.transform.position)) > dist)
        {
            signObject.transform.position = direction;
            signObject.transform.localPosition = new Vector3(signObject.transform.localPosition.x, signObject.transform.localPosition.y, 1f);
            goalObject.transform.position = Vector3.one * -999f;
        }
        else
        {
            goalObject.transform.position = goalPosi;
            goalObject.transform.localPosition = new Vector3(goalObject.transform.localPosition.x, goalObject.transform.localPosition.y, 1f);
            signObject.transform.position = Vector3.one * -999f;
        }
    }
}