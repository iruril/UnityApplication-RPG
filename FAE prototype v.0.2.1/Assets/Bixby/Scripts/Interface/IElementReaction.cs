using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IElementReaction
{
    void Fusion();
    void Freezing();
    void ElectricShock();
    void Explosion();
    void Evaporation();
    void Transmission();
    //원소 합성반응연산 함수를 하나씩 기재하고, 그 반응을 구현하면 된다.
}
