using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MinimapCam : MonoBehaviour
{
    GameObject player;
    GameObject camDirection;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camDirection = GameObject.Find("camDirection");
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
    }
}
