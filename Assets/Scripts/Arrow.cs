using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour 
{
    ParticleSystem particle;
    ParticleSystem.MainModule main;
    EnemySpawner enemySpawner;
    public readonly int damage = 1;
    [SerializeField] float range = 30;
    Queue<GameObject> targets = new Queue<GameObject>();
    List<GameObject> enemies = new List<GameObject>();


    void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }
    void Start()
    {
        enemySpawner.EnemySpawned += OnEnemySpawned;
        main = particle.main;
    }

    void OnDestroy()
    {
        enemySpawner.EnemySpawned -= OnEnemySpawned;
    }

    void OnEnemySpawned(GameObject enemy)
    {
        if(enemies.Contains(enemy)==false)
        enemies.Add(enemy);
    }

    void Update()
    {
        SortTargetQueue();
        AimFirstTarget();

    }

    void SortTargetQueue()
    {
        if (enemies.Count == 0 || enemies == null) return;
        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeSelf==false || enemy == null)
            {
                enemies.Remove(enemy);
                return;
            }
            var enemyRange = Vector3.Distance(transform.parent.position, enemy.transform.position);
            if (!targets.Contains(enemy) && enemyRange <= range)
            {
                targets.Enqueue(enemy);

            }
            else if (targets.Contains(enemy) && enemyRange > range)
            {
                targets.Dequeue();

            }
        }
    }

    void AimFirstTarget()
    {
        if (targets.Count > 0)
        {
            GameObject firstTarget = targets.Peek();
            if ( firstTarget.activeSelf == false ||  firstTarget.GetComponent<Enemy>().isAlive== false)
            {
                targets.Dequeue();
                return;
            }
            var mark = firstTarget.gameObject.transform.GetChild(10);
            transform.forward = mark.transform.position - transform.parent.position;
            if (particle.isStopped)
            {
                particle.Play();
            }
            main.startSpeed = Vector3.Distance(transform.parent.position, firstTarget.transform.position) * 3.5f;
        }
        else
        {
            particle.Stop();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
