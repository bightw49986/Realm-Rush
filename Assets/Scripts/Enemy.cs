using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Enemy : MonoBehaviour
{
    PathCreator pathCreator;

    [Header("Properties")]
    [SerializeField] int MaxHealth = 3;
    [SerializeField] int Health;
    [SerializeField] public int Damage = 1;
    [SerializeField] float MovementSpeed = 10f;
    [SerializeField] public bool isAlive = true;

    void Awake()
    {
        pathCreator = FindObjectOfType<PathCreator>();
    }

    void Start()
    {
        isAlive = true;
        Health = MaxHealth;
        var path = pathCreator.GetPath();
        StartCoroutine(FollowPath(path));
    }

    void OnParticleCollision(GameObject other)
    {
        var HitType = other.GetComponent<Arrow>();
        TakeDamage(HitType.damage);
    }

    public event Action<Enemy> EnemyPassed;
    public event Action<Enemy> EnemyDied;

    protected virtual void OnEnemyPassed(Enemy enemy)
    {
        if (EnemyPassed != null)
            EnemyPassed(enemy);
        print(gameObject + " passed goal.");
    }
    protected virtual void OnEnemyDied(Enemy enemy)
    {
        if (EnemyDied != null)
            EnemyDied(enemy);
        print(gameObject + " is dead.");
    }

    public void TakeDamage(int damage)
    {
        if (isAlive)
        {
            Health -= damage;
            if(Health <= 0)
            {
                Die();
            }
        }
    }

    public void SetSpeed(float newSpeed)
    {
        MovementSpeed = newSpeed;
    }

    void Die()
    {
        isAlive = false;
        OnEnemyDied(this);
        Destroy(this);
        //todo deathFX
    }

    IEnumerator FollowPath(List<MyGrid> path)
    {   
        float startTime;
        float distanceToNext;
        Vector3 startPos;
        Vector3 nextPos;

        if (Time.timeScale < Mathf.Epsilon) yield return null;
        for (var i = 0; i < path.Count; i++)
        {
            if (path[i] != pathCreator.EndPoint)
            {
                transform.LookAt(path[i + 1].transform);
                startTime = Time.time;
                startPos = path[i].transform.position;
                transform.position = startPos;
                nextPos = path[i + 1].transform.position;
                distanceToNext = Vector3.Distance(startPos, nextPos);
                while (true)
                {
                    float distanceMoved = (Time.time - startTime) * MovementSpeed;
                    float partOfJourney = distanceMoved / distanceToNext;
                    transform.position = Vector3.Lerp(startPos, nextPos, partOfJourney);
                    if (transform.position == nextPos)
                    {
                        break;
                    }
                    yield return null;
                }

            }
        }
        OnEnemyPassed(this);
        Destroy(this);
    }

}
