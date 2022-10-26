using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/ReCharge")]
    public class ReCharge : Leaf
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

            obj.GetComponent<Enemy>().Stat.hp = obj.GetComponent<Enemy>().Stat.maxHp;
            obj.GetComponent<Enemy>().ReCharge();

            return NodeResult.success;
        }
    }
}
