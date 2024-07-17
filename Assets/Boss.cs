using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] private float length, width;
    [SerializeField] LayerMask characterLayer;
    [SerializeField] int damage;
    [SerializeField] int maxHealth;

    bool isLeft;
    Vector2 top_right_boss;
    Vector2 top_left_boss;
    Vector2 bottom_right_boss;
    Vector2 bottom_left_boss;
    int Health;

    private void Start()
    {


        Health = maxHealth;
    }


    public void lookAtPlayer()
    {
        if (playerPos.position.x < transform.position.x && isLeft)
        {
            Flip();
        }
        else if (playerPos.position.x > transform.position.x && !isLeft)
        {
            Flip();
        }
    }
    private void Flip()
    {
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        isLeft = !isLeft;
    }

    private void Update()
    {
        var hit = Physics2D.OverlapArea(top_right_boss, bottom_left_boss, characterLayer);
        if (hit)
        {
            hit.gameObject.GetComponent<Character>().GetHit(damage);
        }
    }
    private void OnDrawGizmos()
    {
        bottom_right_boss = new Vector2(transform.position.x + length / 2, transform.position.y - width / 2);
        top_right_boss = new Vector2(transform.position.x + length / 2, transform.position.y + width / 2);
        bottom_left_boss = new Vector2(transform.position.x - length / 2, transform.position.y - width / 2);
        top_left_boss = new Vector2(transform.position.x - length / 2, transform.position.y + width / 2);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(bottom_right_boss, top_right_boss);
        Gizmos.DrawLine(bottom_left_boss, top_left_boss);
        Gizmos.DrawLine(top_left_boss, top_right_boss);
        Gizmos.DrawLine(bottom_left_boss, bottom_right_boss);

    }
    public void GetHit(int damage)
    {
        if (Health <= 0)
            return;

        Health -= damage;

        if (Health <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(this);
    }
}
