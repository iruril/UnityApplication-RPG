using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/MeleeDashAttack")]
    public class MeleeDashAttack : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public GameObjectReference TargetObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public FloatReference Variable = new FloatReference(VarRefMode.DisableConstant);
        public float Speed = 3.0f;

        public float UpdateInterval = 3.0f;
        public float Time_ = 0.0f;

        public bool AnimOnOff;

        public Vector3 targetVec;

        public override void OnEnter()
        {
            AnimOnOff = false;
            targetVec = TargetObjRef.Value.transform.position;
            if (ObjRef.Value.tag == "Enemy")
            {
                targetVec += ObjRef.Value.transform.forward*-2.0f;
                ObjRef.Value.GetComponent<Enemy>().isAttacked = true;
            }
            else if (ObjRef.Value.tag == "DungeonBoss")
            {
                targetVec += ObjRef.Value.transform.forward*-2.0f;
                ObjRef.Value.GetComponent<DungeonBoss>().isAttacked = true;
            }
            else if (ObjRef.Value.tag == "FinalBoss")
            {
                targetVec += ObjRef.Value.transform.forward*-4.0f;
                ObjRef.Value.GetComponent<FinalBoss>().isAttacked = true;
            }
        }

        public override NodeResult Execute()
        {
            Transform self = ObjRef.Value.transform;
            Transform target;
            if (TargetObjRef.Value == null)
                return NodeResult.failure;
            else    
                target = TargetObjRef.Value.transform;
            Vector3 dir = target.position - self.position;
            
            Time_ += Time.deltaTime;

            if (ObjRef.Value.tag == "Enemy")
            {
                if(!ObjRef.Value.GetComponent<Enemy>().isAttacked)
                    return NodeResult.success;
                if (ObjRef.Value.GetComponent<Enemy>().Stat.hp <= 0)
                    return NodeResult.failure;
                
                if (Time_ <= 0.5f)
                    self.rotation = Quaternion.Lerp(self.rotation, Quaternion.LookRotation(dir),
                                                    Time.deltaTime * 20.0f);
                else
                {
                    if (!AnimOnOff)
                    {
                        ObjRef.Value.GetComponent<Enemy>().Anim.SetFloat("AnimSpeed", (Speed/2 / Variable.Value) * 1.25f);
                        ObjRef.Value.GetComponent<Enemy>().Anim.SetTrigger("IsDash");
                        AnimOnOff = true;
                    }
                    ObjRef.Value.transform.position = Vector3.MoveTowards(transform.position,
                                                                        targetVec,
                                                                        Speed/2 * Time.deltaTime);
                }
            }
            else if (ObjRef.Value.tag == "DungeonBoss")
            {
                if(!ObjRef.Value.GetComponent<DungeonBoss>().isAttacked)
                    return NodeResult.success;
                if (ObjRef.Value.GetComponent<DungeonBoss>().Stat.hp <= 0)
                    return NodeResult.failure;
                
                if (Time_ <= 1.0f)
                    self.rotation = Quaternion.Lerp(self.rotation, Quaternion.LookRotation(dir),
                                                    Time.deltaTime * 20.0f);
                else
                {
                    if (!AnimOnOff)
                    {
                        ObjRef.Value.GetComponent<DungeonBoss>().Anim.SetFloat("AnimSpeed", (Speed / Variable.Value) * 1.43f);
                        ObjRef.Value.GetComponent<DungeonBoss>().Anim.SetTrigger("isDashAttacked");
                        AnimOnOff = true;
                    }
                    ObjRef.Value.transform.position = Vector3.MoveTowards(transform.position,
                                                                        targetVec,
                                                                        Speed * Time.deltaTime);
                }
            }
            else if (ObjRef.Value.tag == "FinalBoss")
            {
                if(!ObjRef.Value.GetComponent<FinalBoss>().isAttacked)
                    return NodeResult.success;
                if (ObjRef.Value.GetComponent<FinalBoss>().Stat.hp <= 0)
                    return NodeResult.failure;

                if (Time_ <= 1.0f)
                    self.rotation = Quaternion.Lerp(self.rotation, Quaternion.LookRotation(dir),
                                                    Time.deltaTime * 20.0f);
                else
                {
                    if (!AnimOnOff)
                    {
                        ObjRef.Value.GetComponent<FinalBoss>().Anim.SetFloat("AnimSpeed", (Speed / Variable.Value) * 1.1f);
                        ObjRef.Value.GetComponent<FinalBoss>().Anim.SetTrigger("isDashAttack");
                        AnimOnOff = true;
                    }
                    ObjRef.Value.transform.position = Vector3.MoveTowards(transform.position,
                                                                        targetVec,
                                                                        Speed * Time.deltaTime);
                }
            }
            
            return NodeResult.running;
        }

        public override void OnExit()
        {
            if (ObjRef.Value.tag == "Enemy")
                ObjRef.Value.GetComponent<Enemy>().Anim.SetFloat("AnimSpeed", 1.0f);
            else if (ObjRef.Value.tag == "DungeonBoss")
                ObjRef.Value.GetComponent<DungeonBoss>().Anim.SetFloat("AnimSpeed", 1.0f);
            else if (ObjRef.Value.tag == "FinalBoss")
                ObjRef.Value.GetComponent<FinalBoss>().Anim.SetFloat("AnimSpeed", 1.0f);
            Time_ = 0.0f;
        }
            
    }
}
