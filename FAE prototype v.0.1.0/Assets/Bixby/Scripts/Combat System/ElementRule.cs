using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementRule : MonoBehaviour
{
    public static LinkedList<ElementType> Elements = new LinkedList<ElementType>();
    public enum ElementType
    {
        NONE = -1,
        FIRE = 0,
        ICE = 1,
        WATER = 2,
        ELECTRICITY = 3,

        NUM = 4,
    };

    public int CheckAdventage(ElementType myElementType, ElementType enemyElement) // 1일때 이김, -1일때 짐, 0일때 비김.
    {
        if (myElementType == ElementType.NONE) return 0; //속성이 없을때는 연산해봐야 의미가 없다.

        if(Elements.Find(myElementType).Next == Elements.Find(enemyElement)
            || (Elements.Find(myElementType).Next == null && enemyElement == ElementType.WATER))
        {
            return 1;
        }
        else if(Elements.Find(myElementType).Previous == Elements.Find(enemyElement)
            || (Elements.Find(myElementType).Previous == null && enemyElement == ElementType.ELECTRICITY))
        {
            return -1; // 일반 상태에선 져도 피해가 없는 것으로 끝나나, 쉴드에 질 때는 반사피해를 입는다.
        }
        else
        {
            return 0;
        }
    }
}
