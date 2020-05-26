using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region PrivateVariables

    private int _enemiesNumber;

    private readonly List<EnemySpawn> _enemiesSpawns = new List<EnemySpawn>();

    [SerializeField] private GameObject playerBase = null;

    #endregion

    #region PublicVariables

    public GameObject[] enemiesPrefab;

    public static SpawnManager Instance;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        Instance = this;

        foreach (Transform child in transform)
        {
            EnemySpawn enemySpawn;
            if ((enemySpawn = child.gameObject.GetComponent<EnemySpawn>()) != null)
            {
                enemySpawn.SetPlayerBase(playerBase);
                _enemiesSpawns.Add(enemySpawn);
            }
        }
    }

    private void Update()
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused && !WavesManager.GameIsBetweenWaves)
        {
            if (_enemiesNumber == 0)
            {
                //Go to the next wave
                WavesManager.SetGameStatus(true);
            }
        }
    }

    #endregion

    #region PrivateVariables

    private List<GameObject> GetEnemiesPossibleInWave(int waveNumber)
    {
        var possibleEnemies = new List<GameObject>();

        foreach (var enemyPrefab in enemiesPrefab)
        {
            var enemy = enemyPrefab.GetComponent<Enemy>();

            if (enemy.GetEnemyWaveNumberApparition() <= waveNumber)
                possibleEnemies.Add(enemyPrefab);
        }

        return possibleEnemies;
    }

    #endregion

    #region PublicMethods

    public void LaunchSpawns(int waveWeight, int waveNumber)
    {
        var possibleEnemies = GetEnemiesPossibleInWave(waveNumber);

        while (waveWeight > 0)
        {
            var enemyPrefab = possibleEnemies[Random.Range(0, possibleEnemies.Count)];
            var enemy = enemyPrefab.GetComponent<Enemy>();
            var enemyWeight = enemy.GetEnemyWeight();

            if (enemyWeight > waveWeight)
            {
                possibleEnemies.Remove(enemyPrefab);
            }
            else
            {
                waveWeight -= enemyWeight;
                _enemiesNumber++;
                _enemiesSpawns[Random.Range(0, _enemiesSpawns.Count)].AddEnemyToSpawn(enemyPrefab);
            }
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        _enemiesNumber--;
        Destroy(enemy);
    }

    #endregion
}