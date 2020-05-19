using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawns : MonoBehaviour
{
    #region PrivateVariables

    private int _enemiesNumber;

    private readonly List<Transform> _spawns = new List<Transform>();

    private readonly List<GameObject> _enemiesToSpawn = new List<GameObject>();

    #endregion

    #region SerializableVariables

    [SerializeField] private float timeBeforeBeginToSpawn = 2.0f;
    [SerializeField] private float repeatTimeToSpawn = 2.0f;

    #endregion

    #region PublicVariables

    public GameObject[] enemiesPrefab;
    public GameObject playerBase;

    public static EnemiesSpawns Instance;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        Instance = this;

        foreach (Transform child in transform)
        {
            _spawns.Add(child);
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), timeBeforeBeginToSpawn, repeatTimeToSpawn);
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

    private void SpawnEnemy()
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused && !WavesManager.GameIsBetweenWaves &&
            _enemiesToSpawn.Count > 0)
        {
            var enemy = Instantiate(_enemiesToSpawn[0], PositionOfSpawn(), Quaternion.identity);

            var enemyController = enemy.GetComponent<Enemy>();
            enemyController.SetPlayerBase(playerBase);

            _enemiesToSpawn.Remove(_enemiesToSpawn[0]);
        }
    }

    private Vector3 PositionOfSpawn()
    {
        return _spawns[Random.Range(0, _spawns.Count)].position;
    }

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

    public List<Transform> GetEnemiesSpawns()
    {
        return _spawns;
    }

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
                _enemiesToSpawn.Add(enemyPrefab);
            }
        }

        _enemiesNumber = _enemiesToSpawn.Count;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        _enemiesNumber--;
        Destroy(enemy);
    }

    #endregion
}