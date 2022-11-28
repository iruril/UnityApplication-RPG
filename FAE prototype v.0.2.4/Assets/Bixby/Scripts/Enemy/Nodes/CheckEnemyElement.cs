using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Services/Check Enemy Element Service")]
    public class CheckEnemyElement : Service
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public IntReference Variable = new IntReference(VarRefMode.DisableConstant);

        public override void Task()
        {
            GameObject obj = ObjRef.Value;

            if (obj == null)
                return;

            if (obj.tag == "Enenmy")
                Variable.Value = (int)obj.GetComponent<Enemy>().Stat.element;
            else if (obj.tag == "FinalBoss")
                Variable.Value = (int)obj.GetComponent<FinalBoss>().Stat.element;
        }
    }
}
