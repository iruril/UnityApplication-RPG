using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Run To Sleep")]
    public class RunToSleep : Leaf
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

            obj.GetComponent<Enemy>().RunToSleepChange();

            return NodeResult.success;
        }
    }
}
