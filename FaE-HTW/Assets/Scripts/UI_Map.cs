using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Map : MonoBehaviour
{
    [SerializeField] Vector2 max;
    [SerializeField] Vector2 min;
    float realMaxX, realMaxZ, realMinX, realMinZ;
    float mapX, mapY;
    [SerializeField] GameObject realPlayer;
    [SerializeField] GameObject mapPlayer;
    public float temp;
    void Start()
    {
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
            (mapX * ((realPlayer.transform.position.x - realMinX) / (realMaxX - realMinX)));
        float ytemp=
            (mapY * ((realPlayer.transform.position.z - realMinZ) / (realMaxZ - realMinZ)));
        mapPlayer.transform.position = new Vector3(xtemp, ytemp, 0f);
    }
}
