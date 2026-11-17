
using UnityEngine;

public class WaterBottlePickup : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pickupPrompt;

    [Header("Checkpoint")]
    [SerializeField] private PlayerRespawnSystem respawnSystem;

    private Collider2D nearbyBottle;
    private bool bottleCollected = false;

    private void Start()
    {
        if (pickupPrompt != null)
        {
            pickupPrompt.SetActive(false);
        }
        else
        {
            Debug.LogError("Pickup Prompt belum dihubungkan!", this);
        }

        if (respawnSystem == null)
        {
            Debug.LogError("Respawn System belum dihubungkan!", this);
        }
    }

    private void Update()
    {
        if (nearbyBottle == null || bottleCollected)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("TOMBOL F DITEKAN!");
            CollectBottle();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger terdeteksi: " + collision.name
            + " | Tag: " + collision.tag);

        if (collision.CompareTag("WaterBottle") && !bottleCollected)
        {
            nearbyBottle = collision;

            if (pickupPrompt != null)
            {
                pickupPrompt.SetActive(true);
                Debug.Log("UI PICKUP AKTIF!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (nearbyBottle == collision)
        {
            nearbyBottle = null;

            if (pickupPrompt != null)
                pickupPrompt.SetActive(false);
        }
    }

    private void CollectBottle()
    {
        if (nearbyBottle == null || respawnSystem == null)
            return;

        Vector3 bottlePosition = nearbyBottle.transform.position;

        respawnSystem.SetCheckpointPosition(bottlePosition);

        if (pickupPrompt != null)
            pickupPrompt.SetActive(false);

        Destroy(nearbyBottle.gameObject);

        nearbyBottle = null;
        bottleCollected = true;

        Debug.Log("WATER BOTTLE BERHASIL DIAMBIL!");
    }
}
