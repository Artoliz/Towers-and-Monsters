﻿using UnityEngine;

public class Informations : MonoBehaviour
{
    [SerializeField] private EnemyInfo _enemyInfo = null;
    [SerializeField] private TowerInfo _towerInfo = null;

    private Camera _camera;

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
        public float _repair;
        public float _upgrade;
        public float _damageToEnemy;
        public float _speedAttack;
    };

    private enum Unit
    {
        None,
        Enemy,
        Tower
    };

    private Unit _unitType = Unit.None;

    private Enemy _enemy = null;
    private Tower _tower = null;

    private void Awake()
    {
        _enemyInfo.gameObject.SetActive(false);
        _towerInfo.gameObject.SetActive(false);
        _unitType = Unit.None;
        _camera = Camera.main;
    }

    private void Update()
    {
        if (!WavesManager.gameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (Input.GetMouseButtonDown(0) && !Builder.isBuilding)
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var click, 1000))
                {
                    if (click.collider.gameObject.CompareTag("Enemy"))
                    {
                        _unitType = Unit.Enemy;
                        _enemy = click.collider.gameObject.GetComponent<Enemy>();
                        _enemyInfo.gameObject.SetActive(true);
                        _towerInfo.gameObject.SetActive(false);
                    }
                    else if (click.collider.gameObject.CompareTag("Tower"))
                    {
                        _unitType = Unit.Tower;
                        _tower = click.collider.gameObject.GetComponent<Tower>();
                        _towerInfo.gameObject.SetActive(true);
                        _enemyInfo.gameObject.SetActive(false);
                    }
                    else
                    {
                        _unitType = Unit.None;
                        _enemy = null;
                        _tower = null;
                        _enemyInfo.gameObject.SetActive(false);
                        _towerInfo.gameObject.SetActive(false);
                    }
                }
            }
            if (_unitType == Unit.Enemy && _enemy)
            {
                //_enemyInfo.SetInformations(_enemy.GetInformations());
            } else if (_unitType == Unit.Tower && _tower)
            {
                //_towerInfo.SetInformations(_tower.GetInformations());
            }
        }
    }
}