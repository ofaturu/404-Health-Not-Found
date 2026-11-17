using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PortalInteraction : MonoBehaviour
{
    [Header("Portal Settings")]
    [SerializeField] private string nextSceneName = "Level3";
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    [Header("UI Settings")]
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private TMP_Text promptText;

    private bool playerInRange;

    private void Start()
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }

        if (promptText != null)
        {
            promptText.text = "Tekan E untuk masuk portal";
        }
    }

    private void Update()
    {
        if (!playerInRange)
            return;

        if (Input.GetKeyDown(interactionKey))
        {
            SceneManager.LoadScene(nextSceneName);
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