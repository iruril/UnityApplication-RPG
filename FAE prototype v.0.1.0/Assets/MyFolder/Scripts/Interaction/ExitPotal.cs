using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPotal : MonoBehaviour
{
    private GameObject potalObj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadingSceneController.Instance.LoadScene("FieldScene");
        }
    }
}
