using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusControl : ElementControl, IDamgeable
{
    public const float GravityForce = 30.0f;

    public float MyStartingHealth = 100.0f;
    public float Health;
    public float MyStartingStamina = 100.0f;
    public float Stamina;

    public float attackDamage = 10.0f;

    protected bool isHitted;
    public bool Dead;
    public float DealtDamage;

    public float DashStaminaAmount = 20.0f;
    public float RunStaminaAmount = 10.0f;
    public float SwimStaminaAmount = 10.0f;
    public float ChargeAttackStaminaAmount = 10.0f;

    protected bool isDashed;
    public float DashDistance = 5.0f;

    public float Speed = 5.0f;
    public float SwimSpeed = 4.0f;
    public float RunSpeed = 10.0f;
    public float DashSpeed = 20.0f;
    public float MyCurrentSpeed;
    public float JumpPower = 5.0f;

    public float ShootDistance = 50f;
    public float SwitchToChargeTime = 2.0f;
    public float FireRate = 0.25f;

    protected float rotationSpeed = 45.0f;
    protected float nextFire = 0.0f;

    public float AdditionalDamage = 1.0f; // 퍼센티지 스탯으로 연산되어, 받는 피해 추가계수를 나타낸다.
    public float SpeedMultiply = 1.0f; // 속도 감소 디버프, 혹은 속도 증가 버프를 위한 계수.

    protected override void Start()
    {
        base.Start();
        Health = MyStartingHealth;
        Stamina = MyStartingStamina;
        MyCurrentSpeed = Speed;
        isDashed = false;
    }
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.mySkillStartColor = FireSkillStartColor;
            this.mySkillEndColor = FireSkillEndColor;
            this.MyElement = ElementType.FIRE;
            Debug.Log("current element : FIRE");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.mySkillStartColor = IceSkillStartColor;
            this.mySkillEndColor = IceSkillEndColor;
            this.MyElement = ElementType.ICE;
            Debug.Log("current element : ICE");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.mySkillStartColor = WaterSkillStartColor;
            this.mySkillEndColor = WaterSkillEndColor;
            this.MyElement = ElementType.WATER;
            Debug.Log("current element : WATER");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            this.mySkillStartColor = ElectroSkillStartColor;
            this.mySkillEndColor = ElectroSkillEndColor;
            this.MyElement = ElementType.ELECTRICITY;
            Debug.Log("current element : ELECTRICITY");
        }
    }

    public virtual void TakeHit(float damage)
    {
        isHitted = true;
        Health -= damage * AdditionalDamage;
        DealtDamage = Mathf.Round(damage * 10) * 0.1f;
        if (Health <= 0 && !Dead)
        {
            die();
        }
    }
    public virtual void TakeElementHit(float damage, ElementControl.ElementType elementType)
    {
        isHitted = true;
        if (elementType != ElementType.NONE && elementType != ElementStack.Peek())
        {
            this.ElementStack.Push(elementType);
        }
        Health -= damage * AdditionalDamage;
        DealtDamage = Mathf.Round(damage * 10) * 0.1f;
        if (Health <= 0 && !Dead)
        {
            die();
        }
    }

    public virtual void TakeElementHit(float damage, EnemyElement element) //나중에 삭제될 가능성 있음.
    {
        isHitted = true;
        enemyElementCheck(element);
        float curDamage = attackedOnNormal(damage, EnemyElement, MyElement);

        // 처음에 빈 stack에서 peek하려다보니 문제가 생기는 것 같음. 아닐 수도 ㅎ
        // if (EnemyElement != ElementType.NONE && EnemyElement != ElementStack.Peek())
        // {
        //     this.ElementStack.Push(EnemyElement);
        // }
        Health -= curDamage;
        DealtDamage = Mathf.Round(curDamage * 10) * 0.1f;
        if (Health <= 0 && !Dead)
        {
            die();
        }
    }

    public virtual void StaminaUse(float amount)
    {
        Stamina -= amount;
    }

    public virtual void StaminaTickUse(float amount)
    {
        Stamina -= Time.deltaTime * amount;
    }

    public virtual void StaminaRegerenate()
    {
        if(Stamina != MyStartingStamina)
        {
            Stamina += Time.deltaTime * 10f;
        }

        if(Stamina > MyStartingStamina)
        {
            Stamina = MyStartingStamina;
        }
    }

    protected virtual void die()
    {
        Dead = true;
        GameObject.Destroy(gameObject);
    }
}