using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementControl : ElementRule, IElementReaction
{
    public ElementType MyElement;
    public ElementType EnemyElement;
    public Stack<ElementType> ElementStack; // 속성 합성 시너지 스택. 2스택 쌓이면 바로 전부 팝!

    public GameObject ExplosionObj;
    public GameObject TransmissionObj;

    protected Color mySkillStartColor = Color.yellow;
    protected Color mySkillEndColor = Color.white;

    public static Color FireSkillStartColor = new Color(201 / 255f, 29 / 255f, 6 / 255f);
    public static Color FireSkillEndColor = new Color(255 / 255f, 185 / 255f, 33 / 255f);

    public static Color IceSkillStartColor = new Color(33 / 255f, 255 / 255f, 255 / 255f);
    public static Color IceSkillEndColor = new Color(237 / 255f, 253 / 255f, 255 / 255f);

    public static Color WaterSkillStartColor = new Color(16 / 255f, 46 / 255f, 222 / 255f);
    public static Color WaterSkillEndColor = new Color(33 / 255f, 255 / 255f, 255 / 255f);

    public static Color ElectroSkillStartColor = new Color(112 / 255f, 69 / 255f, 255 / 255f);
    public static Color ElectroSkillEndColor = new Color(112 / 255f, 69 / 255f, 255 / 255f);

    public bool IsFusion = false;
    public bool IsEvaporation = false;
    public bool IsElectronicShock = false;
    public bool IsExplosion = false;
    public bool IsFreezing = false;
    public bool IsTransmission = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        ElementStack = new Stack<ElementType>();
        if (Elements.Count < 4)
        {
            Elements.AddLast(ElementType.WATER);
            Elements.AddLast(ElementType.FIRE);
            Elements.AddLast(ElementType.ICE);
            Elements.AddLast(ElementType.ELECTRICITY);
        }

        MyElement = ElementType.NONE;
        EnemyElement = ElementType.NONE;
    }


    protected void checkIsPopTime()
    {
        if (ElementStack.Count >= 2)
        {
            elementReaction();
        }
        else
            return;
    }

    protected void elementReaction()
    {
        ElementType firstElement = ElementStack.Pop();
        ElementType secondElement = ElementStack.Pop();

        if ((firstElement == ElementType.FIRE && secondElement == ElementType.ICE)
            || (secondElement == ElementType.FIRE && firstElement == ElementType.ICE)) //융해
        {
            if (!IsFusion)
            {
                Debug.Log("융해!");
                Fusion();
                IsFusion = true;
                UI_Control.Inst.damageSet("융해", this.gameObject);
            }
        }
        if ((firstElement == ElementType.WATER && secondElement == ElementType.ICE)
            || (secondElement == ElementType.WATER && firstElement == ElementType.ICE)) //빙결
        {
            if (!IsFreezing)
            {
                Debug.Log("빙결!");
                Freezing();
                IsFreezing = true;
                UI_Control.Inst.damageSet("빙결", this.gameObject);
            }
        }
        if ((firstElement == ElementType.ELECTRICITY && secondElement == ElementType.ICE)
            || (secondElement == ElementType.ELECTRICITY && firstElement == ElementType.ICE)) //전도
        {
            if (!IsTransmission)
            {
                Debug.Log("전도!");
                Transmission();
                //IsTransmission = true;
                UI_Control.Inst.damageSet("전도", this.gameObject);
            }
        }
        if ((firstElement == ElementType.ELECTRICITY && secondElement == ElementType.FIRE)
            || (secondElement == ElementType.ELECTRICITY && firstElement == ElementType.FIRE)) //폭발
        {
            if (!IsExplosion)
            {
                Debug.Log("폭발!");
                Explosion();
                //IsExplosion = true;
                UI_Control.Inst.damageSet("폭발", this.gameObject);
            }
        }
        if ((firstElement == ElementType.ELECTRICITY && secondElement == ElementType.WATER)
            || (secondElement == ElementType.ELECTRICITY && firstElement == ElementType.WATER)) //감전
        {
            if (!IsElectronicShock)
            {
                Debug.Log("감전!");
                ElectricShock();
                IsElectronicShock = true;
                UI_Control.Inst.damageSet("감전", this.gameObject);
            }
        }
        if ((firstElement == ElementType.WATER && secondElement == ElementType.FIRE)
            || (secondElement == ElementType.WATER && firstElement == ElementType.FIRE)) //증발
        {
            if (!IsEvaporation)
            {
                Debug.Log("증발!");
                Evaporation();
                IsEvaporation = true;
                UI_Control.Inst.damageSet("증발", this.gameObject);
            }
        }
    }

    protected float attackedOnNormal(float damage)
    {
        int adventage = CheckAdventage(MyElement, EnemyElement);
        switch (adventage)
        {
            case 1:
                return damage * 2;
            case 0:
                return damage * 1;
            case -1:
                return damage / 2;
        }
        return 0;
    }

    protected void attackedOnSheild() // 반동을 입을 가능성이 있기 때문에, 반환형은 일단 void로 한다. 
    { // 자기 자신에게 대미지를 줘야 할 상황(자기 자신에 대한 접근)이 생길 가능성이 있다.
        int adventage = CheckAdventage(MyElement, EnemyElement);
        switch (adventage)
        {
            case 1:

                break;
            case 0:

                break;
            case -1:

                break;
        }
    }

    public virtual void Fusion()
    {
        //50퍼 받피증.
        StartCoroutine(fusion(10.0f));
    }
    public virtual void Freezing()
    {
        StartCoroutine(freezing(10.0f));
        //이동속도를 50퍼센트 늦춘다.
    }
    public virtual void ElectricShock()
    {
        StartCoroutine(eletricShock(5.0f, 10.0f));
    }
    public virtual void Explosion()
    {
        Instantiate(ExplosionObj, this.transform.position, this.transform.rotation);
        IsExplosion = false;
    }
    public virtual void Evaporation()
    {
        //공격딜레이가 2배로 증가.
        StartCoroutine(evaporation(10.0f));
    }
    public virtual void Transmission()
    {
        Instantiate(TransmissionObj, this.transform.position, this.transform.rotation);
        IsTransmission = false;
    }

    IEnumerator fusion(float time)
    {
        this.gameObject.GetComponent<CombatStatus>().AdditionalDamage = 1.5f; //50퍼 받피증
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1.0f);
        }
        IsEvaporation = false;
        this.gameObject.GetComponent<CombatStatus>().AdditionalDamage = 1.0f;

        float temp = this.gameObject.GetComponent<CombatStatus>().FireRate;

        this.gameObject.GetComponent<CombatStatus>().FireRate =
        this.gameObject.GetComponent<CombatStatus>().FireRate * 2.0f;
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1.0f);
        }
        IsFusion = false;
        this.gameObject.GetComponent<CombatStatus>().FireRate = temp;
    }

    IEnumerator freezing(float time)
    {
        this.gameObject.GetComponent<CombatStatus>().SpeedMultiply = 0.5f;
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1.0f);
        }
        IsFreezing = false;
        this.gameObject.GetComponent<CombatStatus>().SpeedMultiply = 1.0f;
    }

    IEnumerator eletricShock(float time, float damage)
    {
        while(time > 0)
        {
            if (this.gameObject.tag == "Player")
            {
                this.gameObject.GetComponent<PlayerStatusControl>().TakeHit(damage);
            }
            else if(this.gameObject.tag == "Enemy")
            {
                this.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
            time--;
            yield return new WaitForSeconds(1.0f);
        }
        IsElectronicShock = false;
    }

    IEnumerator evaporation(float time)
    {
        float temp = this.gameObject.GetComponent<CombatStatus>().FireRate;

        this.gameObject.GetComponent<CombatStatus>().FireRate =
        this.gameObject.GetComponent<CombatStatus>().FireRate * 2.0f;
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1.0f);
        }
        IsFusion = false;
        this.gameObject.GetComponent<CombatStatus>().FireRate = temp;
    }

    // 임시 추가부분. 적과 연동하기 위한 코드, 작성자: 류창렬
    protected void setEnemyElement(ElementType element)
    {
        EnemyElement = element;
    }
}