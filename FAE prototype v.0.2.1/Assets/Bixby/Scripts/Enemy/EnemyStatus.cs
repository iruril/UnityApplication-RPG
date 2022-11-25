using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStatus
{
    public EnemyType type { get; }
    public float maxHp { get; set; }
    public float hp { get; set; }
    public float damage { get; set; }
    public float attackRange { get; set; }
    public float attackSpeed { get; set; }
    public float moveSpeed { get; set; }
    public float sight { get; set; }
    public ElementRule.ElementType element { get; set; }

    public EnemyStatus()
    {

    }

    public EnemyStatus(EnemyType type, float maxHp, float damage, float attackRange, float attackSpeed, float moveSpeed, float sight, ElementRule.ElementType element)
    {
        this.type = type;
        this.maxHp = maxHp;
        hp = maxHp;
        this.damage = damage;
        this.attackRange = attackRange;
        this.attackSpeed = attackSpeed;
        this.moveSpeed = moveSpeed;
        this.sight = sight;
        this.element = element;
    }

    public EnemyStatus SetEnemyStatus(EnemyType enemyType, ElementRule.ElementType enemyElement)
    {
        EnemyStatus stat = null;

        if (enemyType == EnemyType.Melee)
            stat = new EnemyStatus(enemyType, 100.0f, 10.0f, 2.0f, 5.0f, 5.0f, 15.0f, enemyElement);
        else if (enemyType == EnemyType.Ranged)
            stat = new EnemyStatus(enemyType, 150.0f, 15.0f, 7.0f, 5.0f, 5.0f, 15.0f, enemyElement);

        return stat;
    }
}
