using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Example/Set Random Position", 500)]
    public class SetRandomPosition : Leaf
    {
        public Bounds bounds;
        public GameObjectReference objRef = new GameObjectReference(VarRefMode.DisableConstant);
        public Vector3Reference blackboardVariable = new Vector3Reference(VarRefMode.DisableConstant);

        public Vector3 myCenter;

        public override void OnEnter()
        {
            
        }

        public override NodeResult Execute()
        {
            GameObject obj = objRef.Value;
            bounds.center = obj.GetComponent<Enemy>().centerPosition;

            // Random values per component inside bounds
            blackboardVariable.Value = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
            return NodeResult.success;
        }
    }
}
