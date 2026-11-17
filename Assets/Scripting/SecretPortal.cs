using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SecretPortal : MonoBehaviour
{
    [Header("Secret Level")]
    [SerializeField] private string hiddenSceneName = "HiddenRoom";
    [SerializeField] private int requiredKeys = 3;

    [Header("Interaction")]
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private TMP_Text promptText;

    private bool playerInRange;

    private void Start()
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (!playerInRange)
            return;

        int collectedKeys = 0;

        if (KeyManager.Instance != null)
        {
            collectedKeys = KeyManager.Instance.TotalKeys;
        }

        if (promptText != null)
        {
            if (collectedKeys >= requiredKeys)
            {
                promptText.text = "Tekan E untuk memasuki kamar rahasia";
            }
            else
            {
                promptText.text =
                    "Kunci terkumpul: " + collectedKeys +
                    "/" + requiredKeys;
            }
        }

        if (Input.GetKeyDown(interactionKey))
        {
            if (collectedKeys >= requiredKeys)
            {
                SceneManager.LoadScene(hiddenSceneName);
            }
            else
            {
                Debug.Log(
                    "Portal terkunci. Kumpulkan " +
                    requiredKeys + " kunci terlebih dahulu."
                );
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = true;

        if (interactionUI != null)
        {
            interactionUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = false;

        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }
}