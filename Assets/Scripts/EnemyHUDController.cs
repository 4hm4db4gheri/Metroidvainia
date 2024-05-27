using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHUDController : MonoBehaviour
{
    [SerializeField] private Slider healthBar;

    public void Setup(Enemy enemy)
    {
        healthBar.minValue = 0;
        healthBar.maxValue = enemy.MaxHeath;
        healthBar.value = enemy.Health;
    }

    public void Repaint(Enemy enemy)
    {
        healthBar.value = enemy.Health;
    }
}
