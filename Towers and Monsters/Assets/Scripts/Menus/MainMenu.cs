using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region PublicMethods

    public GameObject MainMenuCanvas;
    public GameObject LevelSelectionCanvas;
    public GameObject OptionMenuCanvas;
    public GameObject HighScoreMenuCanvas;
    public GameObject QuitGameConfirmationMenuCanvas;

    void Awake()
    {
        MainMenuCanvas.SetActive(true);
        LevelSelectionCanvas.SetActive(false);
        OptionMenuCanvas.SetActive(false);
//        HighScoreMenuCanvas.SetActive(false);
        QuitGameConfirmationMenuCanvas.SetActive(false);
    }

    public void LaunchLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void SelectLevelSelectionMenu()
    {
        MainMenuCanvas.SetActive(false);
        
        LevelSelectionCanvas.SetActive(true);
    }
    
    public void SelectOptionMenu()
    {
        MainMenuCanvas.SetActive(false);
        
        OptionMenuCanvas.SetActive(true);
    }

    public void SelectHighScoreMenu()
    {
        MainMenuCanvas.SetActive(false);
        
        HighScoreMenuCanvas.SetActive(true);
    }

    public void SelectMainMenuFromOptionMenu()
    {
        OptionMenuCanvas.SetActive(false);
        
        MainMenuCanvas.SetActive(true);
    }

    public void SelectMainMenuFromLevelSelectionMenu()
    {
        LevelSelectionCanvas.SetActive(false);
        
        MainMenuCanvas.SetActive(true);
    }

    public void SelectMainMenuFromHighScoreMenu()
    {
        HighScoreMenuCanvas.SetActive(false);
        
        MainMenuCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        QuitGameConfirmationMenuCanvas.SetActive(true);
    }
    
    public void QuitGameConfirm()
    {
        Application.Quit();
    }    
    
    public void QuitGameCancel()
    {
        QuitGameConfirmationMenuCanvas.SetActive(false);
    }

    #endregion
}