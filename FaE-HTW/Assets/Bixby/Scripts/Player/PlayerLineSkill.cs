using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLineSkill : MonoBehaviour
{
    //나중에 Get으로 다 바꿀 예정.
    public ParticleSystem NormalSkill;

    public ParticleSystem ChargeSkill_Fire;

    public ParticleSystem ChargeSkill_Ice;

    public ParticleSystem ChargeSkill_Water;

    public ParticleSystem ChargeSkill_Electro;

    public ParticleSystem EnemyHitParticle;

    void Start()
    {
    }

    public void ShowAttackEffect(int attackType, int elementType, Transform pos)
    {
        if(attackType == 6)
        {
            switch (elementType)
            {
                case -1:
                    Instantiate(NormalSkill, pos);
                    break;
                case 0:
                    Instantiate(ChargeSkill_Fire, pos);
                    break;
                case 1:
                    Instantiate(ChargeSkill_Ice, pos);
                    break;
                case 2:
                    Instantiate(ChargeSkill_Water, pos);
                    break;
                case 3:
                    Instantiate(ChargeSkill_Electro, pos);
                    break;
            }
        }
        else
        {
            Instantiate(NormalSkill, pos);
        }
    }
    public void ShowHitEffect(Vector3 pos)
    {
        Instantiate(EnemyHitParticle, pos, this.transform.rotation);
    }
}
