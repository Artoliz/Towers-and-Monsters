﻿using UnityEngine;

public class Base : MonoBehaviour
{
    #region PrivateVariables

    private int _maxHealth;

    private float _maxSizeX;

    private bool _isBaseDestroyed;

    private Vector3 _size;

    #endregion

    #region SerializableVariables

    [SerializeField] private int baseHealth = 1000;

    [SerializeField] private Transform health = null;
    [SerializeField] private Transform progress = null;
    [SerializeField] private Transform mainCamera = null;

    #endregion

    #region PublicVariables

    public static Base instance;

    public Material[] hpColors;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        instance = this;
        progress.GetComponent<Renderer>().material = hpColors[0];
    }

    private void Start()
    {
        var localScale = progress.transform.localScale;

        _maxHealth = baseHealth;
        _maxSizeX = localScale.x;
        _size = localScale;
    }

    private void Update()
    {
        health.LookAt(mainCamera);

        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused && baseHealth <= 0)
        {
            _isBaseDestroyed = true;
        }
    }

    #endregion

    #region PublicMethods

    public bool IsBaseDestroyed()
    {
        return _isBaseDestroyed;
    }

    public void LoseHealth(int damage)
    {
        baseHealth -= damage;

        progress.GetComponent<Renderer>().material = hpColors[1];

        if (_maxHealth / 2 >= baseHealth)
            progress.GetComponent<Renderer>().material = hpColors[2];
        if (_maxHealth / 4 >= baseHealth)
            progress.GetComponent<Renderer>().material = hpColors[3];

        float currentSizeX = (baseHealth * _maxSizeX) / _maxHealth;
        progress.localScale = new Vector3(currentSizeX, _size.y, _size.z);

        float currentPosX = -((_maxSizeX - currentSizeX) / 2.0f);
        progress.localPosition = new Vector3(currentPosX, 0, 0);
    }

    #endregion
}