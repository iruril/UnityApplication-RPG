using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Look Target")]
    public class LookTarget : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public GameObjectReference TargetObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public FloatReference Variable = new FloatReference(VarRefMode.DisableConstant);

        public float delay;

        public override void OnEnter()
        {
            delay = 0.0f;
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
            Vector3 rayVector = new Vector3(self.position.x, self.position.y +1.0f, self.position.z);
            RaycastHit hitInfo;

            if (ObjRef.Value.tag == "Enemy")
            {
                //delay += Time.deltaTime;
                delay += Time.deltaTime / ObjRef.Value.GetComponent<Enemy>().FireRate;

                //(Time_ > UpdateInterval) ||
                if(((delay > ObjRef.Value.GetComponent<Enemy>().Stat.attackSpeed) && 
                    Physics.BoxCast(rayVector, self.lossyScale/2, self.forward, out hitInfo, self.rotation, ObjRef.Value.GetComponent<Enemy>().Stat.sight, ObjRef.Value.GetComponent<Enemy>().Mask)))
                {
                    // Reset time and update destination
                    return NodeResult.success;
                }
                if ((Variable.Value > ObjRef.Value.GetComponent<Enemy>().Stat.attackRange) ||
                    ((int)ObjRef.Value.GetComponent<Enemy>().State == 3) || (ObjRef.Value.GetComponent<Enemy>().Stat.hp <= 0))
                    return NodeResult.failure;
            }
            else if (ObjRef.Value.tag == "DungeonBoss")
            {
                delay += Time.deltaTime / ObjRef.Value.GetComponent<DungeonBoss>().FireRate;

                if((delay > ObjRef.Value.GetComponent<DungeonBoss>().Stat.attackSpeed) && 
                    Physics.BoxCast(rayVector, self.lossyScale/2, self.forward, out hitInfo, self.rotation, ObjRef.Value.GetComponent<DungeonBoss>().Stat.sight, ObjRef.Value.GetComponent<DungeonBoss>().Mask))
                {
                    // Reset time and update destination
                    return NodeResult.success;
                }
                if ((Variable.Value > ObjRef.Value.GetComponent<DungeonBoss>().Stat.attackRange) ||
                    (ObjRef.Value.GetComponent<DungeonBoss>().Stat.hp <= 0))
                    return NodeResult.failure;
            }
            else if (ObjRef.Value.tag == "FinalBoss")
            {
                delay += Time.deltaTime / ObjRef.Value.GetComponent<FinalBoss>().FireRate;

                if((delay > ObjRef.Value.GetComponent<FinalBoss>().Stat.attackSpeed) && 
                    Physics.BoxCast(rayVector, self.lossyScale/2, self.forward, out hitInfo, self.rotation, ObjRef.Value.GetComponent<FinalBoss>().Stat.sight, ObjRef.Value.GetComponent<FinalBoss>().Mask))
                {
                    // Reset time and update destination
                    return NodeResult.success;
                }
                if ((Variable.Value > ObjRef.Value.GetComponent<FinalBoss>().Stat.attackRange) ||
                    (ObjRef.Value.GetComponent<FinalBoss>().Stat.hp <= 0))
                    return NodeResult.failure;
            }
            
            // self.rotation = Quaternion.Lerp(self.rotation, Quaternion.LookRotation(dir), 
            //                               Time.deltaTime * 20.0f);

            self.rotation = Quaternion.Lerp(self.rotation, Quaternion.LookRotation(dir), 
                                          Time.deltaTime * 20.0f);

            Debug.DrawRay(rayVector, self.forward * 10.0f, Color.red);

            return NodeResult.running;
        }
    }
}
