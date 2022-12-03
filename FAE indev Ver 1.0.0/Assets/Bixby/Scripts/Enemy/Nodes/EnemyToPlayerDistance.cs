using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Services/Enemy To Player Distance Service")]
    public class EnemyToPlayerDistance : Service
    {
        [Space]
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public GameObjectReference targetObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public FloatReference Variable = new FloatReference(VarRefMode.DisableConstant);

        public override void Task()
        {
            Transform t1 = ObjRef.Value.transform;
            Transform t2 = targetObjRef.Value.transform;
            if (t1 == null || t2 == null)
            {
                return;
            }
            Variable.Value = Vector3.Distance(t1.position, t2.position);
        }
    }
}
