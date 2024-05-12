using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int Health;
    [SerializeField] private bool canPatrol;
    [SerializeField] private List<PatrolMovement> patrolPositions;

    private List<Vector3> patrolPositionCopy;
    private int patrolPosIndex;
    private int _maxHealth;

    private float time;

    private void Start()
    {
        _maxHealth = Health;

        patrolPositionCopy = new List<Vector3>();
        foreach (var t in patrolPositions)   
        {
            patrolPositionCopy.Add(new Vector3(t.patrolPosition.position.x,t.patrolPosition.position.y,
                t.patrolPosition.position.z));
        }
    }

    public void GetHit(int damage)
    {
        if(Health<=0)
            return;

        Health -= damage;

        if (Health <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (canPatrol)
        {
            MoveToPosition(patrolPositionCopy[patrolPosIndex],patrolPositions[patrolPosIndex].duration);
        }
    }

    private void MoveToPosition(Vector3 pos, float duration)
    {
        var t = duration;
        var prevIndex = patrolPosIndex - 1;
        if (patrolPosIndex < 1)
        {
            prevIndex = patrolPositionCopy.Count - 1;
        }

        var newPos = Vector3.Lerp(patrolPositionCopy[prevIndex], pos, time / t);
        transform.position = newPos;
        CheckPatrolPositionReached();
    }

    private void CheckPatrolPositionReached()
    {
        if (Vector3.Distance(patrolPositionCopy[patrolPosIndex], transform.position) <= 0.1f)
        {
            patrolPosIndex++;
            if (patrolPosIndex >= patrolPositionCopy.Count)
            {
                patrolPosIndex = 0;
            }

            time = 0;
        }
    }
}
[System.Serializable]
class PatrolMovement
{
    public Transform patrolPosition;
    public float duration;
}



