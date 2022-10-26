using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/MeleeDashAttack")]
    public class MeleeDashAttack : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public GameObjectReference TargetObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public FloatReference Variable = new FloatReference(VarRefMode.DisableConstant);
        public float Speed = 3.0f;

        public float UpdateInterval = 2.0f;
        public float Time_ = 0.0f;

        public float AttackRange = 0.0f;

        public override NodeResult Execute()
        {
            // int Element;
            // CheckElements
            // Element = (int)ObjRef.Value.GetComponent<Enemy>().Element;

            Time_ += Time.deltaTime * 1.5f;

            if(Time_ > UpdateInterval)
            {
                // Reset time and update destination
                Time_ = 0.0f;
                return NodeResult.success;
            }

            ObjRef.Value.transform.position = Vector3.MoveTowards(transform.position, 
                                                        TargetObjRef.Value.transform.position, 
                                                        Speed * Time.deltaTime);

            return NodeResult.running;
        }

        public override void OnExit()
        {
            // if (Variable.Value <= AttackRange)
            // {
                Collider[] colliders = Physics.OverlapSphere(transform.position, AttackRange, 
                                                ObjRef.Value.GetComponent<Enemy>().Mask, 
                                                QueryTriggerInteraction.Ignore);
            
                if(colliders.Length > 0)
                {
                    //colliders[0].GetComponent<Player>().TakeDamage(5.0f);
                    Debug.Log("Hit");
                }
            // }
        }
    }
}
