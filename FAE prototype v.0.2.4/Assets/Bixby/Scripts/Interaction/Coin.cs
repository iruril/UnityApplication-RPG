using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] protected float coinSpeed; //획득하는 코인 속도
    [SerializeField] protected float coinPower; //상자 열었을 때 코인에 주는 힘
    [SerializeField] protected float cognitiveRange;//당겨지는 범위

    [SerializeField] protected int acheiveGold; //획득 골드

    bool act; //활성화 된건지 아닌지

    float angle;

    GameObject Player; //플레이어 오브젝트

    Rigidbody rd;
    public Collider coinCollider;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rd = GetComponent<Rigidbody>();

        act = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //회전
        angle += 90 * Time.deltaTime;
        this.transform.localEulerAngles = new Vector3(0f, angle, 90f);

        //가까워지면 먹어라
        if (Vector3.Distance(Player.transform.position, transform.position) <= cognitiveRange && act)
        {
            //중력제거
            this.rd.useGravity = false;
            this.rd.isKinematic = true;
            //통과하게 만들기
            coinCollider.isTrigger = true;

            transform.LookAt(Player.transform.localPosition);
            //transform.Translate(coinSpeed * Vector3.forward * Time.deltaTime);

            transform.position = Vector3.Slerp(transform.position, Player.transform.position + new Vector3(0, 1, 0), coinSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            act = true;
            coinCollider.isTrigger = false;
        }

        if (other.tag == "Player" && act == true)
        {
            //인벤토리에 골드 변경
            inventoryObject.Inst.Gold += acheiveGold;
            //제이슨 저장
            inventoryObject.Inst.jsonSave();
            Destroy(gameObject);
        }
    }
}
