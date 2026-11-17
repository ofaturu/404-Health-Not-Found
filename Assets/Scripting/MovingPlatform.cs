
using UnityEngine;

public static class GameProgress
{
    private const string SAVE_SCENE_KEY = "SavedScene";

    public static void SaveCurrentScene()
    {
        PlayerPrefs.SetString(
            SAVE_SCENE_KEY,
            UnityEngine.SceneManagement.SceneManager
                .GetActiveScene().name
        );

        PlayerPrefs.Save();

        Debug.Log("Progress Saved!");
    }

    public static void ClearProgress()
    {
        PlayerPrefs.DeleteKey(SAVE_SCENE_KEY);
        PlayerPrefs.Save();
    }
}
