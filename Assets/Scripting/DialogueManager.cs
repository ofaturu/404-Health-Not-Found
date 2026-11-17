using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialogPanel;
    public TMP_Text characterNameText;
    public TMP_Text dialogueText;
    public Button choiceButton1;
    public Button choiceButton2;
    public TMP_Text choiceText1;
    public TMP_Text choiceText2;

    private void Start()
    {
        dialogPanel.SetActive(false);
    }

    public void StartDialogue()
    {
        dialogPanel.SetActive(true);

        characterNameText.text = "Dokter Maya";
        dialogueText.text =
            "Halo, Raka! Bagaimana keadaanmu hari ini?";

        choiceText1.text = "Saya merasa sehat.";
        choiceText2.text = "Saya merasa kurang sehat.";

        choiceButton1.gameObject.SetActive(true);
        choiceButton2.gameObject.SetActive(true);

        choiceButton1.onClick.RemoveAllListeners();
        choiceButton2.onClick.RemoveAllListeners();

        choiceButton1.onClick.AddListener(HealthyResponse);
        choiceButton2.onClick.AddListener(UnhealthyResponse);
    }

    private void HealthyResponse()
    {
        dialogueText.text =
            "Bagus! Tetap jaga pola hidup sehat.";

        HideChoices();
    }

    private void UnhealthyResponse()
    {
        dialogueText.text =
            "Baik, Raka. Mari kita periksa keadaanmu.";

        HideChoices();
    }

    private void HideChoices()
    {
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
    }

    public void CloseDialogue()
    {
        dialogPanel.SetActive(false);
    }
}