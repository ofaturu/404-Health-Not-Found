using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private Animator animator;

    [SerializeField] private string attackTrigger = "Attack";

    [SerializeField] private float attackCooldown = 0.5f;

    private float nextAttackTime = 0f;

    private void Awake()
    {
        // Cari Animator pada karakter jika belum diisi.
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Periksa apakah Animator tersedia.
        if (animator == null)
        {
            Debug.LogError(
                "PlayerAttack: Animator tidak ditemukan pada karakter!",
                this
            );
        }
    }

    private void Update()
    {
        // Klik kiri mouse untuk menyerang.
        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        // Hentikan jika Animator tidak ditemukan.
        if (animator == null)
            return;

        // Batasi frekuensi serangan.
        if (Time.time < nextAttackTime)
            return;

        // Aktifkan cooldown.
        nextAttackTime = Time.time + attackCooldown;

        // Jalankan animasi serangan.
        animator.SetTrigger(attackTrigger);
    }
}