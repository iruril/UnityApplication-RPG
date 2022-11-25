using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")        // 플레이어와 충돌 시
        {
            //if(GameObject.Find("GameManager").GetComponent<QuestObject>().GetQuestKind() == QuestKind.spot) // 목적지 도달이 퀘스트 목적일 때
            //{
            //    GameObject.Find("GameManager").GetComponent<QuestObject>().SetIsClear(true);                // 클리어
            //    GameObject.Find("GameManager").GetComponent<SetPositionParticle>().InitializeVariable();    // 위치 변경
            //}
            if (QuestObject.manager.GetQuestKind() == QuestKind.spot) // 목적지 도달이 퀘스트 목적일 때
            {
                QuestObject.manager.SetIsClear(true);                // 클리어
                GameObject.Find("GameManager").GetComponent<SetPositionParticle>().InitializeVariable();    // 위치 변경
            }
        }
    }
}
