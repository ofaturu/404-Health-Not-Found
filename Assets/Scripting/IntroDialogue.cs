using UnityEngine;
using TMPro;
using System.Collections;

public class IntroDialogue : MonoBehaviour
{
    [Header("UI Dialog")]
    public GameObject dialogueBox;
    public TMP_Text characterName;
    public TMP_Text dialogueText;

    [Header("Pengaturan Intro")]
    public string speakerName = "Developer";

    [TextArea(2, 5)]
    public string introMessage =
        "Halo, Milo! Selamat datang di dunia ini.";

    [Header("Kecepatan Teks")]
    [Tooltip("Waktu jeda sebelum setiap huruf muncul.")]
    public float typingSpeed = 0.05f;

    [Tooltip("Waktu tunggu setelah teks selesai ditampilkan.")]
    public float waitAfterFinished = 2f;

    private void Start()
    {
        StartCoroutine(ShowIntro());
    }

    private IEnumerator ShowIntro()
    {
        if (dialogueBox == null ||
            characterName == null ||
            dialogueText == null)
        {
            Debug.LogError(
                "IntroDialogue: Referensi UI belum lengkap!",
                this
            );

            yield break;
        }

        dialogueBox.SetActive(true);

        characterName.text = speakerName;
        dialogueText.text = "";

        // Menampilkan teks satu karakter demi satu karakter.
        foreach (char letter in introMessage)
        {
            dialogueText.text += letter;

            yield return new WaitForSeconds(typingSpeed);
        }

        // Menunggu setelah seluruh teks selesai ditampilkan.
        yield return new WaitForSeconds(waitAfterFinished);

        // Menyembunyikan kotak dialog secara otomatis.
        dialogueBox.SetActive(false);
    }
}