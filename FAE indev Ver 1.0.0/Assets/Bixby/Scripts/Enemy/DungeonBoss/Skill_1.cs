using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_1 : ElementControl
{
    public float Time_;

    public ElementType element;

    private CapsuleCollider col;

    // Start is called before the first frame update
    void Start()
    {
        Time_ = 0.0f;
        col = gameObject.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Time_ += Time.deltaTime;

        if (Time_ >= 2.0f && Time_ <= 3.5f)
        {
            col.enabled = true;
            transform.localScale += Vector3.up * 2.0f * Time.deltaTime * 5.0f;
        }
        else if (Time_ > 3.5f)
            Destroy(transform.parent.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            other.GetComponent<PlayerContorl>().TakeElementHit(10.0f, element);
    }
}
