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

    public static bool GameIsBetweenWaves;
    public static bool GameIsFinished;

    public GameObject gameUi;
    public GameObject gameOverUi;

    public Text timeBetweenNextWaveText;
    public Text waveNumberText;

    public Button skipBetweenWavesButton;

    public SpawnManager spawnManager;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        GameIsFinished = false;

        gameOverUi.SetActive(false);

        skipBetweenWavesButton.gameObject.SetActive(false);
        timeBetweenNextWaveText.gameObject.SetActive(false);
        waveNumberText.gameObject.SetActive(false);

        SetGameStatus(true);
    }

    private void Update()
    {
        if (!GameIsFinished && !PauseMenu.GameIsPaused && _timerBetweenWavesIsRunning)
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
        } else if (!GameIsFinished && !PauseMenu.GameIsPaused && Base.instance.IsBaseDestroyed())
        {
            GameIsFinished = true;
            gameUi.SetActive(false);
            gameOverUi.SetActive(true);
            GameManager.Instance.EndGame(waveNumber - 1);
        }
        else if (!GameIsFinished && !PauseMenu.GameIsPaused && GameIsBetweenWaves)
        {
            EnableUiBetweenWaves();
            SetNextWave();
        }
    }

    #endregion

    #region PrivateMethods

    private void DisplayTimeBetweenWaves(float timeToDisplay)
    {
        timeBetweenNextWaveText.text = timeToDisplay.ToString("00") + "s";
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

        waveNumberText.text = $"Round {waveNumber}";
    }

    private void RunWave()
    {
        if (_firstWave)
            _firstWave = false;

        SetGameStatus(false);
        SetTimeBetweenWaves(false, 0);

        SoundsManager.Instance.PlaySound(SoundsManager.Audio.WaveStart);

        spawnManager.LaunchSpawns(_waveWeight, waveNumber);
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
        GameIsBetweenWaves = isBetweenWaves;
    }

    public void SkipBetweenWaves()
    {
        if (!PauseMenu.GameIsPaused && _timerBetweenWavesIsRunning)
        {
            _timeBetweenWaves = -1;
            DisplayTimeBetweenWaves(_timeBetweenWaves);
        }
    }

    #endregion
}