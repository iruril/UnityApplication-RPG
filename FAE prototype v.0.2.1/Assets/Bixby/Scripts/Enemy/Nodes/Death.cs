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
            ObjRef.Value.GetComponent<Enemy>().Anim.SetTrigger("IsDied");
        }

        public override NodeResult Execute()
        {
            Time_ += Time.deltaTime * 1.5f;

            if((Time_ > UpdateInterval))
            {
                // Reset time and update destination
                Time_ = 0.0f;
                return NodeResult.success;
            }

            // 무언가 해야한다면 여기

            return NodeResult.running;
        }

        public override void OnExit()
        {
            Destroy(ObjRef.Value);
        }
    }
}
