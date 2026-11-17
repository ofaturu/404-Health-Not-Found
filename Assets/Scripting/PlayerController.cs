
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // ==========================================
    // MOVEMENT SETTINGS
    // ==========================================

    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpSpeed = 8f;
    public float fallThreshold = -10f;

    private float direction;

    private Rigidbody2D mc;
    private SpriteRenderer spriteRenderer;
    private Animator playerAnimation;

    private Vector3 respawnPoint;


    // ==========================================
    // GROUND CHECK
    // ==========================================

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool isTouchingGround;


    // ==========================================
    // MOVING PLATFORM
    // ==========================================

    private MovingPlatform currentPlatform;
    private bool isStandingOnMovingPlatform;


    // ==========================================
    // COMBAT SETTINGS
    // ==========================================

    [Header("Combat Settings")]
    public GameObject laptopPrefab;
    public Transform throwPoint;

    public bool canMove = true;


    // ==========================================
    // INITIALIZATION
    // ==========================================

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


    // ==========================================
    // UPDATE
    // ==========================================

    private void Update()
    {
        if (mc == null)
            return;

        // Horizontal Input
        direction = canMove
            ? Input.GetAxisRaw("Horizontal")
            : 0f;


        // Ground Detection
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


        // Jump
        if (canMove &&
            Input.GetButtonDown("Jump") &&
            isTouchingGround)
        {
            mc.velocity = new Vector2(
                mc.velocity.x,
                jumpSpeed
            );

            // Release Moving Platform
            currentPlatform = null;

            isStandingOnMovingPlatform = false;

            isTouchingGround = false;
        }


        // Character Flip
        if (spriteRenderer != null)
        {
            if (direction < 0f)
            {
                spriteRenderer.flipX = true;
            }
            else if (direction > 0f)
            {
                spriteRenderer.flipX = false;
            }
        }


        // Fall Detection
        if (transform.position.y < fallThreshold)
        {
            Respawn();
            return;
        }


        // Update Animation
        UpdateAnimation();
    }


    // ==========================================
    // FIXED UPDATE
    // ==========================================

    private void FixedUpdate()
    {
        if (mc == null)
            return;

        float horizontalVelocity = canMove
            ? direction * speed
            : 0f;

        mc.velocity = new Vector2(
            horizontalVelocity,
            mc.velocity.y
        );
    }


    // ==========================================
    // MOVING PLATFORM FOLLOW
    // ==========================================

    private void LateUpdate()
    {
        if (mc == null)
            return;

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


    // ==========================================
    // ANIMATION
    // ==========================================

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


    // ==========================================
    // COLLISION ENTER
    // ==========================================

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckMovingPlatform(collision);
    }


    // ==========================================
    // COLLISION STAY
    // ==========================================

    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckMovingPlatform(collision);
    }


    // ==========================================
    // COLLISION EXIT
    // ==========================================

    private void OnCollisionExit2D(Collision2D collision)
    {
        MovingPlatform platform =
            collision.gameObject.GetComponentInParent<MovingPlatform>();

        if (platform != null && currentPlatform == platform)
        {
            currentPlatform = null;

            isStandingOnMovingPlatform = false;
        }
    }


    // ==========================================
    // CHECK MOVING PLATFORM
    // ==========================================

    private void CheckMovingPlatform(Collision2D collision)
    {
        MovingPlatform platform =
            collision.gameObject.GetComponentInParent<MovingPlatform>();

        if (platform == null)
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Detect contact from the upper surface.
            if (contact.normal.y > 0.5f)
            {
                currentPlatform = platform;

                isStandingOnMovingPlatform = true;

                return;
            }
        }
    }


    // ==========================================
    // TRIGGER DETECTION
    // ==========================================

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lava"))
        {
            Respawn();
        }
    }


    // ==========================================
    // RESPAWN
    // ==========================================

    public void Respawn()
    {
        currentPlatform = null;

        isStandingOnMovingPlatform = false;

        isTouchingGround = false;

        direction = 0f;

        if (mc != null)
        {
            mc.velocity = Vector2.zero;

            mc.angularVelocity = 0f;

            mc.position = respawnPoint;
        }
        else
        {
            transform.position = respawnPoint;
        }

        Debug.Log("Player Respawned!");
    }


    // ==========================================
    // THROW LAPTOP
    // ==========================================

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


    // ==========================================
    // GROUND CHECK GIZMOS
    // ==========================================

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
