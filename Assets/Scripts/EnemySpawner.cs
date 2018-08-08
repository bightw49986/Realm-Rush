using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] Transform SpawnAtRunTime;
    [SerializeField] int EnemyNumbers = 5;
    [SerializeField] float Intervals = 1f;
    [Range(10f,50f)][SerializeField] float MovementSpeed = 10f;


    public void StartSpawningEnemies()
    {
        StartCoroutine("SpawnEnemies");
    }

    IEnumerator SpawnEnemies()
    {
        for (var i = 0; i < EnemyNumbers; i++)
        {
            InstantiateEnemies();
            yield return new WaitForSeconds(Intervals);
        }
    }

    private void InstantiateEnemies()
    {
        GameObject enemy = Instantiate(EnemyPrefab, transform.position, Quaternion.identity, SpawnAtRunTime);
        enemy.GetComponent<Enemy>().SetSpeed(MovementSpeed);
    }
}

