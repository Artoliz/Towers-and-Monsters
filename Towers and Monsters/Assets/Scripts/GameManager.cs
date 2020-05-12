using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region PrivateVariables

    public static GameManager Instance = null;

    [SerializeField] private Text _timerSinceStart = null;

    private float _hours = 0;
    private float _minutes = 0;
    private float _seconds = 0;

    [SerializeField] private Text _goldsText = null;

    [SerializeField] private int _golds = 200;

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
    }

    void Start()
    {
        _timerSinceStart.text = "00:00:00";
        _goldsText.text = _golds.ToString();
    }

    void Update()
    {
        if (!WavesManager.gameIsFinished && !PauseMenu.GameIsPaused)
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

    #endregion
}
