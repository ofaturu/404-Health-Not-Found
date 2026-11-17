using UnityEngine;

public class LaptopProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float maxDistance = 8f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float rotationSpeed = 720f;

    private Rigidbody2D rb;
    private Vector2 direction;
    private Vector2 startPosition;
    private bool initialized;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 throwDirection)
    {
        direction = throwDirection.normalized;
        startPosition = transform.position;
        initialized = true;

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.velocity = direction * speed;
        }
    }

    private void Update()
    {
        if (!initialized)
            return;

        transform.Rotate(
            0f,
            0f,
            rotationSpeed * Time.deltaTime
        );

        if (Vector2.Distance(startPosition, transform.position)
            >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;

        EnemyHealth enemy =
            other.GetComponentInParent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}