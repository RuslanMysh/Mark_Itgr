using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                OpenPauseMenu();
        }
    }

    void OpenPauseMenu()
    {
        isPaused = true;

        Time.timeScale = 0f; // останавливаем игру

        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1f;

        SceneManager.UnloadSceneAsync("PauseMenu");
    }

    public static void QuitGame()
    {
        Time.timeScale = 1f;

    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
                Application.Quit();
    #endif
    }
}