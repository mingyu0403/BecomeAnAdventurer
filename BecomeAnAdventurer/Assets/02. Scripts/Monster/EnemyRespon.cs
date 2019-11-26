using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespon : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public GameObject SpecialEnemyPrefab;
    public Transform[] ResponPosition;

    public bool isSpecial = false;
    
    // 생성
    private void Start()
    {
        for (int i = 0; i < ResponPosition.Length; i++)
        {
            CreateObject(ResponPosition[i].position, isSpecial);
            CreateObject(ResponPosition[i].position + Vector3.right * Random.Range(2f, 4f) + Vector3.forward * Random.Range(2f, 4f), false);
            CreateObject(ResponPosition[i].position + Vector3.left * Random.Range(2f, 4f) + Vector3.back * Random.Range(2f, 4f), false);
        }
    }

    private void CreateObject(Vector3 position, bool isSpecial)
    {
        if (isSpecial)
        {
            GameObject enemy = (GameObject)Instantiate(SpecialEnemyPrefab, this.transform);
            enemy.transform.position = position;
            enemy.GetComponent<MonsterCtrl>().OringinPos = position;
            enemy.SetActive(true);
        }
        else
        {
            GameObject enemy = (GameObject)Instantiate(EnemyPrefab, this.transform);
            enemy.transform.position = position;
            enemy.GetComponent<MonsterCtrl>().OringinPos = position;
            enemy.SetActive(true);
        }
    }
}
