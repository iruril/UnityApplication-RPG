using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStatus : ElementControl
{
    public float FireRate = 0.25f; //공격 딜레이. 공격 간 딜레이 시간을 나타낸다. 단위는 초.
    public float AdditionalDamage = 1.0f; // 퍼센티지 스탯으로 연산되어, 받는 피해 추가계수를 나타낸다.
    public float SpeedMultiply = 1.0f; // 속도 감소 디버프, 혹은 속도 증가 버프를 위한 계수.
}
