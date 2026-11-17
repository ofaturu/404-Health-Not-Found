using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HiddenRoomEnding : MonoBehaviour
{
    [Header("Blink Effect")]
    [SerializeField] private Image blinkPanel;
    [SerializeField] private float openingDuration = 1.5f;
    [SerializeField] private float blinkDuration = 0.2f;
    [SerializeField] private int blinkCount = 2;
    [SerializeField] private float delayBetweenBlinks = 1.2f;

    [Header("Revelation UI")]
    [SerializeField] private GameObject revelationPanel;
    [SerializeField] private TMP_Text systemTitle;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private float messageDelay = 1f;

    private void Start()
    {
        if (blinkPanel != null)
        {
            SetAlpha(blinkPanel, 1f);
            blinkPanel.gameObject.SetActive(true);
        }

        if (revelationPanel != null)
        {
            revelationPanel.SetActive(false);
        }

        StartCoroutine(PlayEnding());
    }

    private IEnumerator PlayEnding()
    {
        // Mata terbuka: layar hitam perlahan menghilang.
        yield return FadeBlink(1f, 0f, openingDuration);

        // Kedipan mata.
        for (int i = 0; i < blinkCount; i++)
        {
            yield return new WaitForSeconds(delayBetweenBlinks);

            yield return FadeBlink(0f, 1f, blinkDuration);
            yield return new WaitForSeconds(0.1f);
            yield return FadeBlink(1f, 0f, blinkDuration);
        }

        // Tampilkan pengungkapan.
        yield return new WaitForSeconds(messageDelay);

        if (revelationPanel != null)
        {
            revelationPanel.SetActive(true);
        }

        if (systemTitle != null)
        {
            systemTitle.text = "SELAMAT DATANG KEMBALI";
        }

        if (messageText != null)
        {
            messageText.text =
                "Hal yang kamu lakukan sebelumnya " +
                "hanya berupa simulasi bukan hal yang nyata! " +
                "tapi selamat telah berhasil kembali.....";
        }
    }

    private IEnumerator FadeBlink(
        float startAlpha,
        float targetAlpha,
        float duration)
    {
        if (blinkPanel == null)
            yield break;

        blinkPanel.gameObject.SetActive(true);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / duration);

            SetAlpha(
                blinkPanel,
                Mathf.Lerp(startAlpha, targetAlpha, progress)
            );

            yield return null;
        }

        SetAlpha(blinkPanel, targetAlpha);

        if (Mathf.Approximately(targetAlpha, 0f))
        {
            blinkPanel.gameObject.SetActive(false);
        }
    }

    private void SetAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}