using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EnemyHp : MonoBehaviour
{
    [SerializeField] GameObject hpPrefab;
    [SerializeField] GameObject shieldPrefab;
    public class enemyHp
    {
        public List<GameObject> hpObjects;
        //public List<Enemy> enemies;
        //public FinalBoss boss;
        public List<GameObject> EnemyObjects;
        public List<GameObject> ShieldObjects;
        public enemyHp()
        {
            hpObjects = new List<GameObject>();
            //enemies = new List<Enemy>();
            //boss = null;
            EnemyObjects = new List<GameObject>();
            ShieldObjects = new List<GameObject>();
        }
        public void GaugeOff()
        {
            foreach (var item in hpObjects)
            {
                item.SetActive(false);
                EnemyHps.ShieldObjects[EnemyHps.hpObjects.IndexOf(item)].SetActive(false);
            }
        }
    }
    public static enemyHp EnemyHps;
    private void Awake()
    {
        EnemyHps = new enemyHp();
    }
    void Update()
    {
        foreach (var item in EnemyHps.hpObjects)
        {
            int index = EnemyHps.hpObjects.IndexOf(item);
            switch (EnemyHps.EnemyObjects[index].tag)
            {
                case "Enemy":
                    Enemy target = EnemyHps.EnemyObjects[index].GetComponent<Enemy>();
                    if (target.State == Enemy.STATE.CHASE && isInScreen(target.gameObject))
                    {
                        gaugeSet(EnemyHps.EnemyObjects[index].tag, true, item);
                        if (target.isSetShield())
                            gaugeSet(EnemyHps.EnemyObjects[index].tag, false, EnemyHps.ShieldObjects[index]);
                    }
                    else
                    {
                        item.SetActive(false);
                        EnemyHps.ShieldObjects[index].SetActive(false);
                    }
                    break;
                case "DungeonBoss":
                    DungeonBoss targetDungeon = EnemyHps.EnemyObjects[index].GetComponent<DungeonBoss>();
                    if (isInScreen(targetDungeon.gameObject))
                    {
                        if (targetDungeon.Stat.hp != targetDungeon.Stat.maxHp)
                            gaugeSet(EnemyHps.EnemyObjects[index].tag, true, item);
                        if (targetDungeon.Stat.barrier != targetDungeon.Stat.maxBarrier)
                            gaugeSet(EnemyHps.EnemyObjects[index].tag, false, EnemyHps.ShieldObjects[index]);
                    }
                    else
                    {
                        item.SetActive(false);
                        EnemyHps.ShieldObjects[index].SetActive(false);
                    }
                    break;
                case "FinalBoss":
                    FinalBoss targetBoss = EnemyHps.EnemyObjects[index].GetComponent<FinalBoss>();
                    if (isInScreen(targetBoss.gameObject))
                    {
                        if (targetBoss.Stat.hp != targetBoss.Stat.maxHp)
                            gaugeSet(EnemyHps.EnemyObjects[index].tag, true, item);
                        if (targetBoss.Stat.barrier!=targetBoss.Stat.maxBarrier)
                            gaugeSet(EnemyHps.EnemyObjects[index].tag, false, EnemyHps.ShieldObjects[index]);
                    }
                    else
                    {
                        item.SetActive(false);
                        EnemyHps.ShieldObjects[index].SetActive(false);
                    }
                    break;
                default:
                    break;
            }
        }
        //if (EnemyHps.boss == null)
        //{
        //    foreach (var item in EnemyHps.hpObjects)
        //    {
        //        int index = EnemyHps.hpObjects.IndexOf(item);
        //        Enemy target = EnemyHps.enemies[index];//EnemyHps.hpObjects.IndexOf(item);

        //        if (target.State == Enemy.STATE.CHASE && isInScreen(target.gameObject))
        //        {
        //            gaugeSet(false, true, item);
        //        }
        //        else
        //            item.SetActive(false);
        //    }
        //}
        //else
        //{
        //    FinalBoss target = EnemyHps.boss;
        //    GameObject item = EnemyHps.hpObjects[0];
        //    GameObject itemShield = EnemyHps.shieldObject;
        //    if (isInScreen(target.gameObject) && target.Stat.barrier != target.Stat.maxBarrier)
        //    {
        //        if (target.Stat.barrier > 0)
        //        {
        //            itemShield.SetActive(true);
        //            item.SetActive(false);
        //            gaugeSet(true, false, itemShield);
        //        }
        //        else
        //        {
        //            if (!item.activeSelf)
        //                item.SetActive(true);
        //            if (itemShield.activeSelf)
        //                itemShield.SetActive(false);
        //            gaugeSet(true, true, item);
        //        }
        //    }
        //    else
        //    {
        //        item.SetActive(false);
        //        itemShield.SetActive(false);
        //    }
        //}
    }
    public GameObject getPrefab(bool isHp)
    {
        if (isHp)
            return hpPrefab;
        else
            return shieldPrefab;
    }

    bool isInScreen(GameObject target)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
        bool isInScreen = screenPos.z > 0
            && screenPos.x > 0 && screenPos.x < 1920f
            && screenPos.y > 0 && screenPos.y < 1080f;
        return isInScreen;
    }

    void gaugeSet(string tag, bool isHp, GameObject item)
    {
        switch (tag)
        {
            case "Enemy":
                Enemy target;
                if (UI_Control.Inst.OpenedWindow != null)
                    if (UI_Control.Inst.OpenedWindow.name == "Map")
                    {
                        item.SetActive(false);
                        return;
                    }
                target = isHp ? EnemyHps.EnemyObjects[EnemyHps.hpObjects.IndexOf(item)].GetComponent<Enemy>() : EnemyHps.EnemyObjects[EnemyHps.ShieldObjects.IndexOf(item)].GetComponent<Enemy>();
                item.SetActive(true);
                item.transform.position = isHp ? Camera.main.WorldToScreenPoint(target.gameObject.transform.position + Vector3.up * 5f) : Camera.main.WorldToScreenPoint(target.gameObject.transform.position + Vector3.up * 3f);
                item.GetComponent<UnityEngine.UI.Slider>().value = isHp ? target.Stat.hp / target.Stat.maxHp : target.Stat.barrier / target.Stat.maxBarrier;
                break;
            case "DungeonBoss":
                DungeonBoss targetDungeon = isHp ? EnemyHps.EnemyObjects[EnemyHps.hpObjects.IndexOf(item)].GetComponent<DungeonBoss>() : EnemyHps.EnemyObjects[EnemyHps.ShieldObjects.IndexOf(item)].GetComponent<DungeonBoss>();
                if (isHp)
                    item.SetActive(true);
                else
                    item.SetActive(targetDungeon.isSetShield());
                item.transform.position = isHp ? Camera.main.WorldToScreenPoint(targetDungeon.gameObject.transform.position + Vector3.up * 5f) : Camera.main.WorldToScreenPoint(targetDungeon.gameObject.transform.position + Vector3.up * 3f);
                item.GetComponent<UnityEngine.UI.Slider>().value = isHp ? targetDungeon.Stat.hp / targetDungeon.Stat.maxHp : targetDungeon.Stat.barrier / targetDungeon.Stat.maxBarrier;
                break;
            case "FinalBoss":
                FinalBoss targetBoss = isHp ? EnemyHps.EnemyObjects[EnemyHps.hpObjects.IndexOf(item)].GetComponent<FinalBoss>() : EnemyHps.EnemyObjects[EnemyHps.ShieldObjects.IndexOf(item)].GetComponent<FinalBoss>();
                if (isHp)
                    item.SetActive(true);
                else
                    item.SetActive(targetBoss.isSetShield());
                item.transform.position = isHp ? Camera.main.WorldToScreenPoint(targetBoss.gameObject.transform.position + Vector3.up * 13f) : Camera.main.WorldToScreenPoint(targetBoss.gameObject.transform.position + Vector3.up * 10f);
                item.GetComponent<UnityEngine.UI.Slider>().value = isHp ? targetBoss.Stat.hp / targetBoss.Stat.maxHp : targetBoss.Stat.barrier / targetBoss.Stat.maxBarrier;
                break;
            default:
                break;
        }

    }
}
