using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    #region PublicVariables

    public static bool gameIsPaused;

    public GameObject gameUi;
    
    public GameObject pauseMenuUi;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        pauseMenuUi.SetActive(false);
        gameUi.SetActive(true);
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
        gameUi.SetActive(false);
        pauseMenuUi.SetActive(true);

        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        gameUi.SetActive(true);
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
