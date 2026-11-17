using UnityEngine;
using TMPro;
using System.Collections;

public class LevelIntroUI : MonoBehaviour
{
    [Header("Referensi UI")]
    [SerializeField] private GameObject introPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    [Header("Pengaturan Level")]
    [SerializeField] private string levelTitle = "LEVEL 2";

    [TextArea(2, 4)]
    [SerializeField]
    private string levelDescription =
        "Temukan flashdisk berikutnya untuk melanjutkan perjalananmu.";

    [SerializeField] private float displayDuration = 4f;

    private IEnumerator Start()
    {
        if (introPanel == null ||
            titleText == null ||
            descriptionText == null)
        {
            Debug.LogError(
                "LevelIntroUI: Referensi UI belum lengkap!",
                this
            );

            yield break;
        }

        titleText.text = levelTitle;
        descriptionText.text = levelDescription;

        introPanel.SetActive(true);

        yield return new WaitForSeconds(displayDuration);

        introPanel.SetActive(false);
    }
}