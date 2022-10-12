using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Map : MonoBehaviour
{
    [SerializeField] Vector2 max;
    [SerializeField] Vector2 min;
    float realMaxX, realMaxZ, realMinX, realMinZ;
    float mapX, mapY;
    GameObject realPlayer;
    GameObject mapPlayer;
    void Start()
    {
        realPlayer = GameObject.FindGameObjectWithTag("Player");
        mapPlayer = this.transform.GetChild(0).gameObject;
        realMaxX = max.x;
        realMaxZ = max.y;
        realMinX = min.x;
        realMinZ = min.y;
        mapX = this.GetComponent<RectTransform>().rect.width;
        mapY = this.GetComponent<RectTransform>().rect.height;
    }

    void Update()
    {
        float xtemp =
            (mapX * ((realPlayer.transform.position.x - realMinX) / (realMaxX - realMinX) - 0.5f));
        float ytemp =
            (mapY * ((realPlayer.transform.position.z - realMinZ) / (realMaxZ - realMinZ) - 0.5f));
        mapPlayer.transform.localPosition = new Vector3(xtemp, ytemp, 0f);
    }
}
