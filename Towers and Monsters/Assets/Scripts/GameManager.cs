using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region PublicVariables

    public bool GameIsFinished = false;

    public GameObject gameOverUi;

    public int _enemiesKilled = 0;

    #endregion

    #region PrivateVariables

    public static GameManager Instance = null;

    [SerializeField] private Text _timerSinceStart = null;

    [SerializeField] private Texture2D _cursor = null;

    private float _hours = 0;
    private float _minutes = 0;
    private float _seconds = 0;

    [SerializeField] private Text _goldsText = null;

    [SerializeField] private int _golds = 200;

    private int _roundEnded = 0;

    private static GameObject errorMessage;

    private List<Enemy> _enemies = new List<Enemy>();

    [SerializeField] private HighScores _scores = null;
    [SerializeField] private InputField _username = null;

    #endregion

    #region MoboBehaviour

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Instance = null;
            Destroy(this);
        }

        if (_cursor != null)
            Cursor.SetCursor(_cursor, new Vector2(0, 0), CursorMode.ForceSoftware);

        errorMessage = GameObject.Find("TextError");
        if (errorMessage != null)
            errorMessage.SetActive(false);
    }

    void Start()
    {
        _timerSinceStart.text = "00:00:00";
        _goldsText.text = _golds.ToString();
    }

    void Update()
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
        {
            UpdateTimerSinceStart();
        }
    }

    #endregion

    #region PrivateFunctions
    
    private void UpdateTimerSinceStart()
    {
        _seconds += Time.deltaTime;

        if (_seconds >= 60)
        {
            _minutes += 1;
            _seconds = 0;
        }
        if (_minutes >= 60)
        {
            _hours += 1;
            _minutes = 0;
        }

        _timerSinceStart.text = _hours.ToString("00") + ":" + _minutes.ToString("00") + ":" + _seconds.ToString("00");
    }

    #endregion

    #region PublicFunctions

    public void AddEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
    }

    public void ReDrawPathFindingForAll()
    {
        foreach (Enemy enemy in _enemies)
            enemy.ReDrawPathFinding();
    }

    public void AddGolds(int golds)
    {
        _golds += golds;
        _goldsText.text = _golds.ToString();
    }

    public void RemoveGolds(int golds)
    {
        _golds -= golds;
        _goldsText.text = _golds.ToString();
    }

    public int GetGolds()
    {
        return _golds;
    }

    public void EndGame(int wave)
    {
        _roundEnded = wave;

        gameOverUi.GetComponent<GameOver>().EndGame(_roundEnded, _enemiesKilled);

        GameIsFinished = true;
    }

    public void SaveScore()
    {
        var name = "User";
        if (_username.text != "")
            name = _username.text;

        _scores.SaveScore(name, _roundEnded);
    }

    public static IEnumerator DisplayError(string message)
    {
        if (errorMessage != null)
        {
            errorMessage.GetComponent<Text>().text = message;
            errorMessage.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            errorMessage.SetActive(false);
        }
        yield return new WaitForSeconds(0.0f);
    }

    #endregion
}
