using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillControl : ElementControl
{
    public float EnableTime = 0.5f;
    public float Damage = 20.0f;

    private float timer = 0.0f;

    void Start()
    {
        //부모의 Start를 시행하면 안된다.
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= EnableTime)
        {
            this.GetComponent<Collider>().enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            setEnemyElement(other.gameObject.GetComponent<Enemy>().Stat.enemyElement);
            other.gameObject.GetComponent<Enemy>().TakeDamage(attackedOnNormal(Damage));
        }
    }
}
