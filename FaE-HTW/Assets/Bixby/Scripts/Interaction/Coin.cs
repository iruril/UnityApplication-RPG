using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject Player; //플레이어 오브젝트
    public float coinSpeed; //코인 속도

    private void Awake()
    {
        // Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Player.transform.position,transform.position) <= 3.0f)
        {
            transform.LookAt(Player.transform);
            //transform.Translate(coinSpeed * Vector3.forward * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, Player.transform.position, coinSpeed);
        }
    }

    //컴퓨터의 성능 문제로 인식이 안될 시 stay로 변경
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //인벤토리에 골드 변경
            inventoryObject.Inst.Gold++;
            Destroy(gameObject);
        }
    }
}
