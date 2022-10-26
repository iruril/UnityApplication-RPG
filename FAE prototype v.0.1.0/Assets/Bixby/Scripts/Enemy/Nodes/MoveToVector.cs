using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/Move To Vector")]
    public class MoveToVector : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public Vector3Reference TargetPosition;
        public UnityEngine.AI.NavMeshAgent Agent;
        public float StopDistance = 2f;
        [Tooltip("How often target position should be updated")]
        public float UpdateInterval = 1f;
        private float Time_ = 0;

        public override void OnEnter()
        {
            Time_ = 0;
            Agent.isStopped = false;
            Agent.SetDestination(TargetPosition.Value);
            ObjRef.Value.GetComponent<Enemy>().Anim.SetBool("IsMove", true);
        }

        public override NodeResult Execute()
        {
            Vector3 target = TargetPosition.Value;

            Time_ += Time.deltaTime;
            // Update destination every given interval
            if (Time_ > UpdateInterval)
            {
                // Reset time and update destination
                Time_ = 0;
                Agent.SetDestination(TargetPosition.Value);
            }
            // Check if path is ready
            if (Agent.pathPending)
            {
                return NodeResult.running;
            }
            // Check if agent is very close to destination
            if ((Agent.remainingDistance <= StopDistance) || 
                (ObjRef.Value.GetComponent<Enemy>().Stat.hp <= 0))
            {
                return NodeResult.success;
            }
            // Check if there is any path (if not pending, it should be set)
            if (Agent.hasPath)
            {
                return NodeResult.running;
            }
            // By default return failure
            return NodeResult.failure;
        }

         public override void OnExit()
        {
            Agent.isStopped = true;
            // agent.ResetPath();
            ObjRef.Value.GetComponent<Enemy>().Anim.SetBool("IsMove", false);
        }

        public override bool IsValid()
        {
            return !(TargetPosition.isInvalid || Agent == null);
        }
    }
}
