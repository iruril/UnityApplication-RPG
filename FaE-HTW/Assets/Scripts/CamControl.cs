using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    public GameObject player;
    public float xmove = 0; 
    public float ymove = 0;  
    public float distanceMoveMod = 8.0f;
    public float distanceAimMod = 3.0f;

    Vector3 revDistanceMove;
    Vector3 revDistanceAim;
    private float transitionSpeed = 30.0f;

    public bool isOnAim = false;

    public STATE step = STATE.NONE; // 현재 상태.
    public STATE next_step = STATE.NONE; // 다음 상태.
    public float step_timer = 0.0f; // 타이머
    public float mouseSenseX;
    public float mouseSenseY;
    public enum STATE
    {
        NONE = -1,
        MOVE = 0,
        AIM = 1,

        NUM = 3, // 상태 종류
    };

    private void Start()
    {
        revDistanceMove = new Vector3(0.0f, -2.0f, distanceMoveMod);
        revDistanceAim = new Vector3(-1.0f, 0.0f, distanceAimMod);

        this.step = STATE.MOVE;
        this.next_step = STATE.MOVE;
        mouseSenseX = 1;
        mouseSenseY = 1;
    }

    void Update()
    {
        xmove += Input.GetAxis("Mouse X") * mouseSenseX;
        ymove -= Input.GetAxis("Mouse Y") * mouseSenseY;

        ymove = Mathf.Clamp(ymove, -55.0f, 55.0f);
        transform.rotation = Quaternion.Euler(ymove, xmove, 0);

        this.step_timer += Time.deltaTime;
        if (this.next_step == STATE.NONE)
        {
            switch (this.step)
            {
                case STATE.MOVE:
                    if (isOnAim) this.next_step = STATE.AIM;
                    break;
                case STATE.AIM:
                    if (!isOnAim) this.next_step = STATE.MOVE;
                    break;
            }
        }

        // 상태가 변화했을 때------------.
        while (this.next_step != STATE.NONE)
        { // 상태가 NONE이외 = 상태가 변화했다.
            this.step = this.next_step;
            this.next_step = STATE.NONE;
            switch (this.step)
            {
                case STATE.MOVE:

                    break;
                case STATE.AIM:

                    break;
            }
            this.step_timer = 0.0f;
        }
        // 각 상황에서 반복할 것----------.
        switch (this.step)
        {
            case STATE.MOVE:
                moveCamMod();
                break;
            case STATE.AIM:
                aimCamMod();
                break;
        }
    }

    void moveCamMod()
    {
        //transform.position = player.transform.position - transform.rotation * revDistanceMove;
        transform.position = Vector3.Lerp(transform.position,
            player.transform.position - transform.rotation * revDistanceMove,
            Time.deltaTime * transitionSpeed);
    }

    void aimCamMod()
    {
        //transform.position = player.transform.position - transform.rotation * revDistanceAim;
        transform.position = Vector3.Lerp(transform.position,
            player.transform.position - transform.rotation * revDistanceAim,
            Time.deltaTime * transitionSpeed);
    }
}
