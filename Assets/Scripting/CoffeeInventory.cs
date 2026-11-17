
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoffeeInventory : MonoBehaviour
{
    [Header("Coffee Settings")]
    [SerializeField] private int startingCoffee = 1;
    [SerializeField] private int coffeeCount = 0;
    [SerializeField] private float healAmount = 20f;

    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Button coffeeButton;
    [SerializeField] private TMP_Text coffeeCountText;

    private void Start()
    {
        // Hadiah Coffee ketika permainan dimulai.
        coffeeCount = startingCoffee;

        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += HandleHealthChanged;
        }

        if (coffeeButton != null)
        {
            coffeeButton.onClick.AddListener(UseCoffee);
        }

        UpdateUI();
    }

    public void UseCoffee()
    {
        if (playerHealth == null || playerHealth.IsDead)
            return;

        if (coffeeCount <= 0)
            return;

        // Coffee tidak bisa digunakan saat HP penuh.
        if (playerHealth.CurrentHealth >= playerHealth.MaxHealth)
            return;

        playerHealth.Heal(healAmount);

        coffeeCount--;

        UpdateUI();

        Debug.Log("Coffee digunakan! Sisa Coffee: " + coffeeCount);
    }

    public void AddCoffee(int amount)
    {
        if (amount <= 0)
            return;

        coffeeCount += amount;
        UpdateUI();
    }

    private void HandleHealthChanged(float current, float max)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coffeeCountText != null)
        {
            coffeeCountText.text = "Coffee x" + coffeeCount;
        }

        if (coffeeButton != null)
        {
            bool hasCoffee = coffeeCount > 0;
            bool canHeal = playerHealth != null &&
                           !playerHealth.IsDead &&
                           playerHealth.CurrentHealth < playerHealth.MaxHealth;

            coffeeButton.interactable = hasCoffee && canHeal;
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= HandleHealthChanged;
        }

        if (coffeeButton != null)
        {
            coffeeButton.onClick.RemoveListener(UseCoffee);
        }
    }
}
