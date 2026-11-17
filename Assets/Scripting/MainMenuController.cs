
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string firstLevelScene = "Level1";

    [Header("Menu Buttons")]
    [SerializeField] private Button continueButton;

    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Audio Settings")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private AudioSource musicSource;

    private const string SAVE_SCENE_KEY = "SavedScene";
    private const string MUSIC_VOLUME_KEY = "MusicVolume";

    private void Start()
    {
        ShowMainMenu();
        UpdateContinueButton();
        LoadSettings();
    }

    // NEW GAME
    public void NewGame()
    {
        PlayerPrefs.DeleteKey(SAVE_SCENE_KEY);
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene(firstLevelScene);
    }

    // CONTINUE
    public void ContinueGame()
    {
        string savedScene = PlayerPrefs.GetString(
            SAVE_SCENE_KEY, ""
        );

        if (!string.IsNullOrEmpty(savedScene))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(savedScene);
        }
    }

    // HOW TO PLAY
    public void OpenHowToPlay()
    {
        mainMenuPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    // SETTINGS
    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    // BACK TO MAIN MENU
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        howToPlayPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    // EXIT GAME
    public void ExitGame()
    {
        Debug.Log("Game Closed");

        Application.Quit();
    }

    // UPDATE CONTINUE BUTTON
    private void UpdateContinueButton()
    {
        if (continueButton == null) return;

        bool hasSave = PlayerPrefs.HasKey(SAVE_SCENE_KEY);

        continueButton.interactable = hasSave;
    }

    // MUSIC VOLUME
    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
            musicSource.volume = volume;

        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        float volume = PlayerPrefs.GetFloat(
            MUSIC_VOLUME_KEY, 0.5f
        );

        if (musicSlider != null)
            musicSlider.value = volume;

        if (musicSource != null)
            musicSource.volume = volume;
    }
}
