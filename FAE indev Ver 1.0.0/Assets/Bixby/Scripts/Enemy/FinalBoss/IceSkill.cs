using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Ice Skill")]
    public class IceSkill : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public GameObjectReference targetRef = new GameObjectReference(VarRefMode.DisableConstant);
        public FloatReference dist = new FloatReference(VarRefMode.DisableConstant);
        public Vector3 targetVec;

        public float UpdateInterval = 3.5f;
        public float Time_ = 0.0f;

        public float Speed = 30.0f;
        public float EnemyToPlayerDistance; //거리
        public bool AnimOnOff;

        public override void OnEnter()
        {
            targetVec = targetRef.Value.transform.position;
            targetVec += ObjRef.Value.transform.forward*4.0f;

            EnemyToPlayerDistance = Vector3.Distance(targetVec, ObjRef.Value.transform.position);

            AnimOnOff = false;

            ObjRef.Value.GetComponent<FinalBoss>().isAttacked = true;
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
                return NodeResult.success;
            }
            if (Time_ <= 1.0f)
                self.rotation = Quaternion.Lerp(self.rotation, Quaternion.LookRotation(dir), 
                                                Time.deltaTime * 20.0f);
            else
            {
                if (!AnimOnOff)
                {
                    ObjRef.Value.GetComponent<FinalBoss>().Anim.SetFloat("AnimSpeed", (Speed / EnemyToPlayerDistance)*1.4f);
                    ObjRef.Value.GetComponent<FinalBoss>().Anim.SetTrigger("Ice");
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
            ObjRef.Value.GetComponent<FinalBoss>().SkillCooldown = 10.0f;
            ObjRef.Value.GetComponent<FinalBoss>().Anim.SetFloat("AnimSpeed", 1.0f);
        }
    }

}