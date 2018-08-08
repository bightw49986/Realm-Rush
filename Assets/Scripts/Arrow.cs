using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour 
{
    ParticleSystem particle;
    ParticleSystem.MainModule main;
    Transform Tower;
    public readonly int damage = 1;
    [SerializeField] float range = 30;
    Queue<Enemy> targets;
    Enemy[] enemies;

    private void Start()
    {
        targets = new Queue<Enemy>();
        Tower = transform.parent;
        particle = GetComponent<ParticleSystem>();
        main = particle.main;
    }

    private void Update()
    {
        enemies = FindObjectsOfType<Enemy>();
        SortTargetQueue();
        AimFirstTarget();

    }

    private void SortTargetQueue()
    {
        foreach (Enemy enemy in enemies)
        {
            var enemyRange = Vector3.Distance(transform.parent.position, enemy.transform.position);
            if (!targets.Contains(enemy)&& enemyRange <= range)
            {
                targets.Enqueue(enemy);
                print("Target in range, enqueue target.");
            }
            else if (targets.Contains(enemy)&& enemyRange>range)
            {
                targets.Dequeue();
                print("Target offsight, dequeue.");
            }
        }
    }

    private void AimFirstTarget()
    {
        if (targets.Count >0)
        {
            Enemy firstTarget = targets.Peek();
            if(firstTarget.isAlive==false || firstTarget==null)
            {
                targets.Dequeue();
                print("Target down, dequeue.");
                return;
            }
            var mark = firstTarget.gameObject.transform.GetChild(10);
            transform.forward = mark.transform.position - transform.parent.position;
            if(particle.isStopped)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
