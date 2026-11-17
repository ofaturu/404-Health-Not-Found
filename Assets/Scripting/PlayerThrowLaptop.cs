using UnityEngine;

public class PlayerThrowLaptop : MonoBehaviour
{
    [Header("Laptop Settings")]
    [SerializeField] private GameObject laptopPrefab;
    [SerializeField] private Transform throwPoint;

    [Header("Throw Settings")]
    [SerializeField] private float throwCooldown = 0.8f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private float nextThrowTime;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) &&
            Time.time >= nextThrowTime)
        {
            ThrowLaptop();
        }
    }

    private void ThrowLaptop()
    {
        if (laptopPrefab == null || throwPoint == null)
        {
            Debug.LogWarning(
                "Laptop Prefab atau Throw Point belum diatur!"
            );
            return;
        }

        nextThrowTime = Time.time + throwCooldown;

        if (animator != null)
        {
            animator.SetTrigger("ThrowLaptop");
        }

        float directionX = transform.localScale.x >= 0 ? 1f : -1f;
        Vector2 throwDirection = new Vector2(directionX, 0f);

        GameObject laptop = Instantiate(
            laptopPrefab,
            throwPoint.position,
            Quaternion.identity
        );

        LaptopProjectile projectile =
            laptop.GetComponent<LaptopProjectile>();

        if (projectile != null)
        {
            projectile.Initialize(throwDirection);
        }
        else
        {
            Debug.LogError(
                "Prefab laptop tidak memiliki script LaptopProjectile!"
            );

            Destroy(laptop);
        }
    }

}