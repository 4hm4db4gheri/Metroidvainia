using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeDuration;
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private float detectionRadius;

    private float _duration;
    private bool hasShot;
    private int direction;
    private int damage;

    public void Shoot(int direction , int damage)
    {
        _duration = lifeDuration;
        hasShot = true;
        this.direction = direction;
        this.damage = damage;
    }

    private void Update()
    {
        if(!hasShot)
            return;

        Move(direction);

        var hit = Physics2D.OverlapCircle(transform.position, detectionRadius, detectionLayer);

        if (hit)
        {
            if (hit.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.GetHit(damage);
            }

            Die();
                return;
        }

        if (_duration > 0)
        {
            _duration -= Time.deltaTime;
            if (_duration <= 0)
            {
                Die();
            }
        }
    }

    private void Move(int direction)
    {
        var pos = transform.position;
        pos.x += speed * direction * Time.deltaTime;
        transform.position = pos;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
