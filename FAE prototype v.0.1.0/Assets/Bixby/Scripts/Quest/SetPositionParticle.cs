using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionParticle : MonoBehaviour
{
    GameObject positionParticle;
    Vector3 particlePosition;

    // Start is called before the first frame update
    void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "FieldScene")
        {//파티클 객체가 씬에 있으면 if문 제거해도 무관, 현재 씬에 파티클이 없어서 필드에서만 되도록 수정
            positionParticle = GameObject.Find("PositionParticle");
            InitializeVariable();
        }
        else
            positionParticle = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeVariable()
    {
        int sum = this.gameObject.GetComponent<QuestObject>().GetObjectId();
        particlePosition = this.gameObject.GetComponent<QuestObject>().GetPosition();
        positionParticle.transform.position = particlePosition;
        Debug.Log("위치 변환 완료");
    }
}
