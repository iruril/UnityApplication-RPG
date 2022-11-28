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
            GameObject obj = SelfObjRef.Value;

            if (obj == null)
            {
                return;
            }

            if (obj.tag == "Enemy")
                TargetObjRef.Value = obj.GetComponent<Enemy>().target;
            else if (obj.tag == "DungeonBoss")
                TargetObjRef.Value = obj.GetComponent<DungeonBoss>().target;
            else if (obj.tag == "FinalBoss")
                TargetObjRef.Value = obj.GetComponent<FinalBoss>().Target;

        }
    }
}
