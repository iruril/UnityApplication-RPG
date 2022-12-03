using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Change Element")]
    public class ChangeElement : Leaf
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

            obj.GetComponent<FinalBoss>().ChangePhase();

            return NodeResult.success;
        }
    }
}
