using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/DungeonBoss Skill 1")]
    public class DungeonBossSkill_1 : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public GameObjectReference targetObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        
        public float UpdateInterval = 5.5f;
        public float Time_ = 0.0f;
        
        Vector3 p;
        Vector3 targetVec;

        bool use;

        public override void OnEnter()
        {
            p = ObjRef.Value.transform.position;
            targetVec = targetObjRef.Value.transform.position;

            use = false;

            ObjRef.Value.GetComponent<DungeonBoss>().Anim.SetTrigger("Skill_1");
            ObjRef.Value.GetComponent<DungeonBoss>().isAttacked = true;
        }

        public override NodeResult Execute()
        {
            Transform self = ObjRef.Value.transform;
            Transform target;

            if (targetObjRef.Value == null)
                return NodeResult.failure;
            else    
                target = targetObjRef.Value.transform;

            Vector3 dir = target.position - self.position;

            Time_ += Time.deltaTime;

            if(!ObjRef.Value.GetComponent<DungeonBoss>().isAttacked)
            {
                // Reset time and update destination
                Time_ = 0.0f;
                return NodeResult.success;
            }

            if (Time_ <= 1.0f)
            {
                self.rotation = Quaternion.Lerp(self.rotation, Quaternion.LookRotation(dir), 
                                                Time.deltaTime * 10.0f);
            }

            if (!use)
            {
                Instantiate(ObjRef.Value.GetComponent<DungeonBoss>().Skill_1_Obj,
                            targetVec, transform.rotation);
                use = true;
            }

            
            ObjRef.Value.transform.position = p;

            return NodeResult.running;
        }

        public override void OnExit()
        {
            ObjRef.Value.GetComponent<DungeonBoss>().SkillCooldown = 10.0f;

            use = false;
        }
    }
}