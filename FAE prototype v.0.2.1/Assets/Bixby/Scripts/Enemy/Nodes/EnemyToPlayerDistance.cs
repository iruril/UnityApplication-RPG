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
        public GameObjectReference Transform1;
        public FloatReference Variable = new FloatReference(VarRefMode.DisableConstant);

        public override void Task()
        {
            GameObject t1 = Transform1.Value;
            Transform t2 = t1.GetComponent<Enemy>().target.transform;
            if (t1 == null || t2 == null)
            {
                return;
            }
            Variable.Value = Vector3.Distance(t1.transform.position, t2.position);
        }
    }
}
