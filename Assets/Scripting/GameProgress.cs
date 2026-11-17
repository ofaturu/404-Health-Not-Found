using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameProgress
{
    private const string SAVE_SCENE_KEY = "SavedScene";

    public static void SaveCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        PlayerPrefs.SetString(SAVE_SCENE_KEY, currentScene);
        PlayerPrefs.Save();

        Debug.Log("Progress Saved: " + currentScene);
    }

    public static bool HasProgress()
    {
        return PlayerPrefs.HasKey(SAVE_SCENE_KEY);
    }

    public static string GetSavedScene()
    {
        return PlayerPrefs.GetString(SAVE_SCENE_KEY, string.Empty);
    }

    public static void ClearProgress()
    {
        PlayerPrefs.DeleteKey(SAVE_SCENE_KEY);
        PlayerPrefs.Save();

        Debug.Log("Progress Cleared!");
    }
}
