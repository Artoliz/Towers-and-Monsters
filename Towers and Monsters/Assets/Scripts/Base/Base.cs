using UnityEngine;
using UnityEngine.SceneManagement;

public class Base : MonoBehaviour
{
    #region SerializableVariables

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
        if (health <= 0)
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            health -= damage;
        }
    }

    #endregion
}
