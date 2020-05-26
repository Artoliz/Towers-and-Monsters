using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    #region PublicVariables

    public static bool GameIsPaused;

    public GameObject gameUi;

    public GameObject pauseMenuUi;

    public Animator transition;
    public float transitionTime = 1.0f;
    public float introTransitionTime = 5f;
    
    #endregion

    #region MonoBehavior

    private void Awake()
    {

        GameIsPaused = false;
        if (transition.gameObject.activeInHierarchy)
        {
            GameIsPaused = true;
            StartCoroutine(UnpauseAfterIntro());
        }

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
        if (GameManager.Instance.GameIsFinished)
            GameManager.Instance.SaveScore();

        ResumeGame();

        StartCoroutine(LoadLevel("Menu"));
    }

    IEnumerator LoadLevel(string levelName)
    {
        if (transition.gameObject.activeInHierarchy)
        {
            transition.SetTrigger("Start"); 
            yield return new WaitForSeconds(transitionTime);
        }


        SceneManager.LoadScene(levelName);
    }

    public void ExitToDesktop()
    {
        Application.Quit();
    }

    #endregion
    
    #region PrivateMethods
    
    IEnumerator UnpauseAfterIntro()
    {
       yield return new WaitForSeconds(introTransitionTime);

       GameIsPaused = false;
    }
    
    #endregion
}