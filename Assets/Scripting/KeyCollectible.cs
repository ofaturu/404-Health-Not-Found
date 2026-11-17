using UnityEngine;
using TMPro;

public class KeyCollectible : MonoBehaviour
{
    [Header("Identitas Key")]
    [SerializeField] private string keyID = "Key_01";

    [Header("UI Interaksi")]
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private TMP_Text promptText;

    [Header("Pengaturan")]
    [SerializeField] private KeyCode interactionKey = KeyCode.F;

    private bool playerInRange;
    private Transform player;

    private void Start()
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }

        if (KeyManager.Instance != null &&
            KeyManager.Instance.IsCollected(keyID))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!playerInRange || player == null)
            return;

        if (Input.GetKeyDown(interactionKey))
        {
            Collect();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (KeyManager.Instance != null &&
            KeyManager.Instance.IsCollected(keyID))
        {
            Destroy(gameObject);
            return;
        }

        player = other.transform;
        playerInRange = true;

        if (interactionUI != null)
        {
            interactionUI.SetActive(true);
        }

        if (promptText != null)
        {
            promptText.text = "Tekan F untuk mengambil Key";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (player == null || other.transform != player)
            return;

        playerInRange = false;
        player = null;

        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }

    private void Collect()
    {
        if (KeyManager.Instance == null)
        {
            Debug.LogError(
                "KeyManager belum tersedia!",
                this
            );
            return;
        }

        if (!KeyManager.Instance.CollectKey(keyID))
            return;

        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }

        Debug.Log(
            "Berhasil mengambil " + keyID +
            ". Total Key: " +
            KeyManager.Instance.TotalKeys + "/3"
        );

        Destroy(gameObject);
    }
}