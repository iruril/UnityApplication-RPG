using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DamageObject : MonoBehaviour
{
    float timeLast;
    Vector3 basePos;
    TMPro.TextMeshPro TMP;
    void Start()
    {
        timeLast = 0;
        TMP = this.GetComponent<TMPro.TextMeshPro>();
        this.transform.position = this.transform.root.position + Vector3.up * 4f;
        //switch (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContorl>().MyElement)
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camRotate = GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles;
        //camRotate -= Vector3.right * 90f;
        basePos = this.transform.root.position + Vector3.up * 4f;
        timeLast += Time.deltaTime;
        basePos.y = basePos.y + timeLast * 2.5f;
        this.transform.position = basePos;
        this.transform.rotation = Quaternion.Euler(camRotate);
        Color tempColor = TMP.color;
        tempColor.a = (1f - timeLast) / 1f;
        TMP.color = tempColor;
        if (TMP.color.a <= 0.1f)
            Destroy(this.gameObject);
    }
}
