using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtons : MonoBehaviour
{
    [Header("Main Menu Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    // Memulai ulang level yang sedang dimainkan
    public void RetryLevel()
    {
        Time.timeScale = 1f;

        string currentScene =
            SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(currentScene);
    }

    // Kembali ke menu utama
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuSceneName);
    }
}