using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Services/Check Enemy Type Service")]
    public class CheckEnemyType : Service
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public IntReference Variable = new IntReference(VarRefMode.DisableConstant);

        public override void Task()
        {
            GameObject obj = ObjRef.Value;

            if (obj == null)
                return;

            Variable.Value = (int)obj.GetComponent<Enemy>().Stat.type;
        }
    }
}
