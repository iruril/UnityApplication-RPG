using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MBT;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Move To Transform")]
    public class MoveToTransform : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public GameObjectReference TargetRef =  new GameObjectReference(VarRefMode.DisableConstant);
        public NavMeshAgent agent;
        public float stopDistance = 2f;
        [Tooltip("How often target position should be updated")]
        public float updateInterval = 1f;
        private float time = 0;

        public override void OnEnter()
        {
            time = 0;
            agent.isStopped = false;
            agent.SetDestination(TargetRef.Value.transform.position);
            ObjRef.Value.GetComponent<Enemy>().Anim.SetBool("IsMove", true);
        }
        
        public override NodeResult Execute()
        {
            time += Time.deltaTime;
            // Update destination every given interval
            if (time > updateInterval)
            {
                // Reset time and update destination
                time = 0;
                agent.SetDestination(TargetRef.Value.transform.position);
            }
            // Check if path is ready
            if (agent.pathPending)
            {
                return NodeResult.running;
            }
            // Check if agent is very close to destination
            if ((agent.remainingDistance < stopDistance) ||
                (ObjRef.Value.GetComponent<Enemy>().Stat.hp <= 0))
            {
                return NodeResult.success;
            }
            // Check if there is any path (if not pending, it should be set)
            if (agent.hasPath)
            {
                return NodeResult.running;
            }
            // By default return failure
            return NodeResult.failure;
        }

        public override void OnExit()
        {
            ObjRef.Value.GetComponent<Enemy>().Anim.SetBool("IsMove", false);
            agent.isStopped = true;
            // agent.ResetPath();
        }

        public override bool IsValid()
        {
            return !(TargetRef.isInvalid || agent == null);
        }
    }
}
