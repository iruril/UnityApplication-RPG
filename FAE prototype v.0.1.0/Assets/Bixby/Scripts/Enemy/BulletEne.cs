using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEne : MonoBehaviour
{
    public GameObject ObjRef;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * 0.05f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerContorl>().TakeElementHit(ObjRef.GetComponent<Enemy>().Stat.damage, ObjRef.GetComponent<Enemy>().Stat.element);
            Debug.Log("Hit");
            Destroy(gameObject);
        }
    }
}
