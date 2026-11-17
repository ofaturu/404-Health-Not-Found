using TMPro;
using UnityEngine;

public class KeyCounterUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TMP_Text keyCountText;

    [Header("Key Settings")]
    [SerializeField] private int targetKeys = 3;

    private int lastDisplayedCount = -1;

    private void Start()
    {
        UpdateKeyDisplay();
    }

    private void Update()
    {
        UpdateKeyDisplay();
    }

    private void UpdateKeyDisplay()
    {
        if (keyCountText == null)
            return;

        if (KeyManager.Instance == null)
        {
            keyCountText.text = "KEY 0 / " + targetKeys;
            return;
        }

        int currentKeys = KeyManager.Instance.TotalKeys;
        currentKeys = Mathf.Clamp(currentKeys, 0, targetKeys);

        if (currentKeys == lastDisplayedCount)
            return;

        lastDisplayedCount = currentKeys;

        keyCountText.text = "KEY " + currentKeys + " / " + targetKeys;
    }
}