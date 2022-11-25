using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Services/Find Target Service")]
    public class FindTargetService : Service
    {
        [Tooltip("Sphere radius")]
        public GameObjectReference SelfObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public GameObjectReference TargetObjRef = new GameObjectReference(VarRefMode.DisableConstant);

        public override void Task()
        {
            
            TargetObjRef.Value = SelfObjRef.Value.GetComponent<Enemy>().target;
            
        }
    }
}
