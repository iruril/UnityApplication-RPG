using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/SetBarrier")]
    public class SetBarrier : Leaf
    {
        [Space]
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        
        public override NodeResult Execute()
        {
            GameObject obj = ObjRef.Value;

            if (obj == null)
            {
                return NodeResult.success;
            }

            obj.GetComponent<FinalBoss>().SetBarrier();

            return NodeResult.success;
        }
    }
}
