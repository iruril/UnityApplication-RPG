using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmission : MonoBehaviour
{
    // Start is called before the first frame update
    public float EnableTime = 2.5f;
    public float Damage = 10.0f;
    public GameObject DestroyParticle;

    private float timer = 0.0f;
    private float coolDown = 0.5f; //데미지 틱 간격


    private WaitForSeconds CoolDownWaitForSeconds;
    private bool isCoolDown = false;
    public Coroutine CoolCoroutine;

    void Start()
    {
        CoolDownWaitForSeconds = new WaitForSeconds(coolDown);
    }

    public IEnumerator tickCalc()
    {
        isCoolDown = true;
        yield return CoolDownWaitForSeconds;
        isCoolDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (isCoolDown)
        {
            this.GetComponent<Collider>().enabled = false;
        }
        else
        {
            this.GetComponent<Collider>().enabled = true;
        }
        if (timer >= EnableTime)
        {
            Instantiate(DestroyParticle, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player") || other.CompareTag("DungeonBoss") || other.CompareTag("FinalBoss"))
        {
            CoolCoroutine = StartCoroutine(tickCalc());
            other.gameObject.GetComponent<IDamgeable>().TakeHit(Damage);
        }
    }
}
