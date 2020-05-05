using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region PublicMethods

    public void LaunchLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    #endregion
}
