using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    #region PublicVariables

    public static bool GameIsPaused;

    public GameObject gameUi;

    public GameObject pauseMenuUi;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        GameIsPaused = false;

        pauseMenuUi.SetActive(false);
        gameUi.SetActive(true);
    }

    private void Update()
    {
        if (!WavesManager.GameIsFinished && Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    #endregion

    #region PublicMethods

    public void PauseGame()
    {
        gameUi.SetActive(false);
        pauseMenuUi.SetActive(true);

        GameIsPaused = true;
    }

    public void ResumeGame()
    {
        gameUi.SetActive(true);
        pauseMenuUi.SetActive(false);

        Time.timeScale = 1;
        GameIsPaused = false;
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