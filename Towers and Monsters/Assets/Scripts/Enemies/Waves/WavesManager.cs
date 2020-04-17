using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WavesManager : MonoBehaviour
{
    #region PrivateVariables

    private int _waveWeight;
    
    private float _timeBetweenWaves;
    
    //_firstWave explanations: if it's the first wave we don't reset the values so that:
    // -> wave's weight and number are not increased.
    private bool _firstWave = true;
    private bool _timerBetweenWavesIsRunning;

    #endregion

    #region PublicVariables

    public int waveNumber = 1;
    
    public float setTimeBetweenWaves = 60.0f;

    public static bool gameIsBetweenWaves;

    public Text timeBetweenWavesText;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        SetGameStatus(true);
    }

    private void Start()
    {
        StartCoroutine(LaunchTimerWhenSceneIsLoaded());
    }

    private void Update()
    {
        if (!PauseMenu.gameIsPaused && _timerBetweenWavesIsRunning)
        {
            if (_timeBetweenWaves > 0)
            {
                _timeBetweenWaves -= Time.deltaTime;
                DisplayTimeBetweenWaves(_timeBetweenWaves);
            }
            else
            {
                RunWave();
            }
        }
        else if (!PauseMenu.gameIsPaused && gameIsBetweenWaves && !_firstWave)
        {
            SetNextWave();
        }
    }

    #endregion

    #region PrivateMethods

    private IEnumerator LaunchTimerWhenSceneIsLoaded()
    {
        yield return new WaitForSeconds(0.5f);
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

    private static void SetGameStatus(bool isBetweenWaves)
    {
        gameIsBetweenWaves = isBetweenWaves;
    }

    private void RunWave()
    {
        _firstWave = false;
        
        SetGameStatus(false);
        SetTimeBetweenWaves(false, 0);
    }

    private void SetNextWave()
    {
        SetGameStatus(true);
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
