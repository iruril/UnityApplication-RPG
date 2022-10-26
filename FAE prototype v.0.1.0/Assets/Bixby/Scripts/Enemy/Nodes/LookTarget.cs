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
        
        public float AttackRange = 0.0f;


        public override NodeResult Execute()
        {
            Transform self = ObjRef.Value.transform;
            Transform target = TargetObjRef.Value.transform;
            Vector3 dir = target.position - self.position;
            Vector3 rayVector = new Vector3(self.position.x, self.position.y + 1.0f, self.position.z);
            RaycastHit hitInfo;

            //(Time_ > UpdateInterval) || 
            if(Physics.Raycast(rayVector, self.forward, out hitInfo, 10.0f, ObjRef.Value.GetComponent<Enemy>().Mask) ||
                ((int)ObjRef.Value.GetComponent<Enemy>().State == 3) || (ObjRef.Value.GetComponent<Enemy>().Stat.hp <= 0) || 
                (Variable.Value > AttackRange))
            {
                // Reset time and update destination
                return NodeResult.success;
            }
            
            self.rotation = Quaternion.Lerp(self.rotation, Quaternion.LookRotation(dir), 
                                          Time.deltaTime * 5.0f);

            Debug.DrawRay(rayVector, self.forward * 10.0f, Color.red);

            return NodeResult.running;
        }
    }
}
