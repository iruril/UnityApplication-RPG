using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MinimapCam : MonoBehaviour
{
    GameObject player;
    GameObject camDirection;
    GameObject minimap;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        minimap = GameObject.Find("Minimap");
        camDirection = minimap.transform.GetChild(0).gameObject;
    }
    void Update()
    {
        Vector3 temp = player.transform.position;
        temp.y += 10f;
        this.transform.position = temp;
        Vector3 camRotate = GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles;
        camRotate.z = -camRotate.y;
        camRotate.y = camRotate.x = 0f;
        camDirection.transform.rotation = Quaternion.Euler(camRotate);
        if (UI_Control.Inst.OpenedWindow == null)
            minimap.transform.parent.gameObject.SetActive(true);
        else
            minimap.transform.parent.gameObject.SetActive(!(UI_Control.Inst.OpenedWindow.name == "Inventory"
                                    || UI_Control.Inst.OpenedWindow.name == "Shop"));
    }
}
