using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawns : MonoBehaviour
{
    #region PrivateVariables

    private int _enemiesToSpawn;
    private int _enemiesAlive;

    private Transform[] _spawns;

    private List<GameObject> _enemies;

    #endregion

    #region PublicVariables

    public GameObject enemyPrefab;
    public GameObject playerBase;

    public static EnemiesSpawns instance;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        instance = this;
        
        _enemies = new List<GameObject>();

        var i = 0;
        _spawns = new Transform[transform.childCount];

        foreach (Transform child in transform)
        {
            _spawns[i] = child;
            i++;
        }
    }

    private void Update()
    {
        if (!WavesManager.gameIsBetweenWaves && _enemiesToSpawn > 0)
        {
            if (_enemiesToSpawn > 0)
            {
                _enemiesToSpawn--;
                StartCoroutine(SpawnEnemy());
            }
        }
        else if (_enemiesAlive == 0)
        {
            //Go to the next wave
            WavesManager.SetGameStatus(true);
        }
    }

    #endregion

    #region PrivateVariables

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(2.0f);

        var enemy = Instantiate(enemyPrefab);
        _enemies.Add(enemy);

        var enemyController = enemy.GetComponent<EnemyController>();
        enemyController.SetDestination(playerBase);
    }

    #endregion

    #region PublicMethods

    public void LaunchSpawns(int waveWeight)
    {
        _enemiesToSpawn = waveWeight;
        _enemiesAlive = _enemiesToSpawn;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        _enemiesAlive--;
        _enemies.Remove(enemy);
        Destroy(enemy);
    }

    #endregion
}