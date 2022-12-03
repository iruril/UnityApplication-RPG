using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLineSkill : MonoBehaviour
{
    public ParticleSystem NormalSkill;

    public ParticleSystem ChargeSkill_Fire;

    public ParticleSystem ChargeSkill_Ice;

    public ParticleSystem ChargeSkill_Water;

    public ParticleSystem ChargeSkill_Electro;

    public GameObject NormalElementSkill;

    public GameObject ElementSkill_Fire;

    public GameObject ElementSkill_Ice;

    public GameObject ElementSkill_Water;

    public GameObject ElementSkill_Electro;

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
                    Instantiate(NormalSkill, pos.position, pos.rotation);
                    break;
                case 0:
                    Instantiate(ChargeSkill_Fire, pos.position, pos.rotation);
                    break;
                case 1:
                    Instantiate(ChargeSkill_Ice, pos.position, pos.rotation);
                    break;
                case 2:
                    Instantiate(ChargeSkill_Water, pos.position, pos.rotation);
                    break;
                case 3:
                    Instantiate(ChargeSkill_Electro, pos.position, pos.rotation);
                    break;
            }
        }
        else
        {
            Instantiate(NormalSkill, pos.position, pos.rotation);
        }
    }

    public void ShowSkillEffect(int elementType, Transform pos)
    {
        switch (elementType)
        {
            case -1:
                Instantiate(NormalElementSkill, pos.position, pos.rotation);
                break;
            case 0:
                Instantiate(ElementSkill_Fire, pos.position, pos.rotation);
                break;
            case 1:
                Instantiate(ElementSkill_Ice, pos.position, pos.rotation);
                break;
            case 2:
                Instantiate(ElementSkill_Water, pos.position, pos.rotation);
                break;
            case 3:
                Instantiate(ElementSkill_Electro, pos.position, pos.rotation);
                break;
        }
    }

    public void ShowHitEffect(Vector3 pos)
    {
        Instantiate(EnemyHitParticle, pos, this.transform.rotation);
    }
}
