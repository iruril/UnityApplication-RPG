using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusControl : MonoBehaviour, IDamgeable
{
    public const float GravityForce = 9.8f;

    public float myStartingHealth = 100.0f;
    public float health;
    public float myStartingStamina = 100.0f;
    public float stamina;

    protected bool isHitted;
    public bool dead;
    public float dealtDamage;

    public float dashStaminaAmount = 20.0f;
    public float runStaminaAmount = 10.0f;

    protected bool isDashed;
    public float dashDistance = 5.0f;

    public float Speed = 5.0f;
    public float RunSpeed = 10.0f;
    public float DashSpeed = 25.0f;
    public float myCurrentSpeed;
    public float JumpPower = 5.0f;

    protected float rotationSpeed = 60.0f;

    protected virtual void Start()
    {
        health = myStartingHealth;
        stamina = myStartingStamina;
        myCurrentSpeed = Speed;
        isDashed = false;
    }

    public virtual void TakeHit(float damage)
    {
        isHitted = true;
        health -= damage;
        dealtDamage = Mathf.Round(damage * 10) * 0.1f;
        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    public virtual void StaminaUse(float amount)
    {
        stamina -= amount;
    }

    public virtual void StaminaTickUse(float amount)
    {
        stamina -= Time.deltaTime * amount;
    }

    public virtual void StaminaRegerenate()
    {
        if(stamina != myStartingStamina)
        {
            stamina += Time.deltaTime * 10f;
        }

        if(stamina > myStartingStamina)
        {
            stamina = myStartingStamina;
        }
    }

    protected virtual void Die()
    {
        dead = true;
        GameObject.Destroy(gameObject);
    }
}