using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Water Skill")]
    public class WaterSkill : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public GameObjectReference targetObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        
        public float UpdateInterval = 5.5f;
        public float Time_ = 0.0f;
        public int count = 0;

        GameObject obj;
        Vector3 target;
        Vector3 p;
        Vector3 p1;
        Vector3 p2;
        Vector3 p3;
        Vector3 p4;

        public override void OnEnter()
        {
            //ObjRef.Value.GetComponent<FinalBoss>().Anim.SetTrigger("qwe");
            p  = new Vector3(1140.0f, -0.4521f, 350.0f);
            p1 = new Vector3(1080.0f, 85.0f, 285.0f);
            p2 = new Vector3(1200.0f, 85.0f, 285);
            p3 = new Vector3(1100.0f, 85.0f, 420.0f);
            p4 = new Vector3(1200.0f, 85.0f, 425.0f);

            ObjRef.Value.transform.position = p;

            ObjRef.Value.GetComponent<FinalBoss>().Anim.SetTrigger("Water");
            ObjRef.Value.GetComponent<FinalBoss>().isAttacked = true;
        }

        public override NodeResult Execute()
        {
            obj = ObjRef.Value;

            Transform self = ObjRef.Value.transform;
            Transform target;

            if (targetObjRef.Value == null)
                return NodeResult.failure;
            else    
                target = targetObjRef.Value.transform;

            Vector3 dir = target.position - self.position;

            Time_ += Time.deltaTime;

            if(!ObjRef.Value.GetComponent<FinalBoss>().isAttacked)
            {
                // Reset time and update destination
                Time_ = 0.0f;
                return NodeResult.success;
            }

            if (Time_ <= 1.0)
            {
                self.rotation = Quaternion.Lerp(self.rotation, Quaternion.LookRotation(dir), 
                                                Time.deltaTime * 10.0f);
            }

            if (count == 0)
            {
                Instantiate(obj.GetComponent<FinalBoss>().WaterBall, p1, transform.rotation);
                Instantiate(obj.GetComponent<FinalBoss>().WaterBall, p2, transform.rotation);
                Instantiate(obj.GetComponent<FinalBoss>().WaterBall, p3, transform.rotation);
                Instantiate(obj.GetComponent<FinalBoss>().WaterBall, p4, transform.rotation);

                Instantiate(obj.GetComponent<FinalBoss>().WaterBallSpawner,
                        new Vector3(target.position.x, target.position.y+10.0f, target.position.z),
                        transform.rotation);

                count ++;
            }
            
            ObjRef.Value.transform.position = p;

            return NodeResult.running;
            //return NodeResult.running;
        }

        public override void OnExit()
        {
            //ObjRef.Value.GetComponent<FinalBoss>().Count = 0;

            ObjRef.Value.GetComponent<FinalBoss>().SkillCooldown = 10.0f;
        }
    }
}