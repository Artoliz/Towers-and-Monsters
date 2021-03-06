﻿using UnityEngine;
using UnityEngine.EventSystems;

public class Informations : MonoBehaviour
{
    [SerializeField] private EnemyInfo _enemyInfo = null;
    [SerializeField] private TowerInfo _towerInfo = null;
    [SerializeField] private WallInfo _wallInfo = null;

    [SerializeField] private GameObject _selectedEnemy = null;
    [SerializeField] private GameObject _selectedTower = null;

    [SerializeField] private GameObject _background = null;

    private Camera _camera;

    public static Informations Instance = null;

    public struct EnemyData
    {
        public float _hp;
        public float _golds;
        public float _damageToBase;
        public float _damageToTower;
        public float _speed;
        public float _speedAttack;
    };

    public struct TowerData
    {
        public float _hp;
        public float _maxHp;
        public float _repair;
        public float _upgrade;
        public float _sell;
        public float _damageToEnemy;
        public float _damageEffect;
        public float _speedAttack;
        public Tower.towerType _type;
        public bool _isUpgrade;
    };

    public struct WallData
    {
        public int _hp;
        public int _sell;
    };

    private Enemy _enemy = null;
    private Tower _tower = null;
    private Wall _wall = null;

    private void Awake()
    {
        _enemyInfo.gameObject.SetActive(false);
        _towerInfo.gameObject.SetActive(false);
        _wallInfo.gameObject.SetActive(false);
        _background.SetActive(false);
        _camera = Camera.main;
        if (Instance != null)
        {
            Instance = null;
            Destroy(this);
        }
        else
            Instance = this;
    }

    private void Update()
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (_camera && !EventSystem.current.IsPointerOverGameObject(-1) && Input.GetMouseButtonDown(0))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var click, 1000))
                {
                    if (!click.collider.CompareTag("Tower") && !click.collider.CompareTag("Enemy") && !click.collider.CompareTag("Wall"))
                        ResetSelected();
                    else
                        Builder.Instance.SetBuildingEnum(Buildings.None);
                }
            }
        }
    }

    public void ResetSelected()
    {
        if (_enemy != null)
            _enemy.IsSelected(null);
        if (_tower != null)
            _tower.IsSelected(null);
        if (_wall != null)
            _wall.IsSelected = false;
        _enemy = null;
        _tower = null;
        _wall = null;
        _enemyInfo.gameObject.SetActive(false);
        _towerInfo.gameObject.SetActive(false);
        _wallInfo.gameObject.SetActive(false);
        _background.SetActive(false);
    }

    public void SetInformations(TowerData data, Tower tower)
    {
        ResetSelected();

        _tower = tower;
        _tower.IsSelected(_selectedTower);
        _background.SetActive(true);
        _towerInfo.gameObject.SetActive(true);
        _towerInfo.SetInformations(data);
        _towerInfo.SetListener(_tower);
    }

    public void SetInformations(EnemyData data, Enemy enemy)
    {
        ResetSelected();

        _enemy = enemy;
        enemy.IsSelected(_selectedEnemy);
        _background.SetActive(true);
        _enemyInfo.gameObject.SetActive(true);
        _enemyInfo.SetInformations(data);
    }

    public void SetInformations(WallData data, Wall wall)
    {
        ResetSelected();

        _wall = wall;
        _wall.IsSelected = true;
        _background.SetActive(true);
        _wallInfo.gameObject.SetActive(true);
        _wallInfo.SetInformations(data);
        _wallInfo.SetListener(wall);
    }
}
