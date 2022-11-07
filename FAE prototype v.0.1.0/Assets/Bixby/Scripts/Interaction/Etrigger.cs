using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Etrigger : MonoBehaviour
{
    //public int element; //맞춰야할 원소
    //public int nowElement; //현재 원소
    public bool state; //켜져 있는지 아닌지

    public ElementRule.ElementType element; //맞춰야할 원소
    public ElementRule.ElementType nowElement; //현재 원소

    public Material[] m_element; //원소 머테리얼 나중에 쉐이더로 변경될듯

    private void Awake()
    {
        //nowElement = 0;
        state = false;
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
                case ElementRule.ElementType.NONE:
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[0];
                    break;
                case ElementRule.ElementType.FIRE:
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[1];
                    break;
                case ElementRule.ElementType.ICE:
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[2];
                    break;
                case ElementRule.ElementType.WATER:
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[3];
                    break;
                case ElementRule.ElementType.ELECTRICITY:
                    this.gameObject.GetComponent<MeshRenderer>().material = m_element[4];
                    break;
                case ElementRule.ElementType.NUM:
                    break;
                default:
                    break;
            }

            state = true;
        }
    }

    public void GetElement(ElementRule.ElementType elemet)
    {
        nowElement = elemet;
    }
}
