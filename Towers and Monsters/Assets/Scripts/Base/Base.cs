using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Base : MonoBehaviour
{
    #region SerializableVariables

    [SerializeField] private int health = 1000;

    [SerializeField] private Transform _health;

    [SerializeField] private Transform _progress;

    #endregion

    #region PrivateVariables

    private float _maxSizeX;
    private int _maxHealth;

    private Vector3 _size;

    #endregion

    #region PublicVariables

    public static Base instance;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _maxHealth = health;
        _maxSizeX = _progress.transform.localScale.x;
        _size = _progress.transform.localScale;
    }

    private void Update()
    {
        _health.LookAt(Camera.main.transform);
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

        float currentSizeX = (health * _maxSizeX) / _maxHealth;
        _progress.localScale = new Vector3(currentSizeX, _size.y, _size.z);

        float currentPosX = -((_maxSizeX - currentSizeX) / 2.0f);
        _progress.localPosition = new Vector3(currentPosX, 0, 0);
    }

    #endregion
}
