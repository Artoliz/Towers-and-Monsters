using UnityEngine;

public class Informations : MonoBehaviour
{
    [SerializeField] private EnemyInfo _enemyInfo = null;
    [SerializeField] private TowerInfo _towerInfo = null;

    [SerializeField] private GameObject _selectedEnemy = null;
    [SerializeField] private GameObject _selectedTower = null;

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
                        ResetSelected();
                        _unitType = Unit.Enemy;
                        _enemy = click.collider.gameObject.GetComponent<Enemy>();
                        _enemy.IsSelected(_selectedEnemy);
                        _enemyInfo.gameObject.SetActive(true);
                        _towerInfo.gameObject.SetActive(false);
                    }
                    else if (click.collider.gameObject.CompareTag("Tower"))
                    {
                        ResetSelected();
                        _unitType = Unit.Tower;
                        _tower = click.collider.gameObject.GetComponent<Tower>();
                        //_tower.IsSelected(_selectedTower);
                        _towerInfo.gameObject.SetActive(true);
                        _enemyInfo.gameObject.SetActive(false);
                    }
                    else
                        ResetSelected();
                }
            }
            if (_unitType == Unit.Enemy && _enemy)
                _enemyInfo.SetInformations(_enemy.GetEnemyData());
            else if (_unitType == Unit.Tower && _tower)
            {
                //_towerInfo.SetInformations(_tower.GetInformations());
                //_towerInfo.SetListener(_tower.Upgrade(), _tower.Repair(), _tower.destroyParticle());
            }
        }
    }

    private void ResetSelected()
    {
        _unitType = Unit.None;
        if (_enemy != null)
            _enemy.IsSelected(null);
        //if (_tower != null)
        //    _tower.IsSelected(null);
        _enemy = null;
        _tower = null;
        _enemyInfo.gameObject.SetActive(false);
        _towerInfo.gameObject.SetActive(false);
    }
}
