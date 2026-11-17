
using System.Collections;
using UnityEngine;

public class PlayerRespawnSystem : MonoBehaviour
{
    [Header("Respawn Settings")]
    [SerializeField] private Transform startRespawnPoint;
    [SerializeField] private float respawnOffsetY = 0.8f;

    [Header("Blink Effect")]
    [SerializeField] private int blinkCount = 2;
    [SerializeField] private float blinkDuration = 0.15f;

    private Rigidbody2D rb;
    private SpriteRenderer[] spriteRenderers;

    private Vector3 currentRespawnPosition;
    private bool checkpointActivated = false;
    private bool isRespawning = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Mengambil Sprite Renderer Milo, termasuk child object.
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
    }

    private void Start()
    {
        if (startRespawnPoint == null)
        {
            Debug.LogError("StartRespawnPoint belum dihubungkan!", this);
            return;
        }

        // Menyimpan posisi awal tanpa memindahkan Milo.
        currentRespawnPosition = startRespawnPoint.position;
        checkpointActivated = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isRespawning) return;

        // Hanya mendeteksi FallDetector.
        // WaterBottle ditangani oleh WaterBottlePickup.
        if (collision.CompareTag("FallDetector"))
        {
            RespawnPlayer();
        }
    }

    public void SetCheckpointPosition(Vector3 position)
    {
        currentRespawnPosition = position;
        checkpointActivated = true;

        Debug.Log("CHECKPOINT 1 AKTIF!");
    }

    public void RespawnPlayer()
    {
        if (isRespawning) return;

        if (startRespawnPoint == null && !checkpointActivated)
        {
            Debug.LogError("Respawn point belum tersedia!", this);
            return;
        }

        isRespawning = true;

        Vector3 targetPosition =
            currentRespawnPosition + Vector3.up * respawnOffsetY;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.position = targetPosition;
        }
        else
        {
            transform.position = targetPosition;
        }

        StartCoroutine(BlinkEffect());

        Debug.Log(checkpointActivated
            ? "Milo respawn ke Checkpoint 1."
            : "Milo respawn ke posisi awal.");
    }

    private IEnumerator BlinkEffect()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            SetSpriteVisibility(false);
            yield return new WaitForSeconds(blinkDuration);

            SetSpriteVisibility(true);
            yield return new WaitForSeconds(blinkDuration);
        }

        SetSpriteVisibility(true);
        isRespawning = false;
    }

    private void SetSpriteVisibility(bool visible)
    {
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr != null)
            {
                sr.enabled = visible;
            }
        }
    }
}
