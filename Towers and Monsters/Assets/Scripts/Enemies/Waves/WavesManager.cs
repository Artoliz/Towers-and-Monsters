using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WavesManager : MonoBehaviour
{
    #region PrivateVariables

    private int _waveWeight;
    
    private float _timeBetweenWaves;
    
    private bool _firstWave = true;
    private bool _timerBetweenWavesIsRunning;

    #endregion

    #region PublicVariables

    public int waveNumber = 1;
    
    public float setTimeBetweenWaves = 60.0f;

    public static bool GameIsBetweenWaves;
    
    public Text timeBetweenWavesText;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        SetGame(0, true);
    }

    private void Start()
    {
        StartCoroutine(LaunchTimerWhenSceneIsLoaded());
    }

    private void Update()
    {
        if (_timerBetweenWavesIsRunning)
        {
            if (_timeBetweenWaves > 0)
            {
                _timeBetweenWaves -= Time.unscaledDeltaTime;
                DisplayTimeBetweenWaves(_timeBetweenWaves);
            }
            else
            {
                RunWave();
            }
        }
        else if (GameIsBetweenWaves && !_firstWave) //If it's the first wave we don't reset the data
        {
            SetNextWave();
        }
    }

    #endregion

    #region PrivateMethods

    private IEnumerator LaunchTimerWhenSceneIsLoaded()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SetTimeBetweenWaves(true, setTimeBetweenWaves);
    }
    
    void DisplayTimeBetweenWaves(float timeToDisplay)
    {
        timeToDisplay += 1;
        
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);  
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeBetweenWavesText.text = $"Time before wave {waveNumber} : {minutes:00}:{seconds:00}";
    }

    private void SetTimeBetweenWaves(bool timerBetweenIsRunning, float timeBetweenWaves)
    {
        _timerBetweenWavesIsRunning = timerBetweenIsRunning;
        _timeBetweenWaves = timeBetweenWaves;
    }

    private static void SetGame(int timeScale, bool isBetweenWaves)
    {
        Time.timeScale = timeScale;
        GameIsBetweenWaves = isBetweenWaves;
    }

    private void RunWave()
    {
        _firstWave = false;
        
        SetGame(1, false);
        SetTimeBetweenWaves(false, 0);
    }

    private void SetNextWave()
    {
        SetGame(0, true);
        SetTimeBetweenWaves(true, setTimeBetweenWaves);
        waveNumber += 1;
        _waveWeight += waveNumber;
    }

    #endregion

    #region PublicMethods

    public int GetWaveWeight()
    {
        return _waveWeight;
    }

    #endregion
}
