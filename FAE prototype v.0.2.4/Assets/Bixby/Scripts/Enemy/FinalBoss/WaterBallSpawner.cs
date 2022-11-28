using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBallSpawner : MonoBehaviour
{
    public GameObject WaterBall;
    public Transform Target;
    public LayerMask Mask = -1;

    public float Time_;
    public int count;

    // Start is called before the first frame update
    void Start()
    {
        Time_ = 0.0f;
        count = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Time_ += Time.deltaTime;

        findPlayer();

        transform.position = new Vector3(Target.position.x, transform.position.y, Target.position.z);

        if (Time_ >= 1.0f && count == 0)
        {
            Instantiate(WaterBall, transform.position, transform.rotation);
            count ++;
        }
        else if (Time_ >= 3.0f && count == 1)
        {
            Instantiate(WaterBall, transform.position, transform.rotation);
            count ++;
        }
        else if (Time_ >= 5.0f && count == 2)
        {
            Instantiate(WaterBall, transform.position, transform.rotation);
            count ++;
        }
        else if (Time_ >= 5.5f)
            Destroy(gameObject);
    }

    private void findPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10000.0f, 
                                                    Mask, QueryTriggerInteraction.Ignore);

        if (colliders.Length > 0)
        {    
            Target = colliders[0].transform;
        }
    }
}
