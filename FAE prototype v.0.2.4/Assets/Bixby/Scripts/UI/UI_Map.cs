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
    Vector3 mapBasePos;
    List<Transform> warpPoint;
    void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "FieldScene")
        {
            this.gameObject.SetActive(false);
            return;
        }
        realPlayer = GameObject.FindGameObjectWithTag("Player");
        mapPlayer = this.transform.GetChild(0).gameObject;
        realMaxX = max.x;
        realMaxZ = max.y;
        realMinX = min.x;
        realMinZ = min.y;
        mapX = this.GetComponent<RectTransform>().rect.width;
        mapY = this.GetComponent<RectTransform>().rect.height;

        warpPoint = new List<Transform>();
        foreach (Transform item in GameObject.Find("WarpPoints").transform)
        {
            warpPoint.Add(item);
        }
    }
    public void MapSetUp()
    {
        float xtemp =
            (mapX * ((realPlayer.transform.position.x - realMinX) / (realMaxX - realMinX) - 0.5f));
        float ytemp =
            (mapY * ((realPlayer.transform.position.z - realMinZ) / (realMaxZ - realMinZ) - 0.5f));
        mapPlayer.transform.localPosition = new Vector3(xtemp, ytemp, 0f);
        Vector3 mapPos = new Vector3(Screen.width / 2 - mapPlayer.transform.position.x, Screen.height / 2 - mapPlayer.transform.position.y, 0);
        this.gameObject.transform.position += mapPos;
        mapLimitSet(this.gameObject.transform.localPosition);
    }
    void Update()
    {
        
    }
    public void mapDown()
    {
        mapBasePos = this.gameObject.transform.localPosition - Input.mousePosition;
    }
    public void mapDrag()
    {
        mapLimitSet(mapBasePos + Input.mousePosition);
    }
    public void mapScroll()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (this.gameObject.transform.localScale != Vector3.one * 5f)
                this.gameObject.transform.localScale += Vector3.one;
        }
        else
        {
            if (this.gameObject.transform.localScale != Vector3.one)
            {
                this.gameObject.transform.localScale -= Vector3.one;
                mapLimitSet(this.transform.localPosition);
            }
        }
    }

    Vector2 mapLimit(float i)
    {
        float xLimit;
        float yLimit;
        switch (i)
        {
            case 1:
                xLimit = 400;
                yLimit = 820;
                break;
            case 2:
                xLimit = 1758;
                yLimit = 2178;
                break;
            case 3:
                xLimit = 3117;
                yLimit = 3536;
                break;
            case 4:
                xLimit = 4475;
                yLimit = 4895;
                break;
            case 5:
                xLimit = 5834;
                yLimit = 6253;
                break;
            default:
                xLimit = 10000;
                yLimit = 10000;
                break;
        }
        return new Vector2(xLimit, yLimit);
    }
    void mapLimitSet(Vector3 basePos)
    {
        Vector2 limit = mapLimit(this.gameObject.transform.localScale.x);
        float x = Mathf.Clamp(basePos.x, -limit.x, limit.x);
        float y = Mathf.Clamp(basePos.y, -limit.y, limit.y);
        Vector3 newPos = new Vector3(x, y, 0);
        this.gameObject.transform.localPosition = newPos;//400 820
    }

    public void warp(int i)
    {
        realPlayer.GetComponent<PlayerContorl>().enabled = false;
        realPlayer.transform.position = warpPoint[i].position;
        realPlayer.GetComponent<PlayerContorl>().enabled = true;
        UI_Control.Inst.windowSet(this.gameObject);
    }
}
