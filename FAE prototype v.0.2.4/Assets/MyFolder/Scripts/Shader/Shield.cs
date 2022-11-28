using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : ElementControl, IDamgeable
{
    //오브젝트 풀링 사용하기 
    [SerializeField] private GameObject rippleObj;
    private Material material;
    private Queue<GameObject> ripples = new Queue<GameObject>();

    static float heightOffset = 1.0f;
    private void Awake()
    {
        material = this.gameObject.GetComponent<MeshRenderer>().material;
        for (int i = 0; i < 4; i++)
        {
            GameObject ripple = Instantiate(rippleObj, transform);
            ripple.SetActive(false);

            ripples.Enqueue(ripple);
        }
    }
    private void Start()
    {
        this.Initialize();
    }

    public IEnumerator CreateRipple(Vector3 hitPoint)
    {
        if(ripples.Count == 0)
        {
            GameObject go = Instantiate(rippleObj, transform);
            go.SetActive(false);

            ripples.Enqueue(go);
        }

        GameObject ripple = ripples.Dequeue();
        Material mat = ripple.GetComponent<MeshRenderer>().material;
        mat.SetVector("_SphereCenter", hitPoint);

        ripple.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        ripple.SetActive(false);
        ripples.Enqueue(ripple);
    }

    public void SetActive(bool _bool)
    { 
        this.gameObject.SetActive(_bool);
    }
    public void Initialize()
    {
        Transform parent = this.gameObject.transform.parent;

        //Set Scale
        float height = parent.GetComponent<BoxCollider>().size.y + heightOffset;
        this.gameObject.transform.localScale = new Vector3(height, height, height);

        //Set Position
        Vector3 position = parent.GetComponent<BoxCollider>().center;
        this.gameObject.transform.localPosition = position;

        Color color = Color.black;

        //Set Color
        if (parent.gameObject.tag == "Enemy")
            color = parent.GetComponent<Enemy>().GetMyElementColor();
        else if (parent.gameObject.tag == "DungeonBoss")
            color = parent.GetComponent<DungeonBoss>().GetMyElementColor();
        else if (parent.gameObject.tag == "FinalBoss")
            color = parent.GetComponent<FinalBoss>().GetMyElementColor();
        
        this.material.SetColor("_Color", color);
    }

    public virtual void TakeHit(float damage)
    {
        
    }

    public virtual void TakeElementHit(float damage, ElementRule.ElementType enemyElement) //????? ?????? ????? ????.
    {
        if (transform.parent.tag == "Enemy")
        {
            if (CheckAdventage(enemyElement, transform.GetComponentInParent<Enemy>().Stat.element) < 0)
                transform.GetComponentInParent<Enemy>().target.GetComponent<PlayerContorl>().TakeHit(20.0f);
            else
                transform.GetComponentInParent<Enemy>().TakeHit(10.0f);
        }
        if (transform.parent.tag == "DungeonBoss")
        {
            if (CheckAdventage(enemyElement, transform.GetComponentInParent<DungeonBoss>().Stat.element) < 0)
                transform.GetComponentInParent<DungeonBoss>().target.GetComponent<PlayerContorl>().TakeHit(20.0f);
            else
                transform.GetComponentInParent<DungeonBoss>().TakeHit(10.0f);
        }
        if (transform.parent.tag == "FinalBoss")
        {
            if (CheckAdventage(enemyElement, transform.GetComponentInParent<FinalBoss>().Stat.element) < 0)
                transform.GetComponentInParent<FinalBoss>().Target.GetComponent<PlayerContorl>().TakeHit(20.0f);
            else
                transform.GetComponentInParent<FinalBoss>().TakeHit(10.0f);
        }
    }
}
