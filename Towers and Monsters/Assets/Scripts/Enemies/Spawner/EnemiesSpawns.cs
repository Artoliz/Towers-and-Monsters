using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesSpawns : MonoBehaviour
{
    #region PrivateVariables

    private int _enemiesNumber;

    private Transform[] _spawns;

    private List<EnemyController> _enemies;

    #endregion

    #region PublicVariables

    public GameObject enemyPrefab;
    public GameObject playerBase;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        _enemies = new List<EnemyController>();
        
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
        if (!WavesManager.gameIsBetweenWaves && _enemiesNumber > 0)
        {
            /*foreach (var spawn in _spawns)
            {
                if (_enemiesNumber > 0)
                {
                    _enemiesNumber--;
                    StartCoroutine(SpawnEnemy());
                }
            }*/
        }
        else if (EnemiesAllArrived())
        {
            //Wave is finished, no more enemies
            WavesManager.SetGameStatus(true);
        }
    }

    #endregion

    #region PrivateVariables

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(2.0f);
        
        var obj = Instantiate(enemyPrefab);
        
        var enemy = obj.GetComponent<EnemyController>();
        enemy.SetDestination(playerBase);
        _enemies.Add(enemy);
    }

    private bool EnemiesAllArrived()
    {
        foreach (var enemy in _enemies)
        {
            //Faire le reached destination
        }
        
        return false;
    }
    
    #endregion
    
    #region PublicMethods

    public void LaunchSpawns(int waveWeight)
    {
        _enemiesNumber = waveWeight;
    }

    #endregion
}