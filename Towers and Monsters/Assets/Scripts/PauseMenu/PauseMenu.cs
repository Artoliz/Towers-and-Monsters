using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    #region PublicVariables

    public static bool gameIsPaused;

    public Button pauseButton;
    
    public GameObject pauseMenuUi;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        pauseMenuUi.SetActive(false);
        pauseButton.gameObject.SetActive(true);
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
        pauseButton.gameObject.SetActive(false);
        pauseMenuUi.SetActive(true);

        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        pauseButton.gameObject.SetActive(true);
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
