using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Death")]
    public class Death : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);

        public float UpdateInterval = 2.0f;
        public float Time_ = 0.0f;


        public override void OnEnter()
        {
            if (ObjRef.Value.tag == "Enemy")
                ObjRef.Value.GetComponent<Enemy>().Anim.SetTrigger("IsDied");
            else if (ObjRef.Value.tag == "DungeonBoss")
                ObjRef.Value.GetComponent<DungeonBoss>().Anim.SetTrigger("isDied");
            else if (ObjRef.Value.tag == "FinalBoss")
                ObjRef.Value.GetComponent<FinalBoss>().Anim.SetTrigger("isDied");
        }

        public override NodeResult Execute()
        {
            return NodeResult.running;
        }

        public override void OnExit()
        {
            
        }
    }
}
