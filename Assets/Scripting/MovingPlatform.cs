using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Movement Settings")]
    [SerializeField] private Vector2 moveOffset = new Vector2(3f, 0f);
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waitAtPoint = 0.5f;

    // Dibaca oleh PlayerController untuk membawa pemain bersama platform.
    public Vector3 PlatformDelta { get; private set; }

    private Vector3 positionA;
    private Vector3 positionB;
    private Vector3 targetPosition;
    private float waitTimer;
    private bool movingToB = true;

    private void Start()
    {
        positionA = pointA != null ? pointA.position : transform.position;
        positionB = pointB != null
            ? pointB.position
            : positionA + (Vector3)moveOffset;

        transform.position = positionA;
        targetPosition = positionB;
        PlatformDelta = Vector3.zero;
    }

    private void Update()
    {
        Vector3 previousPosition = transform.position;

        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPosition) <= 0.001f)
            {
                transform.position = targetPosition;
                waitTimer = waitAtPoint;

                movingToB = !movingToB;
                targetPosition = movingToB ? positionB : positionA;
            }
        }

        // Selisih posisi per frame. PlayerController membaca nilai ini
        // pada LateUpdate agar pemain ikut bergerak bersama platform.
        PlatformDelta = transform.position - previousPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 a = pointA != null ? pointA.position : transform.position;
        Vector3 b = pointB != null
            ? pointB.position
            : a + (Vector3)moveOffset;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(a, b);
        Gizmos.DrawWireSphere(a, 0.12f);
        Gizmos.DrawWireSphere(b, 0.12f);
    }
}
