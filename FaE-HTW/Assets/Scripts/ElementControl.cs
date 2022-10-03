using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementControl : ElementRule, IElementReaction
{
    public ElementType MyElement;
    public ElementType EnemyElement;
    public Stack<ElementType> ElementStack = new Stack<ElementType>(); // 속성 합성 시너지 스택. 2스택 쌓이면 바로 전부 팝!

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

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.MyElement = ElementType.FIRE;
            Debug.Log("current element : FIRE");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.MyElement = ElementType.ICE;
            Debug.Log("current element : ICE");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.MyElement = ElementType.WATER;
            Debug.Log("current element : WATER");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            this.MyElement = ElementType.ELECTRICITY;
            Debug.Log("current element : ELECTRICITY");
        }
    }

    protected void attackedOnNormal()
    {
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

    protected void attackedOnSheild()
    {
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
}

