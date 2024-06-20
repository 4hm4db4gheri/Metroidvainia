using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private int jumpCount;
    private bool isOnGround;
    private bool isRight = true;
    private bool isHittable = true;
    private bool isWallSliding;
    private float horizontalInput;

    [Header("Sword")]
    [SerializeField] private Animator swordAnimator;
    [SerializeField] private float attackDetectionRange;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask enemyMask;


    [Header("Movement")]
    [SerializeField] private Animator animator;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private float jumpForce;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float groundDetectionRange;
    [SerializeField] private int maxJumpCount;
    [SerializeField] private LayerMask groundMask;

    [Header("Player")]
    [SerializeField] private float deathHeight;
    [SerializeField] private GameObject shape;
    [SerializeField] private int health;
    [SerializeField] private Projectile projectilePrefab;

    [Header("WallSlide")]
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private float wallDetectionRange;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallMask;


    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        CheckHeightDeath();
        GroundDetection();
        MoveLogic(horizontalInput);
        FlipCheck(horizontalInput);
        if (Input.GetKeyDown(jumpKey))
        {
            Jump();
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Shoot(damage);
            Attack(damage);
        }

        WallSlide();

    }



    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, wallDetectionRange, wallMask);
    }

    private void WallSlide()
    {
        if (isWalled() && !isOnGround && horizontalInput != 0)
        {
            isWallSliding = true;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, Math.Clamp(_rigidbody2D.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void Attack(int damage)
    {
        swordAnimator.SetTrigger("Attack");
        var hit = Physics2D.OverlapCircle(swordAnimator.transform.position, attackDetectionRange, enemyMask);
        hit.gameObject.TryGetComponent<Enemy>(out var enemy);
        enemy.GetHit(damage);
    }


    private void CheckHeightDeath()
    {
        if (transform.position.y < deathHeight)
            Die();
    }

    public void GetHit(int Damage)
    {
        if (!isHittable)
            return;

        if (health <= 0)
            return;

        health -= Damage;

        if (health <= 0)
            Die();
        else
            StartCoroutine(StartInvulnerability());
        HUDController.instance.Repaint(health);
    }

    private IEnumerator StartInvulnerability()
    {
        isHittable = false;
        for (int i = 0; i < 3; i++)
        {
            shape.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            shape.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
        isHittable = true;
    }

    private void Die()
    {
        SceneManager.LoadScene(0);
    }

    private void FlipCheck(float input)
    {
        if (input < 0 && isRight)
            Flip();
        else if (input > 0 && !isRight)
            Flip();
    }

    private void Flip()
    {
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        isRight = !isRight;
    }

    private void MoveLogic(float horizontalInput)
    {
        animator.SetBool("IsMoving", horizontalInput != 0);
        var velocity = _rigidbody2D.velocity;
        velocity.x = horizontalInput * movementSpeed;
        _rigidbody2D.velocity = velocity;
    }

    private void Jump()
    {
        if (jumpCount >= maxJumpCount)
            return;
        if (!isWallSliding)
        {
            var velocity = _rigidbody2D.velocity;
            velocity.y = jumpForce;
            _rigidbody2D.velocity = velocity;
            jumpCount++;
        } 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundDetectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(swordAnimator.transform.position, attackDetectionRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheck.position, wallDetectionRange);
    }

    private void GroundDetection()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, groundDetectionRange, groundMask);
        if (!isOnGround && hit)
        {
            jumpCount = 0;
        }

        isOnGround = hit;
    }
}

    // private void Shoot(int damage)
    // {
    //     var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    //     Vector2 projectileAngle = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    //     projectile.Shoot(projectileAngle.normalized, damage);

    // }