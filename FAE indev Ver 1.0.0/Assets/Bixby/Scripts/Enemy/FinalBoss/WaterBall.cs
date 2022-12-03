using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBall : ElementControl
{
    public GameObject Water;
    public LayerMask mask;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Water.transform.localScale = new Vector3(this.transform.localScale.x, 0.1f, 
                                                this.transform.localScale.z);
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, 
            out hit, 1.0f, mask))
        {
            Instantiate(Water, new Vector3(transform.position.x, 0.7f, transform.position.z),
                        transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerContorl>().TakeElementHit(10.0f, ElementType.WATER);
            Destroy(gameObject);
        }
            //other.gameObject.GetComponent<Player>().Take
    }
}
