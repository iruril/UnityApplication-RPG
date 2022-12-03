using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Services/Check Is Moved Service")]
    public class CheckIsMoved : Service
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public BoolReference Variable = new BoolReference(VarRefMode.DisableConstant);

        public override void Task()
        {
            GameObject obj = ObjRef.Value;

            if(obj == null)
                return;

            if (obj.tag == "FinalBoss")
                return;           
        }
    }
}
