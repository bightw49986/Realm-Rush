using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class EnemySpawner : MonoBehaviour
{
    PathCreator pathCreator;
    ObjectPool objectPool;

    [Header("Enemy")]
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] Transform SpawnAtRunTime;
    [SerializeField] public int EnemyNumbers = 5;
    [SerializeField] float Intervals = 1f;
    [Range(10f, 50f)] [SerializeField] float MovementSpeed = 10f;

    [Header("Default player states ")]
    [SerializeField] public int HP;
    [SerializeField] public int Tower;
   
    void Awake()
    {
        objectPool = FindObjectOfType<ObjectPool>();
        pathCreator = FindObjectOfType<PathCreator>();
    }

    void Start()
    {
        pathCreator.PathGenerated += StartSpawningEnemies;
        objectPool.InstantiateGameObjectData(10, PoolingGameObjectData.PoolKey.snowman, EnemyPrefab);
    }

    void StartSpawningEnemies()
    {
        StartCoroutine("SpawnEnemies");
    }

    IEnumerator SpawnEnemies()
    {
        OnSpawningEnemies();
        if (Time.timeScale < Mathf.Epsilon) yield return null;
        for (var i = 0; i < EnemyNumbers; i++)
        {
            InstantiateEnemies();
            yield return new WaitForSeconds(Intervals);
        }
        OnSpawningFinished();
    }

    void InstantiateEnemies()
    {
        GameObject enemy =  objectPool.AccessGameObjectFromPool(PoolingGameObjectData.PoolKey.snowman, EnemyPrefab, true);
        enemy.transform.position = transform.position;
        enemy.AddComponent<Enemy>();
        enemy.GetComponent<Enemy>().SetSpeed(MovementSpeed);
        OnEnemySpawned(enemy);
    }

    public event Action SpawningEnemies;
    public event Action<GameObject> EnemySpawned;
    public event Action SpawningFinished;

    protected virtual void OnSpawningEnemies()
    {
        if (SpawningEnemies != null)
            SpawningEnemies();
    }
    protected virtual void OnEnemySpawned(GameObject enemy)
    {
        if (EnemySpawned != null)
            EnemySpawned(enemy);
        enemy.GetComponent<Enemy>().EnemyPassed += delegate 
        {
            objectPool.ReturnGameObjectToPool(enemy, PoolingGameObjectData.PoolKey.snowman);
        };
        enemy.GetComponent<Enemy>().EnemyDied += delegate 
        {
            objectPool.ReturnGameObjectToPool(enemy, PoolingGameObjectData.PoolKey.snowman);
        };
    }
    protected virtual void OnSpawningFinished()
    {
        if (SpawningFinished != null)
            SpawningFinished();
    }

    public List<int> GetDefaultPlayerData()
    {
        List<int> plist = new List<int> { HP, Tower, 0 };
        return plist;
    }
}
