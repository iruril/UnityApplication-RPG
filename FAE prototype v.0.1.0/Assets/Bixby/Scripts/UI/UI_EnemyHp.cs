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
    private void Awake()
    {
        EnemyHps = new enemyHp();
    }
    void Update()
    {
        foreach (var item in EnemyHps.hpObjects)
        {
            int index = EnemyHps.hpObjects.IndexOf(item);
            Enemy target = EnemyHps.enemies[index];
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.gameObject.transform.position);
            bool isInScreen = screenPos.z > 0
                && screenPos.x > 0 && screenPos.x < 1920f
                && screenPos.y > 0 && screenPos.y < 1080f;
            if (target.State == Enemy.STATE.CHASE && isInScreen)
            {
                item.SetActive(true);
                item.transform.position = Camera.main.WorldToScreenPoint(target.gameObject.transform.position + Vector3.up * 5f);
                item.GetComponent<UnityEngine.UI.Slider>().value = EnemyHps.enemies[index].Stat.hp / EnemyHps.enemies[index].Stat.maxHp;
            }
            else
                item.SetActive(false);
        }
    }
    public GameObject getPrefab()
    {
        return hpPrefab;
    }
}
