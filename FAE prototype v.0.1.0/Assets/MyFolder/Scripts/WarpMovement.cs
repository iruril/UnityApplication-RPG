using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpMovement : MonoBehaviour
{
    private Vector3 pos;
    public float moveSpeed;
    public float moveRange;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = pos;
        v.y += moveRange * Mathf.Sin(Time.time * moveSpeed);
        transform.position = v;
    }
}
