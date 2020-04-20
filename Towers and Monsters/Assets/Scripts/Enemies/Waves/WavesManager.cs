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

    #region SerializableVariables

    [SerializeField] private int waveNumber = 1;
    [SerializeField] private int weightMultiplier = 10;
    
    [SerializeField] private float setTimeBetweenWaves = 60.0f;

    #endregion

    #region PublicVariables

    public static bool gameIsBetweenWaves;

    public Text timeBetweenWavesText;

    public EnemiesSpawns enemiesSpawns;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        SetGameStatus(true);
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
        else if (!PauseMenu.gameIsPaused && gameIsBetweenWaves)
        {
            SetNextWave();
        }
    }

    #endregion

    #region PrivateMethods

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

    private void RunWave()
    {
        if (_firstWave)
            _firstWave = false;

        SetGameStatus(false);
        SetTimeBetweenWaves(false, 0);
        
        enemiesSpawns.LaunchSpawns(_waveWeight);
    }

    private void CalculateNewWaveWeight()
    {
        _waveWeight = weightMultiplier * waveNumber;
    }

    private void SetNextWave()
    {
        SetTimeBetweenWaves(true, setTimeBetweenWaves);
        
        if (!_firstWave)
            waveNumber += 1;
        CalculateNewWaveWeight();
    }

    #endregion

    #region PublicVariables

    public static void SetGameStatus(bool isBetweenWaves)
    {
        gameIsBetweenWaves = isBetweenWaves;
    }

    #endregion
}
