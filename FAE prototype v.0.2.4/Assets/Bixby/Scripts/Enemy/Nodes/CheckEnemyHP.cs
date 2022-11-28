using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Services/Check Enemy HP Service")]
    public class CheckEnemyHP : Service
    {
        [Space]
        public GameObjectReference ObjRef = new GameObjectReference(VarRefMode.DisableConstant);
        public FloatReference Variable = new FloatReference(VarRefMode.DisableConstant);
        
        public override void Task()
        {
            GameObject obj = ObjRef.Value;

            if (obj == null)
            {
                return;
            }

            if (obj.tag == "Enemy")
                Variable.Value = obj.GetComponent<Enemy>().Stat.hp;
            else if (obj.tag == "DungeonBoss")
                Variable.Value = obj.GetComponent<DungeonBoss>().Stat.hp;
            else if (obj.tag == "FinalBoss")
                Variable.Value = obj.GetComponent<FinalBoss>().Stat.hp;
        }
    }
}
