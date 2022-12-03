using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarpPoint : MonoBehaviour
{
    private Vector3 pos;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveRange;
    public bool isActive { get; private set; }
    [SerializeField] private int index;
    private Transform mainObject;

    private GameObject blue;
    private GameObject red;

    [SerializeField] private bool toggle;

    [SerializeField] private GameObject UI;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite redSprite;

    private float deltaTime;
    // Start is called before the first frame update

    void Start()
    {
        mainObject = this.transform.Find("obelisk_main");
        red = this.transform.Find("magic_ring_02").gameObject;
        blue = this.transform.Find("magic_ring_01").gameObject;
        pos = mainObject.position;

        SetActive(WarpObject.instance.isActive[index]);
        deltaTime = 0.0f;
    }
    public void SetActive(bool _bool)
    {
        if (_bool)
        {
            WarpObject.instance.isActive[index] = true;
            isActive = true;
            blue.gameObject.SetActive(true);
            red.SetActive(false);
            UI.transform.GetChild(0).GetComponent<Image>().sprite = blueSprite;
        }
        else
        {
            WarpObject.instance.isActive[index] = false;
            isActive = false;
            blue.SetActive(false);
            red.SetActive(true);
            UI.transform.GetChild(0).GetComponent<Image>().sprite = redSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (toggle)
        {
            SetActive(!isActive);
            toggle = false;
        }
    }

    void FixedUpdate()
    {        
        if (isActive)
        {
            deltaTime += Time.deltaTime;
            Vector3 v = pos;
            v.y += moveRange * Mathf.Sin(deltaTime * moveSpeed);
            mainObject.position = v;
         }
    }

    private IEnumerator Act()
    {
        while (!isActive)
        {
            yield return null;
            
            var wantedPos = Camera.main.WorldToScreenPoint(this.transform.position);            
            inventoryObject.Inst.FieldFKey.transform.position = wantedPos + Vector3.right * 200f;
            
            if (Input.GetKey(KeyCode.F))
            {
                SetActive(true);
                inventoryObject.Inst.FieldFKey.SetActive(false);

                yield break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player") && !isActive)
        {
            inventoryObject.Inst.FieldFKey.SetActive(true);
            StartCoroutine(Act());
        }
    }
    private void OnTriggerExit(Collider other)
    {        
        if (other.CompareTag("Player"))
        {
            inventoryObject.Inst.FieldFKey.SetActive(false);
            StopCoroutine(Act());
        }
    }
}