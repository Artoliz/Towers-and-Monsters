using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    #region PrivateVariables

    [SerializeField] private int damageToBase;
    
    [SerializeField] private float destinationReachedPadding;

    private GameObject _base;

    private NavMeshAgent _agent;

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