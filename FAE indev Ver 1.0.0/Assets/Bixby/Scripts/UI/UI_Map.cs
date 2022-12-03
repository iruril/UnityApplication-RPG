using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Map : MonoBehaviour
{
    private void Awake()
    {
        realPlayer = GameObject.FindGameObjectWithTag("Player");
        mapPlayer = this.transform.GetChild(this.transform.childCount - 1).gameObject;
        realMaxX = max.x;
        realMaxZ = max.y;
        realMinX = min.x;
        realMinZ = min.y;
        mapX = this.GetComponent<RectTransform>().rect.width;
        mapY = this.GetComponent<RectTransform>().rect.height;
        goalPos = this.transform.GetChild(1).gameObject; 
    }
    [SerializeField] Vector2 max;
    [SerializeField] Vector2 min;
    float realMaxX, realMaxZ, realMinX, realMinZ;
    float mapX, mapY;
    GameObject realPlayer;
    GameObject mapPlayer;
    Vector3 mapBasePos;
    List<Transform> warpPoint;
    GameObject goalPos;
    public List<Sprite> GoalPosSprites;//고리, 물음표, 느낌표
    void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "FieldScene")
        {
            this.gameObject.SetActive(false);
            return;
        }

        warpPoint = new List<Transform>();
        foreach (Transform item in GameObject.Find("WarpPoints").transform)
        {
            warpPoint.Add(item);
        }
    }
    public void MapSetUp()
    {
        Vector2 tempPlayer = tempPos(realPlayer.transform.position);
        mapPlayer.transform.localPosition = new Vector3(tempPlayer.x, tempPlayer.y, 0f);

        Vector3 minimapGoal = GameObject.Find("MinimapCamera").GetComponent<UI_MinimapCam>().getGoalPos();
        goalPos.SetActive(minimapGoal != Vector3.one * -999f);
        if (goalPos.activeSelf)
        {
            Vector2 tempGoal = tempPos(minimapGoal);
            goalPos.transform.localPosition = new Vector3(tempGoal.x, tempGoal.y, 0f);
            Rect rect = new Rect(0, 0, UI_Control.Inst.Speech.NPC_Plane_Marks[1].GetTexture("_MainTex").width, UI_Control.Inst.Speech.NPC_Plane_Marks[1].GetTexture("_MainTex").height);
            if (QuestObject.manager.GetIsClear())
            {
                goalPos.GetComponent<UnityEngine.UI.Image>().sprite = GoalPosSprites[1];
                goalPos.GetComponent<UnityEngine.UI.Image>().color = Color.white;
                goalPos.transform.localScale = Vector3.one * 0.6f;
            }
            else if (QuestObject.manager.GetIndex() % 2 == 0)
            {
                goalPos.GetComponent<UnityEngine.UI.Image>().sprite = GoalPosSprites[2];
                goalPos.GetComponent<UnityEngine.UI.Image>().color = Color.white;
                goalPos.transform.localScale = Vector3.one * 0.6f;
            }
            else
            {
                goalPos.GetComponent<UnityEngine.UI.Image>().sprite = GoalPosSprites[0];
                goalPos.GetComponent<UnityEngine.UI.Image>().color = Color.red;
                goalPos.transform.localScale = Vector3.one;
            }
        }

        Vector3 mapPos = new Vector3(Screen.width / 2 - mapPlayer.transform.position.x, Screen.height / 2 - mapPlayer.transform.position.y, 0);
        this.gameObject.transform.position += mapPos;
        mapLimitSet(this.gameObject.transform.localPosition);
    }
    void Update()
    {
        
    }
    #region mapEventTrigger
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
    public void warp(int i)
    {
        if (warpPoint[i].GetComponent<WarpPoint>().isActive == true)
        {
            realPlayer.GetComponent<CharacterController>().enabled = false;
            realPlayer.transform.position = warpPoint[i].position;
            realPlayer.GetComponent<CharacterController>().enabled = true;
            UI_Control.Inst.windowSet(this.gameObject);
        }
    }
    #endregion
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

    Vector2 tempPos(Vector3 pos)
    {
        float xtemp =
            (mapX * ((pos.x - realMinX) / (realMaxX - realMinX) - 0.5f));
        float ytemp =
            (mapY * ((pos.z - realMinZ) / (realMaxZ - realMinZ) - 0.5f));

        return new Vector2(xtemp, ytemp);
    }


}
