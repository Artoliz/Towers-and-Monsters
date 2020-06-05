using System.Collections.Generic;
using UnityEngine;

public class TutorialSwitchPanels : MonoBehaviour
{
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

    #endregion

    #region PrivateMethods

    private void DisableAllPanels()
    {
        tutorialTitle.SetActive(true);
        tutorialButtons.SetActive(true);

        foreach (var panel in tutorialPanels)
        {
            panel.SetActive(false);
        }
    }

    #endregion

    #region PublicMethods

    public void EnablePanelByName(string panelName)
    {
        tutorialTitle.SetActive(false);
        tutorialButtons.SetActive(false);

        foreach (var panel in tutorialPanels)
        {
            panel.SetActive(panel.name == panelName);
        }
    }

    #endregion
}