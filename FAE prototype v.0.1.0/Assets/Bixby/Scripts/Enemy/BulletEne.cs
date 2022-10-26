using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEne : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * 0.05f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Hit");
            Destroy(gameObject);
        }
    }
}
