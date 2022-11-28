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
        public FloatReference Variable = new FloatReference(VarRefMode.DisableConstant);

        public override void OnEnter()
        {
            if (ObjRef.Value.GetComponent<Enemy>().shootCount < 2)
            {
                ObjRef.Value.GetComponent<Enemy>().Bullet.GetComponent<BulletEne>().isCharged = false;
                ObjRef.Value.GetComponent<Enemy>().shootCount++;
            }
            else
            {
                ObjRef.Value.GetComponent<Enemy>().Bullet.GetComponent<BulletEne>().isCharged = true;
                ObjRef.Value.GetComponent<Enemy>().shootCount = 0;
            }
            ObjRef.Value.GetComponent<Enemy>().Anim.SetBool("IsAttack", true);
            ObjRef.Value.GetComponent<Enemy>().isAttacked = true;
        }

        public override NodeResult Execute()
        {
            GameObject obj = ObjRef.Value;

            if(!ObjRef.Value.GetComponent<Enemy>().isAttacked)
            {
                // Reset time and update destination
                return NodeResult.success;
            }
            if (((int)obj.GetComponent<Enemy>().State == 3) ||
                (obj.GetComponent<Enemy>().Stat.hp <= 0) || 
                (Variable.Value > ObjRef.Value.GetComponent<Enemy>().Stat.attackRange))
                return NodeResult.failure;

            // 무언가 해야한다면 여기

            return NodeResult.running;
        }

        public override void OnExit()
        {
            ObjRef.Value.GetComponent<Enemy>().Anim.SetBool("IsAttack", false);
        }
    }
}
