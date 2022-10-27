using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EnemyHp : MonoBehaviour
{
    [SerializeField] GameObject hpPrefab;
    public class enemyHp
    {
        public List<GameObject> hpObjects;
        public List<Enemy> enemies;
        public enemyHp()
        {
            hpObjects = new List<GameObject>();
            enemies = new List<Enemy>();
        }
    }
    public static enemyHp EnemyHps;
    void Start()
    {
        EnemyHps = new enemyHp();
        foreach (var item in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EnemyHps.hpObjects.Add(Instantiate(hpPrefab, GameObject.Find("UI").transform.GetChild(1)));
            EnemyHps.enemies.Add(item.GetComponent<Enemy>());
        }
    }

    void Update()
    {
        foreach (var item in EnemyHps.hpObjects)
        {
            int index = EnemyHps.hpObjects.IndexOf(item);
            Enemy target = EnemyHps.enemies[index];
            if (target.State == Enemy.STATE.CHASE)
            {
                item.SetActive(true);
                item.transform.position = Camera.main.WorldToScreenPoint(target.gameObject.transform.position + Vector3.up * 5f);
                item.GetComponent<UnityEngine.UI.Slider>().value = EnemyHps.enemies[index].Stat.hp / EnemyHps.enemies[index].Stat.maxHp;
            }
            else
                item.SetActive(false);
        }
    }
}
