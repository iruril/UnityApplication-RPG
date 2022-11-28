using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Electricity Skill")]
    public class ElectricitySkill : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public GameObjectReference targetRef = new GameObjectReference(VarRefMode.DisableConstant);
        public FloatReference dist = new FloatReference(VarRefMode.DisableConstant);
        public Vector3 targetVec;

        public float Speed = 3.0f;

        public float UpdateInterval = 3.5f;
        public float Time_;
        
        public bool AnimOnOff;

        public override void OnEnter()
        {
            targetVec = targetRef.Value.transform.position;
            targetVec += ObjRef.Value.transform.forward * 10.0f;
            targetVec += ObjRef.Value.transform.right * 5.0f;

            AnimOnOff = false;

            ObjRef.Value.GetComponent<FinalBoss>().isAttacked = true;
            ObjRef.Value.GetComponent<FinalBoss>().elecAttack = true;

            Time_ = 0.0f;
        } 

        public override NodeResult Execute()
        {
            Transform self = ObjRef.Value.transform;
            Transform target;
            if (targetRef.Value == null)
                return NodeResult.failure;
            else    
                target = targetRef.Value.transform;
            Vector3 dir = target.position - self.position;

            Time_ += Time.deltaTime;

            if(!ObjRef.Value.GetComponent<FinalBoss>().isAttacked)
            {
                // Reset time and update destination
                Time_ = 0.0f;
                //ObjRef.Value.GetComponent<FinalBoss>().elecCount++;
                return NodeResult.success;
            }
            if (ObjRef.Value.GetComponent<FinalBoss>().Stat.hp <= 0.0f)
                return NodeResult.failure;
            if (Time_ <= 1.0f)
            {
                self.rotation = Quaternion.Lerp(self.rotation, Quaternion.LookRotation(dir), 
                                                Time.deltaTime * 20.0f);

                targetVec = targetRef.Value.transform.position;
                targetVec += ObjRef.Value.transform.forward * 10.0f;
                targetVec += ObjRef.Value.transform.right * 5.0f;
            }
            else
            {
                if (!AnimOnOff)
                {
                    ObjRef.Value.GetComponent<FinalBoss>().Anim.SetFloat("AnimSpeed", (Speed / (dist.Value+11.1f))*1.1f);
                    ObjRef.Value.GetComponent<FinalBoss>().Anim.SetTrigger("Electricity");
                    AnimOnOff = true;
                }

                ObjRef.Value.transform.position = Vector3.MoveTowards(transform.position,
                                                                targetVec,
                                                                Speed * Time.deltaTime);
            }

            return NodeResult.running;
        }

        public override void OnExit()
        {
            ObjRef.Value.GetComponent<FinalBoss>().Anim.SetFloat("AnimSpeed", 1.0f);
            ObjRef.Value.GetComponent<FinalBoss>().SkillCooldown = 10.0f;
            ObjRef.Value.GetComponent<FinalBoss>().elecAttack = false;
            ObjRef.Value.GetComponent<FinalBoss>().isHitted = false;
        }
    }

}