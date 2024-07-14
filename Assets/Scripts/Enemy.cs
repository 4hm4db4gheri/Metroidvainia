using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using DG.Tweening;
using UnityEditor.ShaderGraph.Internal;

public class Enemy : MonoBehaviour
{
    public int MaxHeath;

    public int Health;

    [SerializeField] private EnemyHUDController _enemyHUDController;
    [SerializeField] private Transform patrolTransform;
    [SerializeField] private LayerMask characterLayer;
    [SerializeField] private float characterDetectionRange;
    [SerializeField] private int damage;
    [SerializeField] private float patrolTime = 2f;


    private void Start()
    {
        transform.DOMove(patrolTransform.position, patrolTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        MaxHeath = Health;
        _enemyHUDController.Setup(this);
    }

    public void GetHit(int damage)
    {
        if (Health <= 0)
            return;

        Health -= damage;
        Repaint();

        if (Health <= 0)
            Die();
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {

        var hit = Physics2D.OverlapCircle(transform.position, characterDetectionRange, characterLayer);
        if (hit)
        {
            hit.gameObject.GetComponent<Character>().GetHit(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, characterDetectionRange);
    }

    public void Repaint()
    {
        _enemyHUDController.Repaint(this);
    }

}



