using UnityEngine;
using UnityEngine.SceneManagement;

public class Script : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private int jumpCount;
    private bool isOnGround;
    private bool isRight = true;

    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float jumpForce;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private float movementSpeed;
    [SerializeField] private int maxJumpCount;
    [SerializeField] private float groundDetectionRange;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject shape;
    [SerializeField] private float deathHeight;
    [SerializeField] private int damage;
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
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
            Shoot(damage);
        }
    }

    private void Shoot(int damage)
    {
        var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.Shoot(isRight ? 1 : -1, damage);
    }

    private void CheckHeightDeath()
    {
        if (transform.position.y < deathHeight)
            Die();
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
        var scale = shape.transform.localScale;
        scale.x *= -1;
        shape.transform.localScale = scale;
        isRight = !isRight;
    }

    private void MoveLogic(float horizontalInput)
    {
        var velocity = _rigidbody2D.velocity;
        velocity.x = horizontalInput * movementSpeed;
        _rigidbody2D.velocity = velocity;
    }

    private void Jump()
    {
        if (jumpCount >= maxJumpCount)
            return;
        var velocity = _rigidbody2D.velocity;
        velocity.y = jumpForce;
        _rigidbody2D.velocity = velocity;
        jumpCount++;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position,transform.position + Vector3.down * groundDetectionRange);
    }

    private void GroundDetection()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, groundDetectionRange,groundMask);
        if (!isOnGround && hit)
        {
            jumpCount = 0;
        }

        isOnGround = hit;
    }
}
