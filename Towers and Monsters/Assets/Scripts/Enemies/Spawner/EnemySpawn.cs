using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    #region PrivateVariables

    private GameObject _playerBase;
    
    [SerializeField] private float timeBeforeBeginToSpawn = 2.0f;
    [SerializeField] private float repeatTimeToSpawn = 2.0f;
    
    private readonly List<GameObject> _enemiesToSpawn = new List<GameObject>();

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), timeBeforeBeginToSpawn, repeatTimeToSpawn);
    }

    #endregion

    #region PrivateMethods

    private void SpawnEnemy()
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused && !WavesManager.GameIsBetweenWaves)
        {
            if (_enemiesToSpawn.Count > 0)
            {
                var enemy = Instantiate(_enemiesToSpawn[0], transform.position, Quaternion.identity);

                var enemyController = enemy.GetComponent<Enemy>();
                enemyController.SetPlayerBase(_playerBase);

                _enemiesToSpawn.Remove(_enemiesToSpawn[0]);
            }
        }
    }

    #endregion

    #region PublicMethods

    public void SetPlayerBase(GameObject playerBase)
    {
        _playerBase = playerBase;
    }
    
    public void AddEnemyToSpawn(GameObject enemy)
    {
        _enemiesToSpawn.Add(enemy);
    }

    #endregion
}