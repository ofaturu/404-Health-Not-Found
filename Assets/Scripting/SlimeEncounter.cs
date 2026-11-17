
using System.Collections;
using UnityEngine;

public class SlimeEncounter : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject quizPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Slime References")]
    [SerializeField] private Animator slimeAnim;
    [SerializeField] private GameObject enemyRoot;

    [Header("Encounter Settings")]
    [SerializeField] private float destroyDelay = 1.5f;
    [SerializeField] private string deadAnimationName = "enemy_dead";

    private PlayerController player;
    private bool hasTriggered;
    private bool encounterFinished;

    private void Start()
    {
        if (quizPanel != null)
            quizPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Ambil Animator dari parent jika belum dihubungkan.
        if (slimeAnim == null)
            slimeAnim = GetComponentInParent<Animator>();

        // Ambil objek Enemy sebagai parent.
        if (enemyRoot == null && transform.parent != null)
            enemyRoot = transform.parent.gameObject;

        if (slimeAnim == null)
            Debug.LogError("Animator Enemy tidak ditemukan!", this);

        if (enemyRoot == null)
            Debug.LogError("Parent Enemy tidak ditemukan!", this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered || encounterFinished)
            return;

        if (!collision.CompareTag("Player"))
            return;

        PlayerController detectedPlayer =
            collision.GetComponentInParent<PlayerController>();

        if (detectedPlayer == null)
            detectedPlayer = collision.GetComponent<PlayerController>();

        if (detectedPlayer == null)
        {
            Debug.LogWarning(
                "PlayerController tidak ditemukan pada objek Player.",
                this
            );
            return;
        }

        player = detectedPlayer;
        TriggerEncounter();
    }

    private void TriggerEncounter()
    {
        hasTriggered = true;

        if (player != null)
            player.canMove = false;

        if (quizPanel != null)
        {
            quizPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Quiz Panel belum dihubungkan!", this);

            if (player != null)
                player.canMove = true;

            hasTriggered = false;
        }
    }

    public void AnswerCorrect()
    {
        if (!hasTriggered || encounterFinished)
            return;

        encounterFinished = true;

        if (quizPanel != null)
            quizPanel.SetActive(false);

        // Jalankan animasi kematian.
        if (slimeAnim != null)
        {
            slimeAnim.Play(deadAnimationName, 0, 0f);
        }
        else
        {
            Debug.LogWarning("Animator tidak ditemukan!", this);
        }

        if (player != null)
            player.canMove = true;

        StartCoroutine(RemoveSlime());
    }

    private IEnumerator RemoveSlime()
    {
        yield return new WaitForSeconds(destroyDelay);

        // Hancurkan seluruh Enemy beserta semua child-nya.
        if (enemyRoot != null)
        {
            Destroy(enemyRoot);
        }
        else
        {
            Debug.LogError("Objek Enemy tidak ditemukan!", this);
        }
    }

    public void AnswerWrong()
    {
        if (!hasTriggered || encounterFinished)
            return;

        encounterFinished = true;

        if (quizPanel != null)
            quizPanel.SetActive(false);

        if (player != null)
            player.canMove = false;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogError(
                "Game Over Panel belum dihubungkan!",
                this
            );
        }
    }
}
