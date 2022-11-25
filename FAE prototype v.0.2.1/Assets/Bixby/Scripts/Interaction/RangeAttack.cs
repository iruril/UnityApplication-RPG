using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    public float Range = 4.0f;
    public GameObject effectObj;
    public float time = 3;




    // Start is called before the first frame update
    void Start()
    {
        //dds = effectObj.GetComponent<MeshRenderer>().materials[0].GetFloat("Vector1_4b313987053c459c8c6d64938e820ea9");
        
        StartCoroutine(Explosion(1.0f, 0.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Explosion(float target, float current)
    {
        //터지는 범위 이펙트 키기
        effectObj.SetActive(true);
        //float duration = 3.0f;
        float offset = (target - current) / time;
        
        while (current<=target)
        {
            current += offset * Time.deltaTime;
            effectObj.GetComponent<MeshRenderer>().materials[0].SetFloat("Vector1_4b313987053c459c8c6d64938e820ea9", current);

            yield return null;
        }

        current = target;

        //yield return new WaitForSeconds(0.0f);
        //3초 후 터지는 이펙트 키기
        effectObj.SetActive(false);

        //콜라이더 콜로 변경
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position,
                                                        Range, Vector3.up, 0f, LayerMask.GetMask("P"));
        
        //이거할 때 콜라이더로 변경되면 플레이어는 플레이어에 변수 전달. 적도 적에게 변수 전달.
        foreach (RaycastHit hitObj in rayHits)
        {
            //부딛힌거 판단.
            //hitObj.transform.GetComponent<move>().Hit();
            //각자 콜라이더 부딛힌 오브젝트에 있는 스크립트를 읽어와서 각자에서 실행
        }


    }

    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
