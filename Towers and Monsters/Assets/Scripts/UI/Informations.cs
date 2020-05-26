using UnityEngine;

public class Informations : MonoBehaviour
{
    [SerializeField] private EnemyInfo _enemyInfo = null;
    [SerializeField] private TowerInfo _towerInfo = null;

    [SerializeField] private GameObject _selectedEnemy = null;
    [SerializeField] private GameObject _selectedTower = null;

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
        public float _repair;
        public float _upgrade;
        public float _damageToEnemy;
        public float _speedAttack;
    };

    private Enemy _enemy = null;
    private Tower _tower = null;

    private void Awake()
    {
        _enemyInfo.gameObject.SetActive(false);
        _towerInfo.gameObject.SetActive(false);
        _camera = Camera.main;
        if (Instance != null)
        {
            Instance = null;
            Destroy(this);
        }
        else
            Instance = this;
    }

    public void ResetSelected()
    {
        if (_enemy != null)
            _enemy.IsSelected(null);
        if (_tower != null)
            _tower.IsSelected(null);
        _enemy = null;
        _tower = null;
        _enemyInfo.gameObject.SetActive(false);
        _towerInfo.gameObject.SetActive(false);
    }

    public void SetInformations(TowerData data, Tower tower)
    {
        ResetSelected();

        _tower = tower;
        _tower.IsSelected(_selectedTower);
        _towerInfo.gameObject.SetActive(true);
        _enemyInfo.gameObject.SetActive(false);
        _towerInfo.SetInformations(data);
        _towerInfo.SetListener(_tower);
    }

    public void SetInformations(EnemyData data, Enemy enemy)
    {
        ResetSelected();

        _enemy = enemy;
        enemy.IsSelected(_selectedEnemy);
        _enemyInfo.gameObject.SetActive(true);
        _towerInfo.gameObject.SetActive(false);
        _enemyInfo.SetInformations(data);
    }
}
