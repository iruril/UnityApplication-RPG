using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStatus
{
    public EnemyType enemyType { get; }
    public float maxHp { get; set; }
    public float hp { get; set; }
    public ElementRule.ElementType enemyElement { get; set; }

    public EnemyStatus()
    {

    }

    public EnemyStatus(EnemyType enemyType, float maxHp, ElementRule.ElementType enemyElement)
    {
        this.enemyType = enemyType;
        this.maxHp = maxHp;
        hp = maxHp;
        this.enemyElement = enemyElement;
    }

    public EnemyStatus SetEnemyStatus(EnemyType enemyType, ElementRule.ElementType enemyElement)
    {
        EnemyStatus stat = null;

        if (enemyType == EnemyType.Melee)
            stat = new EnemyStatus(enemyType, 100.0f, enemyElement);
        else if (enemyType == EnemyType.Ranged)
            stat = new EnemyStatus(enemyType, 150.0f, enemyElement);

        return stat;
    }
}
