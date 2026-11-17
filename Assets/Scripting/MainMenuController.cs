
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // ==========================================
    // SCENE SETTINGS
    // ==========================================

    [Header("Scene Settings")]
    [SerializeField] private string firstLevelScene = "Level1";


    // ==========================================
    // MAIN MENU PANEL
    // ==========================================

    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private Button continueButton;


    // ==========================================
    // OTHER PANELS
    // ==========================================

    [Header("Other Panels")]
    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private GameObject settingsPanel;


    // ==========================================
    // AUDIO SETTINGS
    // ==========================================

    [Header("Audio Settings")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private AudioSource musicSource;


    // ==========================================
    // INITIALIZATION
    // ==========================================

    private void Start()
    {
        ShowMainMenu();
        LoadSettings();
        UpdateContinueButton();
    }


    // ==========================================
    // NEW GAME
    // ==========================================

    public void NewGame()
    {
        Debug.Log("Starting New Game...");

        GameProgress.ClearProgress();

        Time.timeScale = 1f;

        SceneManager.LoadScene(firstLevelScene);
    }


    // ==========================================
    // CONTINUE GAME
    // ==========================================

    public void ContinueGame()
    {
        if (!GameProgress.HasProgress())
        {
            Debug.LogWarning("No saved progress found!");
            UpdateContinueButton();
            return;
        }

        string savedScene = GameProgress.GetSavedScene();

        if (string.IsNullOrEmpty(savedScene))
        {
            Debug.LogWarning("Saved scene is empty!");
            return;
        }

        if (Application.CanStreamedLevelBeLoaded(savedScene))
        {
            Debug.Log("Continuing: " + savedScene);

            Time.timeScale = 1f;

            SceneManager.LoadScene(savedScene);
        }
        else
        {
            Debug.LogError(
                "Scene not found in Build Profiles: " + savedScene
            );
        }
    }


    // ==========================================
    // HOW TO PLAY
    // ==========================================

    public void OpenHowToPlay()
    {
        Debug.Log("Opening How To Play...");

        mainMenuPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }


    // ==========================================
    // SETTINGS
    // ==========================================

    public void OpenSettings()
    {
        Debug.Log("Opening Settings...");

        mainMenuPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }


    // ==========================================
    // BACK TO MAIN MENU
    // ==========================================

    public void BackToMainMenu()
    {
        ShowMainMenu();
    }


    // ==========================================
    // SHOW MAIN MENU
    // ==========================================

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        howToPlayPanel.SetActive(false);
        settingsPanel.SetActive(false);

        UpdateContinueButton();

        Debug.Log("Main Menu Opened!");
    }


    // ==========================================
    // EXIT GAME
    // ==========================================

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");

        Time.timeScale = 1f;

        Application.Quit();
    }


    // ==========================================
    // CONTINUE BUTTON STATE
    // ==========================================

    public void UpdateContinueButton()
    {
        if (continueButton == null)
            return;

        continueButton.interactable = GameProgress.HasProgress();
    }


    // ==========================================
    // MUSIC VOLUME
    // ==========================================

    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        if (musicSource != null)
        {
            musicSource.volume = volume;
        }

        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }


    // ==========================================
    // LOAD SETTINGS
    // ==========================================

    private void LoadSettings()
    {
        float volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        if (musicSlider != null)
        {
            musicSlider.SetValueWithoutNotify(volume);
        }

        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
    }
}
