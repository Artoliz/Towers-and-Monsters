using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    #region PublicVariables

    public static bool gameIsPaused;

    public GameObject pauseMenuUi;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        pauseMenuUi.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    #endregion

    #region PublicMethods

    public void PauseGame()
    {
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    public void ReturnToMenu()
    {
        ResumeGame();

        SceneManager.LoadScene("Menu");
    }

    public void ExitToDesktop()
    {
        Application.Quit();
    }

    #endregion
}
