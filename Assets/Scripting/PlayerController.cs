using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpSpeed = 8f;
    public float fallThreshold = -10f;

    private float direction;
    private Rigidbody2D mc;
    private SpriteRenderer spriteRenderer;
    private Animator playerAnimation;

    private Vector3 respawnPoint;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool isTouchingGround;
    private MovingPlatform currentPlatform;
    private bool isStandingOnMovingPlatform;

    [Header("Combat Settings")]
    public GameObject laptopPrefab;
    public Transform throwPoint;
    public bool canMove = true;

    private void Awake()
    {
        mc = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimation = GetComponent<Animator>();
    }

    private void Start()
    {
        respawnPoint = transform.position;
    }

    private void Update()
    {
        if (mc == null)
            return;

        // Baca input gerakan.
        direction = canMove
            ? Input.GetAxisRaw("Horizontal")
            : 0f;

        // Deteksi tanah.
        bool groundDetected = false;

        if (groundCheck != null)
        {
            groundDetected = Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundLayer
            );
        }

        isTouchingGround =
            groundDetected || isStandingOnMovingPlatform;

        // Lompat.
        if (canMove &&
            Input.GetButtonDown("Jump") &&
            isTouchingGround)
        {
            mc.velocity = new Vector2(
                mc.velocity.x,
                jumpSpeed
            );

            // Lepaskan platform saat mulai melompat.
            currentPlatform = null;
            isStandingOnMovingPlatform = false;
            isTouchingGround = false;
        }

        // Balik sprite sesuai arah gerakan.
        if (spriteRenderer != null)
        {
            if (direction < 0f)
                spriteRenderer.flipX = true;
            else if (direction > 0f)
                spriteRenderer.flipX = false;
        }

        // Respawn jika jatuh keluar map.
        if (transform.position.y < fallThreshold)
        {
            Respawn();
            return;
        }

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (mc == null)
            return;

        // Gerakan horizontal karakter.
        float horizontalVelocity = canMove
            ? direction * speed
            : 0f;

        mc.velocity = new Vector2(
            horizontalVelocity,
            mc.velocity.y
        );
    }

    private void LateUpdate()
    {
        if (mc == null)
            return;

        // Terapkan perpindahan platform setelah animasi platform
        // memperbarui Transform-nya.
        if (currentPlatform != null &&
            isStandingOnMovingPlatform &&
            isTouchingGround)
        {
            Vector3 delta = currentPlatform.PlatformDelta;

            mc.position += new Vector2(
                delta.x,
                delta.y
            );
        }
    }

    private void UpdateAnimation()
    {
        if (playerAnimation == null || mc == null)
            return;

        playerAnimation.SetFloat(
            "Speed",
            Mathf.Abs(mc.velocity.x)
        );

        playerAnimation.SetBool(
            "OnGround",
            isTouchingGround
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckMovingPlatform(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckMovingPlatform(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        MovingPlatform platform =
            collision.gameObject.GetComponent<MovingPlatform>();

        if (platform != null && currentPlatform == platform)
        {
            currentPlatform = null;
            isStandingOnMovingPlatform = false;
        }
    }

    private void CheckMovingPlatform(Collision2D collision)
    {
        MovingPlatform platform =
            collision.gameObject.GetComponent<MovingPlatform>();

        if (platform == null)
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Pastikan MC menyentuh permukaan atas platform.
            if (contact.normal.y > 0.5f)
            {
                currentPlatform = platform;
                isStandingOnMovingPlatform = true;
                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lava"))
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        currentPlatform = null;
        isStandingOnMovingPlatform = false;
        isTouchingGround = false;

        if (mc != null)
        {
            mc.velocity = Vector2.zero;
        }

        transform.position = respawnPoint;
    }

    public void ThrowLaptop()
    {
        if (laptopPrefab != null && throwPoint != null)
        {
            Instantiate(
                laptopPrefab,
                throwPoint.position,
                Quaternion.identity
            );
        }
        else
        {
            Debug.LogWarning(
                "Laptop Prefab atau Throw Point belum diatur!"
            );
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }
}