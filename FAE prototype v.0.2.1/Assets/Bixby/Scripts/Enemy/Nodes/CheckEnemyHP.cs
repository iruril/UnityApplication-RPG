using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Services/Check Enemy HP Service")]
    public class CheckEnemyHP : Service
    {
        [Space]
        public GameObjectReference Obj1;
        public FloatReference Variable = new FloatReference(VarRefMode.DisableConstant);
        
        public override void Task()
        {
            GameObject obj = Obj1.Value;

            if (obj == null)
            {
                return;
            }

            Variable.Value = obj.GetComponent<Enemy>().Stat.hp;
        }
    }
}
