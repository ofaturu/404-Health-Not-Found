
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;

    [Header("HUD References")]
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TMP_Text healthText;

    // Memberi tahu sistem lain ketika HP berubah atau habis.
    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => currentHealth <= 0f;

    private void Awake()
    {
        maxHealth = Mathf.Max(1f, maxHealth);
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0f || IsDead)
            return;

        currentHealth = Mathf.Max(0f, currentHealth - damage);

        UpdateHealthUI();

        Debug.Log("Milo menerima damage: " + damage
            + " | HP: " + currentHealth + "/" + maxHealth);

        if (IsDead)
        {
            Debug.Log("MILO KEHABISAN HP!");
            OnDeath?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (amount <= 0f || IsDead)
            return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);

        UpdateHealthUI();

        Debug.Log("Milo memulihkan HP: " + amount
            + " | HP: " + currentHealth + "/" + maxHealth);
    }

    private void UpdateHealthUI()
    {
        float healthPercentage = currentHealth / maxHealth;

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = healthPercentage;
        }

        if (healthText != null)
        {
            healthText.text =
                Mathf.CeilToInt(currentHealth) + " / " +
                Mathf.CeilToInt(maxHealth) + " HP";
        }

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
