using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Sleep")]
    public class Sleep : Leaf
    {
        public GameObjectReference Obj1;

        public float UpdateInterval = 3.0f;
        public float Time_ = 0.0f;

        public override NodeResult Execute()
        {
            GameObject obj = Obj1.Value;

            Time_ += Time.deltaTime * 1.5f;

            if(Time_ > UpdateInterval)
            {
                // Reset time and update destination
                Time_ = 0;
                return NodeResult.success;
            }
            
            //obj.GetComponent<Enemy>().hp = 100.0f;

            return NodeResult.running;
        }
    }
}
