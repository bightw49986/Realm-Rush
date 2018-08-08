using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Hp")]
    [SerializeField] int MaxHealth = 3;
    [SerializeField] int Health;
    public bool isAlive = true;

    [Header("PathFollowing")]
    PathCreator pathCreator;
    float startTime;
    float distanceToNext;
    float timeCost;
    Vector3 startPos;
    Vector3 nextPos;

    [SerializeField] float MovementSpeed = 10f;

    private void Start()
    {
        isAlive = true;
        Health = MaxHealth;
        pathCreator = FindObjectOfType<PathCreator>();
        var path = pathCreator.GetPath();
        StartCoroutine(FollowPath(path));
    }

    private void OnParticleCollision(GameObject other)
    {
		var HitType = other.GetComponent<Arrow>();
		TakeDamage(HitType.damage);
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

    private void Die()
    {
        print(gameObject + "is dead.");
        isAlive = false;
        //todo deathFX
        Destroy(gameObject, 0.3f);
    }

    IEnumerator FollowPath(List<MyGrid> path)
    {
        for (var i = 0; i < path.Count; i++)
        {
            if (path[i] != pathCreator.EndPoint)
            {
                transform.LookAt(path[i + 1].transform);
                startTime = Time.time;
                //print("Moving to next waypoint..." + path[i+1]);
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
                //print("Reached a waypoint." + path[i + 1]);
            }
        }
        //print("Reached goal. Selfdestroying...");
        Destroy(gameObject, 0.3f);
    }

}
