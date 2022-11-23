using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    //오브젝트 풀링 사용하기 
    [SerializeField] private GameObject rippleObj;
    private Material mat;

    static float heightOffset = 0.1f;
    private void Start()
    {
        mat = this.gameObject.GetComponent<MeshRenderer>().material;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Test"))
        {
            //오브젝트 풀링 사용하거나 SetActive 사용해서 온오프 하기
            GameObject ripple = Instantiate(rippleObj, transform);
            mat = ripple.GetComponent<MeshRenderer>().material;
            mat.SetVector("_SphereCenter", collision.contacts[0].point);
            Destroy(ripple, 0.5f);
        }
    }
    public void SetActive(bool _bool)
    {
        this.gameObject.SetActive(_bool);
        float height = this.gameObject.transform.parent.GetComponent<CapsuleCollider>().height + heightOffset;
        this.gameObject.transform.localScale = new Vector3(height, height, height);
    }
}
