﻿using UnityEngine;
using UnityEngine.SceneManagement;
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
    public static bool gameIsFinished;

    public GameObject gameUi;
    public GameObject gameOverUi;

    public Text timeBetweenNextWaveText;
    public Text waveNumberText;

    public Button skipBetweenWavesButton;

    public EnemiesSpawns enemiesSpawns;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        gameIsFinished = false;

        gameOverUi.SetActive(false);

        skipBetweenWavesButton.gameObject.SetActive(false);
        timeBetweenNextWaveText.gameObject.SetActive(false);
        waveNumberText.gameObject.SetActive(false);

        SetGameStatus(true);
    }

    private void Update()
    {
        if (!gameIsFinished && Base.instance.IsBaseDestroyed())
        {
            gameIsFinished = true;
            gameUi.SetActive(false);
            gameOverUi.SetActive(true);
        }
        else if (gameIsFinished)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene("Menu");
        }
        else if (!gameIsFinished && !PauseMenu.gameIsPaused && _timerBetweenWavesIsRunning)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                SkipBetweenWaves();

            if (_timeBetweenWaves > 0)
            {
                _timeBetweenWaves -= Time.deltaTime;
                DisplayTimeBetweenWaves(_timeBetweenWaves);
            }
            else
            {
                EnableUiDuringWave();
                RunWave();
            }
        }
        else if (!gameIsFinished && !PauseMenu.gameIsPaused && gameIsBetweenWaves)
        {
            EnableUiBetweenWaves();
            SetNextWave();
        }
    }

    #endregion

    #region PrivateMethods

    private void DisplayTimeBetweenWaves(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeBetweenNextWaveText.text = $"Time before next wave : {minutes:00}:{seconds:00}";
    }

    private void SetTimeBetweenWaves(bool timerBetweenIsRunning, float timeBetweenWaves)
    {
        _timerBetweenWavesIsRunning = timerBetweenIsRunning;
        _timeBetweenWaves = timeBetweenWaves;
    }

    private void EnableUiBetweenWaves()
    {
        skipBetweenWavesButton.gameObject.SetActive(true);
        timeBetweenNextWaveText.gameObject.SetActive(true);
        waveNumberText.gameObject.SetActive(false);
    }

    private void EnableUiDuringWave()
    {
        skipBetweenWavesButton.gameObject.SetActive(false);
        timeBetweenNextWaveText.gameObject.SetActive(false);
        waveNumberText.gameObject.SetActive(true);

        waveNumberText.text = $"Wave : {waveNumber}";
    }

    private void RunWave()
    {
        if (_firstWave)
            _firstWave = false;

        SetGameStatus(false);
        SetTimeBetweenWaves(false, 0);

        enemiesSpawns.LaunchSpawns(_waveWeight, waveNumber);
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

    public void SkipBetweenWaves()
    {
        if (!PauseMenu.gameIsPaused && _timerBetweenWavesIsRunning)
        {
            _timeBetweenWaves = -1;
            DisplayTimeBetweenWaves(_timeBetweenWaves);
        }
    }

    #endregion
}