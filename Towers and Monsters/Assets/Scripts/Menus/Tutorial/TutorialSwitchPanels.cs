using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSwitchPanels : MonoBehaviour
{
    #region PrivateVariables

    private int _activePanel;

    #endregion
    
    #region SerializableVariables

    [SerializeField] private GameObject tutorialTitle;
    [SerializeField] private GameObject tutorialButtons;

    [SerializeField] private List<GameObject> tutorialPanels;

    #endregion

    #region MonoBehaviour

    private void OnEnable()
    {
        DisableAllPanels();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisableAllPanels();
        }
    }

    #endregion

    #region PublicMethods

    public void DisableAllPanels()
    {
        _activePanel = -1;

        tutorialTitle.SetActive(true);
        tutorialButtons.SetActive(true);

        foreach (var panel in tutorialPanels)
        {
            panel.SetActive(false);
        }
    }

    public void EnablePanelByName(string panelName)
    {
        tutorialTitle.SetActive(false);
        tutorialButtons.SetActive(false);

        var i = 0;
        foreach (var panel in tutorialPanels)
        {
            if (panel.name == panelName)
            {
                panel.SetActive(true);
                _activePanel = i;
            }
            else
            {
                panel.SetActive(false);
            }

            i++;
        }
    }

    public void GoPreviousPanel()
    {
        tutorialPanels[_activePanel].SetActive(false);
        
        if (_activePanel == 0)
        {
            _activePanel = tutorialPanels.Count - 1;
            tutorialPanels[_activePanel].SetActive(true);
        }
        else
        {
            _activePanel -= 1;
            tutorialPanels[_activePanel].SetActive(true);
        }
    }

    public void GoNextPanel()
    {
        tutorialPanels[_activePanel].SetActive(false);
        
        if (_activePanel == tutorialPanels.Count - 1)
        {
            _activePanel = 0;
            tutorialPanels[_activePanel].SetActive(true);
        }
        else
        {
            _activePanel += 1;
            tutorialPanels[_activePanel].SetActive(true);
        }
    }

    #endregion
}