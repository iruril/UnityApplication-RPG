using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    private enum SimpleEnemyType
    { 
        COMMON,
        BOSS
    };

    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject[] doors;
    [SerializeField] private GameObject potal;
    private bool[] isOpen;
    private SimpleEnemyType[] enemyType;
    private bool[] isGroup;
    private void Start()
    {
        isOpen = new bool[doors.Length];
        isGroup = new bool[enemies.Length];
        enemyType = new SimpleEnemyType[enemies.Length];        

        for (int i = 0; i < enemyType.Length; i++)
        {
            if (enemies[i].gameObject.CompareTag("Untagged"))
            {
                isGroup[i] = true;

                if (enemies[i].gameObject.transform.GetChild(0).CompareTag("Enemy"))
                    enemyType[i] = SimpleEnemyType.COMMON;
                else
                    enemyType[i] = SimpleEnemyType.BOSS;
            }
            else
            {
                if (enemies[i].gameObject.CompareTag("Enemy"))
                    enemyType[i] = SimpleEnemyType.COMMON;
                else
                    enemyType[i] = SimpleEnemyType.BOSS;

                isGroup[i] = false;
            }
        }
    }
    void Update()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (isOpen[i])
                continue;

            if (isGroup[i])
            {
                if (enemies[i].transform.childCount == 0)
                {
                    isOpen[i] = true;
                    this.Open(doors[i]);
                }
            }
            else 
            {
                if (enemies[i] == null)
                {
                    isOpen[i] = true;
                    this.Open(doors[i]);

                    if (enemyType[i] == SimpleEnemyType.BOSS)
                        potal.gameObject.SetActive(true);
                }
            }            
        }
    }

    void Open(GameObject door)
    {
        if (door != null)
        {
            int length = door.transform.childCount;

            if (length > 0)
                for (int j = 0; j < length; j++)
                    StartCoroutine(door.transform.GetChild(j).GetComponent<Dissolve>().Act(door.transform.GetChild(j).gameObject));
            else
                StartCoroutine(door.GetComponent<Dissolve>().Act(door));
        }
    }


}
