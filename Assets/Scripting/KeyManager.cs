using UnityEngine;
using System.Collections.Generic;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance { get; private set; }

    private HashSet<string> collectedKeys = new HashSet<string>();

    public int TotalKeys => collectedKeys.Count;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool IsCollected(string keyID)
    {
        return collectedKeys.Contains(keyID);
    }

    public bool CollectKey(string keyID)
    {
        if (string.IsNullOrWhiteSpace(keyID))
            return false;

        if (!collectedKeys.Add(keyID))
            return false;

        Debug.Log(
            "Key terkumpul: " + TotalKeys + "/3"
        );

        if (TotalKeys >= 3)
        {
            Debug.Log(
                "Semua Key terkumpul! Level bonus terbuka."
            );
        }

        return true;
    }
}