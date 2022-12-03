using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEne : ElementControl
{
    public GameObject ObjRef;
    public bool isCharged;
    public ElementType myEle;
    public float myDmg;

    void Start()
    {
        if (isCharged)
            transform.localScale += new Vector3(1.0f, 1.0f, 1.0f);
    }
    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.forward * 0.05f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (isCharged)
                other.gameObject.GetComponent<PlayerContorl>().TakeElementHit(myDmg*2.0f, myEle);
            else
                other.gameObject.GetComponent<PlayerContorl>().TakeElementHit(myDmg, myEle);
            Destroy(gameObject);
        }
    }
}
