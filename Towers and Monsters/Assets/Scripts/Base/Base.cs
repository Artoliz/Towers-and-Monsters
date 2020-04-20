using UnityEngine;
using UnityEngine.SceneManagement;

public class Base : MonoBehaviour
{
    #region PrivateVariables

    [SerializeField] private int health = 1000;

    #endregion
    
    #region PublicVariables

    public static Base instance;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        instance = this;
    }

    #endregion
    
    #region PublicMethods

    public void LoseHealth(int damage)
    {
        if (health > 0)
        {
            health -= damage;
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
    }

    #endregion
}
