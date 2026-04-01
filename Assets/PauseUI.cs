using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public void Resume()
    {
        PauseManager.ResumeGame();
    }

    public void Quit()
    {
        PauseManager.QuitGame();
    }
}