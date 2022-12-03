using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillControl : ElementControl
{
    public float EnableTime = 0.5f;
    public float Damage = 20.0f;

    private float timer = 0.0f;

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
        if (other.CompareTag("Enemy"))
        {
            setEnemyElement(other.gameObject.GetComponent<Enemy>().Stat.element);
            other.gameObject.GetComponent<Enemy>().TakeElementHit(Damage, MyElement);
        }
        if (other.CompareTag("FinalBoss")) {
            setEnemyElement(other.GetComponent<FinalBoss>().Stat.element);
            other.GetComponent<FinalBoss>().TakeElementHit(Damage, MyElement);
        }
        if (other.CompareTag("DungeonBoss"))
        {
            setEnemyElement(other.GetComponent<DungeonBoss>().Stat.element);
            other.GetComponent<DungeonBoss>().TakeElementHit(Damage, MyElement);
        }
    }
}
