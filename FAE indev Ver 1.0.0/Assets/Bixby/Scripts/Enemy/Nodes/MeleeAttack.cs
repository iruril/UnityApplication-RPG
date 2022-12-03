using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/MeleeAttack")]
    public class MeleeAttack : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public FloatReference Variable = new FloatReference(VarRefMode.DisableConstant);

        public float UpdateInterval = 5.0f;
        public float Time_ = 0.0f;

        public override void OnEnter()
        {
            if (ObjRef.Value.tag == "Enemy")
            {
                ObjRef.Value.GetComponent<Enemy>().Anim.SetBool("IsAttack", true);
                ObjRef.Value.GetComponent<Enemy>().isAttacked = true;
            }
            else if (ObjRef.Value.tag == "DungeonBoss")
            {
                ObjRef.Value.GetComponent<DungeonBoss>().Anim.SetTrigger("isAttacked");
                ObjRef.Value.GetComponent<DungeonBoss>().isAttacked = true;
            }
            else if (ObjRef.Value.tag == "FinalBoss")
            {
                ObjRef.Value.GetComponent<FinalBoss>().Anim.SetTrigger("isAttack");
                ObjRef.Value.GetComponent<FinalBoss>().isAttacked = true;
            }
        }

        public override NodeResult Execute()
        {
            Time_ += Time.deltaTime;

            if (ObjRef.Value.tag == "Enemy")
            {
                if(!ObjRef.Value.GetComponent<Enemy>().isAttacked)
                {
                    // Reset time and update destination
                    Time_ = 0.0f;
                    return NodeResult.success;
                }
                if (((int)ObjRef.Value.GetComponent<Enemy>().State == 3) || 
                    (ObjRef.Value.GetComponent<Enemy>().Stat.hp <= 0) || 
                    (Variable.Value > ObjRef.Value.GetComponent<Enemy>().Stat.attackRange))
                    return NodeResult.failure;
            }
            else if (ObjRef.Value.tag == "DungeonBoss")
            {
                if(!ObjRef.Value.GetComponent<DungeonBoss>().isAttacked)
                {
                    // Reset time and update destination
                    Time_ = 0.0f;
                    return NodeResult.success;
                }
                if (ObjRef.Value.GetComponent<DungeonBoss>().Stat.hp <= 0)
                    return NodeResult.failure;
            }
            else if (ObjRef.Value.tag == "FinalBoss")
            {
                if(!ObjRef.Value.GetComponent<FinalBoss>().isAttacked)
                {
                    // Reset time and update destination
                    Time_ = 0.0f;
                    return NodeResult.success;
                }
                if (ObjRef.Value.GetComponent<FinalBoss>().Stat.hp <= 0)
                    return NodeResult.failure;
            }

            // 무언가 해야한다면 여기

            return NodeResult.running;
        }

        public override void OnExit()
        {
            if (ObjRef.Value.tag == "Enemy")
            {
                if (Variable.Value <= ObjRef.Value.GetComponent<Enemy>().Stat.attackRange)
                    //ObjRef.Value.GetComponent<Enemy>().MeleeAttack();
                

                ObjRef.Value.GetComponent<Enemy>().Anim.SetBool("IsAttack", false);
            }
        }
    }
}
