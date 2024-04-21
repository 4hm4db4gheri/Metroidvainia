using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int Health;

    private int _maxHealth;

    private void Start()
    {
        _maxHealth = Health;
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
}
