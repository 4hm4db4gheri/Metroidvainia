using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    private Rigidbody2D rb;
    private int jumpCount;
    private bool isOnGround;
    private bool isRight = true;
    private bool isHittable = true;
    private bool isWallSliding;
    private float horizontalInput;
    private int maxHealth;

    [Header("Sword")]
    [SerializeField] private Animator swordAnimator;
    [SerializeField] private float attackDetectionRange;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask enemyMask;


    [Header("Movement")]
    [SerializeField] private Animator animator;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
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

    [Header("WallJump")]
    [SerializeField] float wallJumpDuration;
    [SerializeField] Vector2 wallJumpForce;
    private bool isWallJumping;
    private float wallJumpTime;
    private float wallJumpDirection;

    [Header("Respawn")]
    [SerializeField] Enemy[] enemies;




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxHealth = health;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        CheckHeightDeath();
        GroundDetection();
        if (!isWallJumping)
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
        WallJump();

    }


    private void WallJump()
    {
        if (isWallSliding)
        {
            wallJumpTime = wallJumpDuration;
            wallJumpDirection = -transform.localScale.x;
        }


        if (Input.GetKeyDown(jumpKey) && isWallSliding && !isOnGround)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpForce.x, wallJumpForce.y);
        }


        if (isWallJumping)
        {
            if (wallJumpTime > 0)
            {
                wallJumpTime -= Time.deltaTime;
                if (wallJumpTime <= 0)
                {
                    isWallJumping = false;
                }
            }
        }
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
            rb.velocity = new Vector2(rb.velocity.x, Math.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
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
        if (hit)
        {
            hit.gameObject.TryGetComponent<Enemy>(out var enemy);
            enemy.GetHit(damage);
        }
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
        RespawnEntities();

        RespawnSystem.instance.Respawn(this);
    }

    private void RespawnEntities()
    {
        //Setting player health
        health = maxHealth;

        //Setting enemies health
        foreach (var enemy in enemies)
        {
            if (!enemy.gameObject.activeInHierarchy)
                enemy.gameObject.SetActive(true);
            enemy.Health = enemy.MaxHeath;
            enemy.Repaint();
        }
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
        var velocity = rb.velocity;
        velocity.x = horizontalInput * movementSpeed;
        rb.velocity = velocity;
    }

    private void Jump()
    {
        if (jumpCount >= maxJumpCount)
            return;
        if (!isWallSliding)
        {
            var velocity = rb.velocity;
            velocity.y = jumpForce;
            rb.velocity = velocity;
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