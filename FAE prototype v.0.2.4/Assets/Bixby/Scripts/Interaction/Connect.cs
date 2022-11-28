using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connect : MonoBehaviour
{
    public bool state; //켜져 있는지 아닌지
    public float moveSpeed; //움직이는 속도


    public GameObject[] otherConnect; //다른 연결 오브젝트

    public Material[] m_connect; //연결 오브젝트 머테리얼


    public Vector3 HitPoint;
    public Vector3 HitNormal;

    public NORMAL nor = NORMAL.NONE;

    public enum NORMAL
    {
        NONE = 0,
        X_minus = 1,
        X_plus = 2,
        Y_minus = 3,
        Y_plus = 4,
        Z_minus = 5,
        Z_plus = 6,
    };


    private void Awake()
    {
        state = false;
        this.gameObject.GetComponent<MeshRenderer>().material = m_connect[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveConnect();
        stateCheck();
    }



    void moveConnect()
    {
        //공격을 받으면
        if (HitNormal != new Vector3(0, 0, 0))
        {
            if (HitNormal.x == -1)
            {
                //상태 변경
                nor = NORMAL.X_minus;
                Debug.Log("X-");
                HitNormal = new Vector3(0, 0, 0);
            }
            else if (HitNormal.x == 1)
            {
                //상태 변경
                nor = NORMAL.X_plus;
                Debug.Log("X+");
                HitNormal = new Vector3(0, 0, 0);
            }
            else if (HitNormal.y == -1)
            {
                //상태 변경
                nor = NORMAL.Y_minus;
                Debug.Log("Y-");
                HitNormal = new Vector3(0, 0, 0);
            }
            else if (HitNormal.y == 1)
            {
                //상태 변경
                nor = NORMAL.Y_plus;
                Debug.Log("Y-");
                HitNormal = new Vector3(0, 0, 0);
            }
            else if (HitNormal.z == -1)
            {
                //상태 변경
                nor = NORMAL.Z_minus;
                Debug.Log("Z-");
                HitNormal = new Vector3(0, 0, 0);
            }
            else if (HitNormal.z == 1)
            {
                //상태 변경
                nor = NORMAL.Z_plus;
                Debug.Log("Z-");
                HitNormal = new Vector3(0, 0, 0);
            }
            else
            {
                Debug.Log("오류");
            }
        }


        switch (nor)
        {
            case NORMAL.NONE:
                break;
            case NORMAL.X_minus:
                //상태 초기화
                if (moveTriger == false)
                {
                    StartCoroutine(move(nor));
                }
                nor = NORMAL.NONE;
                break;
            case NORMAL.X_plus:
                //상태 초기화
                if (moveTriger == false)
                {
                    StartCoroutine(move(nor));
                }
                nor = NORMAL.NONE;
                break;
            case NORMAL.Y_minus:
                //상태 초기화
                nor = NORMAL.NONE;
                break;
            case NORMAL.Y_plus:
                //상태 초기화
                nor = NORMAL.NONE;
                break;
            case NORMAL.Z_minus:
                //상태 초기화
                if (moveTriger == false)
                {
                    StartCoroutine(move(nor));
                }
                nor = NORMAL.NONE;
                break;
            case NORMAL.Z_plus:
                //상태 초기화
                if (moveTriger == false)
                {
                    StartCoroutine(move(nor));
                }
                nor = NORMAL.NONE;
                break;
            default:
                break;
        }
    }

    void stateCheck()
    {
        for (int i = 0; i < otherConnect.Length; i++)
        {
            float distance = (otherConnect[i].transform.position - transform.position).magnitude;

            if (distance <= 1.1f)
            {
                this.gameObject.GetComponent<MeshRenderer>().material = m_connect[1];
                state = true;
            }
        }
    }

    //타겟
    public bool moveTriger = false;
    public bool testFlag = true;
    private float blockMoveTime = 1.0f;

    //코루틴
    private IEnumerator move(NORMAL n)
    {
        moveTriger = true;
        float elapsedTime = 0.0f;

        if (state == false)
        {
            if (n == NORMAL.X_minus)
            {
                Vector3 currentPosition = transform.position;
                Vector3 targetPosition = currentPosition + Vector3.right;

                while (elapsedTime < blockMoveTime)
                {
                    transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedTime / blockMoveTime);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = targetPosition;

                moveTriger = false;
            }
            else if (n == NORMAL.X_plus)
            {
                Vector3 currentPosition = transform.position;
                Vector3 targetPosition = currentPosition + Vector3.left;

                while (elapsedTime < blockMoveTime)
                {
                    transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedTime / blockMoveTime);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = targetPosition;

                moveTriger = false;
            }
            else if (n == NORMAL.Y_minus)
            {
                yield return null;
            }
            else if (n == NORMAL.Y_plus)
            {
                yield return null;
            }
            else if (n == NORMAL.Z_minus)
            {
                Vector3 currentPosition = transform.position;
                Vector3 targetPosition = currentPosition + Vector3.forward;

                while (elapsedTime < blockMoveTime)
                {
                    transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedTime / blockMoveTime);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = targetPosition;

                moveTriger = false;
            }
            else if (n == NORMAL.Z_plus)
            {
                Vector3 currentPosition = transform.position;
                Vector3 targetPosition = currentPosition + Vector3.back;

                while (elapsedTime < blockMoveTime)
                {
                    transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedTime / blockMoveTime);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = targetPosition;

                moveTriger = false;
            }
        }
    }
    public void GetP(Vector3 hitPiont, Vector3 hitNormal)
    {
        HitPoint = hitPiont;
        HitNormal = hitNormal;
    }
}
