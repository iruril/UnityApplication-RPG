using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Etrigger : MonoBehaviour
{
    public int element; //맞춰야할 원소
    public int nowElement; //현재 원소
    public bool state; //켜져 있는지 아닌지

    public Material[] m_element; //원소 머테리얼 나중에 쉐이더로 변경될듯

    private void Awake()
    {
        nowElement = 0;
        state = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //투사체 개념이 있어야 할거같다.
        if (other.tag == "projectile")
        {
            switch (other.GetComponent<Projectile>().projectileState)
            {
                case 0:
                    nowElement = 0;
                    break;
                case 1:
                    nowElement = 1;
                    break;
                case 2:
                    nowElement = 2;
                    break;
                case 3:
                    nowElement = 3;
                    break;
                case 4:
                    nowElement = 4;
                    break;
                default:
                    break;
            }
        }
        //TriggerState();
    }

    // Update is called once per frame
    void Update()
    {
        //업데이트에서 빼고 트리거쪽에 넣어야함
        TriggerState();
    }

    public void TriggerState()
    {
        //현재원소와 맞춰야할 원소가 같을 때
        if (element == nowElement)
        {
            switch (element)
            {
                case 0:
                    //null
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[0];
                    break;
                case 1:
                    //불
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[1];
                    break;
                case 2:
                    //물
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[2];
                    break;
                case 3:
                    //얼음
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[3];
                    break;
                case 4:
                    //번개
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[4];
                    break;
                default:
                    break;
            }

            state = true;
        }
    }
}
