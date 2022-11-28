using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    public float EnableTime = 0.5f;
    public float Damage = 30.0f;

    private float timer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= EnableTime)
        {
            this.GetComponent<Collider>().enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player") || other.CompareTag("DungeonBoss") || other.CompareTag("FinalBoss"))
        {
            other.gameObject.GetComponent<IDamgeable>().TakeHit(Damage);
        }
    }
}
