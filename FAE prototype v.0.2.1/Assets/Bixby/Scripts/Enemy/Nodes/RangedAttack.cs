using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Tasks/RangedAttack")]
    public class RangedAttack : Leaf
    {
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public GameObject Bullet;
        public FloatReference Variable = new FloatReference(VarRefMode.DisableConstant);

        public float UpdateInterval = 2.0f;
        public float Time_ = 0.0f;
        
        public float AttackRange = 0.0f;


        public override void OnEnter()
        {
            ObjRef.Value.GetComponent<Enemy>().Anim.SetBool("IsAttack", true);
        }

        public override NodeResult Execute()
        {
            GameObject obj = ObjRef.Value;

            int Element;
            // CheckElements
            Element = (int)obj.GetComponent<Enemy>().Element;

            Time_ += Time.deltaTime * 1.5f;

            if((Time_ > UpdateInterval) || ((int)obj.GetComponent<Enemy>().State == 3) ||
                (obj.GetComponent<Enemy>().Stat.hp <= 0) || (Variable.Value > AttackRange))
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
            if (Variable.Value <= AttackRange)
                Instantiate(Bullet, ObjRef.Value.transform.position, ObjRef.Value.transform.rotation);
            ObjRef.Value.GetComponent<Enemy>().Anim.SetBool("IsAttack", false);
        }
    }
}
