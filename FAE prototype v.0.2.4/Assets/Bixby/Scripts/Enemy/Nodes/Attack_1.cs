using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Attack_1")]
    public class Attack_1 : Leaf
    {
        public TransformReference SelfPosition;
        public GameObject Bullet;

        public override NodeResult Execute()
        {
            Transform self = SelfPosition.Value;

            Instantiate(Bullet, self.position, self.rotation);

            return NodeResult.success;
        }
    }
}
