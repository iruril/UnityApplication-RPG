using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WhitePotal : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            LoadingSceneController.Instance.LoadScene("IceDungeon");
        }
    }
}
