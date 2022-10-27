using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementControl : ElementRule, IElementReaction
{
    public ElementType MyElement;
    public ElementType EnemyElement;
    public Stack<ElementType> ElementStack = new Stack<ElementType>(); // 속성 합성 시너지 스택. 2스택 쌓이면 바로 전부 팝!

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

    // Start is called before the first frame update
    protected virtual void Start()
    {
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

        if (firstElement == ElementType.FIRE && secondElement == ElementType.ICE) //융해
        {
            Fusion();
        }
        else if (firstElement == ElementType.WATER && secondElement == ElementType.ICE) //빙결
        {
            Freezing();
        }
        else if (firstElement == ElementType.ELECTRICITY && secondElement == ElementType.ICE) //전도
        {
            Transmission();
        }
        else if (firstElement == ElementType.ELECTRICITY && secondElement == ElementType.FIRE) //폭발
        {
            Explosion();
        }
        else if (firstElement == ElementType.ELECTRICITY && secondElement == ElementType.WATER) //감전
        {
            ElectricShock();
        }
        else if (firstElement == ElementType.WATER && secondElement == ElementType.FIRE) //증발
        {
            Evaporation();
        }
        else
        {
            return;
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
        //일단, 공격속도를 50퍼센트 늦춘다.
        StartCoroutine(fusion(100.0f));
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
        //instaciate();
    }
    public virtual void Evaporation()
    {
        StartCoroutine(evaporation(10.0f));
    }
    public virtual void Transmission()
    {
        //instanciate();
    }

    IEnumerator fusion(float time)
    {
        float temp = this.gameObject.GetComponent<PlayerStatusControl>().FireRate;

        this.gameObject.GetComponent<PlayerStatusControl>().FireRate =
        this.gameObject.GetComponent<PlayerStatusControl>().FireRate * 1.5f;
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1.0f);
        }
        this.gameObject.GetComponent<PlayerStatusControl>().FireRate = temp;
    }

    IEnumerator freezing(float time)
    {
        this.gameObject.GetComponent<PlayerStatusControl>().SpeedMultiply = 0.5f;
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1.0f);
        }
        this.gameObject.GetComponent<PlayerStatusControl>().SpeedMultiply = 1.0f;
    }

    IEnumerator eletricShock(float time, float damage)
    {
        while(time > 0)
        {
            this.gameObject.GetComponent<PlayerStatusControl>().TakeHit(damage);
            time--;
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator evaporation(float time)
    {
        this.gameObject.GetComponent<PlayerStatusControl>().AdditionalDamage = 1.5f; //50퍼 받피증
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1.0f);
        }
        this.gameObject.GetComponent<PlayerStatusControl>().AdditionalDamage = 1.0f;
    }

    // 임시 추가부분. 적과 연동하기 위한 코드, 작성자: 류창렬
    protected void setEnemyElement(ElementType element)
    {
        EnemyElement = element;
    }
}