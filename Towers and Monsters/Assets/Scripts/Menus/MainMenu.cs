using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    #region PublicMethods

    public Animator transition;
    public float transitionTime = 2.0f;

    //public List<NamedMenu> menus;
    public GameObject []Menus;

    public Texture2D _cursor = null;
    #endregion

    void Awake()
    {
        transition.SetBool("Menu", true);
        
        if (_cursor != null)
            Cursor.SetCursor(_cursor, new Vector2(0, 0), CursorMode.ForceSoftware);

        SelectMainMenu();
    }

    public void LaunchLevel(string levelName)
    {
        StartCoroutine(LoadLevel(levelName));
    }

    IEnumerator LoadLevel(string levelName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);
    }
    
    public void SelectLevelSelectionMenu()
    {
        foreach (GameObject menu in Menus)
        {
            if (menu.name == "LevelSelectionMenu")
                menu.SetActive(true);
            else
                menu.SetActive(false);
        }   
    }
    
    public void SelectOptionMenu()
    {
        foreach (GameObject menu in Menus)
        {
            if (menu.name == "OptionMenu")
            {
                menu.SetActive(true);
            }
                
            else
                menu.SetActive(false);
        } 
    }

    public void SelectHighScoreMenu()
    {
        foreach (GameObject menu in Menus)
        {
            if (menu.name == "HighScoreMenu")
                menu.SetActive(true);
            else
                menu.SetActive(false);
        } 
    }

    public void SelectMainMenu()
    {
        foreach (GameObject menu in Menus)
        {
            if (menu.name == "MainMenu")
                menu.SetActive(true);
            else
                menu.SetActive(false);
        } 
    }

    public void SelectHumanValleyLobby()
    {
        foreach (GameObject menu in Menus)
        {
            if (menu.name == "HumanValleyLobby")
                menu.SetActive(true);
            else
                menu.SetActive(false);
        } 
    }

    public void SelectDwarfMountainLobby()
    {
        foreach (GameObject menu in Menus)
        {
            if (menu.name == "DwarfMountainLobby")
                menu.SetActive(true);
            else
                menu.SetActive(false);
        } 
    }

    public void SelectElvenForestLobby()
    {
        foreach (GameObject menu in Menus)
        {
            if (menu.name == "ElvenForestLobby")
                menu.SetActive(true);
            else
                menu.SetActive(false);
        } 
    }

    public void SelectGoblinDesertLobby()
    {
        foreach (GameObject menu in Menus)
        {
            if (menu.name == "GoblinDesertLobby")
                menu.SetActive(true);
            else
                menu.SetActive(false);
        } 
    }

    public void QuitGame()
    {
        foreach (GameObject menu in Menus)
        {
            if (menu.name == "QuitGameConfirmation")
                menu.SetActive(true);
        } 
    }
    
    public void QuitGameConfirm()
    {
        Application.Quit();
    }    
    
    public void QuitGameCancel()
    {
        foreach (GameObject menu in Menus)
        {
            if (menu.name == "QuitGameConfirmation")
                menu.SetActive(false);
        } 
    }
}