using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private CharacterController characterController;
    private void Start()
    {
        characterController = this.GetComponent<CharacterController>();

        if (startPosition != new Vector3(0.0f, 0.0f, 0.0f))
        {
            characterController.enabled = false;

            this.transform.position = startPosition;
            transform.SetPositionAndRotation(startPosition, startRotation);

            characterController.enabled = true;
        }
    }
    public void SetPosition(string previousSceneName)
    {
        switch (previousSceneName)
        {
            case "FireDungeon" :
                startPosition =  new Vector3(-272.0f, 14.0f, -292.0f);
                break;

            case "IceDungeon" :
                startPosition = new Vector3(-317.0f, 14.0f, 335.0f);
                break;

            case "WaterDungeon" :
                startPosition = new Vector3(192.0f, 13.0f, 205.0f);
                break;

            case "ElectricDungeon" :
                startPosition = new Vector3(284.0f, 14.0f, -271.0f);
                break;

            case "BossDungeon":
                startPosition = new Vector3(85.0f, 10.0f, -158.0f);
                break;

            default :
                startPosition = new Vector3(0.0f, 0.0f, 0.0f);
                break;
        }

        startRotation = Quaternion.LookRotation(-startPosition);
    }
}
