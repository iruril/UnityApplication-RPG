using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDungeon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")       // 플레이어와 충돌 시
        {
            if(SceneManager.GetActiveScene().name == "WaterDungeon") SceneManager.LoadScene("FieldScene");
            else SceneManager.LoadScene("WaterDungeon");
        }
    }
}
