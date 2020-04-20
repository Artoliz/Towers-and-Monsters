using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region PrivateVariables

    private GameObject _base;

    private NavMeshAgent _agent;

    #endregion

    #region SerializableVariables

    [SerializeField] private int enemyWeight = 1;
    [SerializeField] private int waveNumberApparition = 1;
    [SerializeField] private int damageToBase = 100;

    [SerializeField] private float destinationReachedPadding = 1.5f;

    #endregion
    
    #region MonoBehavior

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (PauseMenu.gameIsPaused)
        {
            _agent.isStopped = true;
        }
        else if (HasReachedBase())
        {
            Base.instance.LoseHealth(damageToBase);
            DestroyEnemy();
        }
        else
        {
            _agent.isStopped = false;
            _agent.SetDestination(_base.transform.position);
        }
    }

    #endregion

    #region PrivateMethods

    private bool HasReachedBase()
    {
        var distanceToTarget = Vector3.Distance(transform.position, _base.transform.position);
        
        return distanceToTarget < destinationReachedPadding;
    }

    #endregion

    #region PublicMethods

    public int GetEnemyWaveNumberApparition()
    {
        return waveNumberApparition;
    }

    public int GetEnemyWeight()
    {
        return enemyWeight;
    }
    
    public void DestroyEnemy()
    {
        EnemiesSpawns.instance.RemoveEnemy(gameObject);
    }

    public void SetDestination(GameObject destination)
    {
        _base = destination;
    }

    #endregion
}