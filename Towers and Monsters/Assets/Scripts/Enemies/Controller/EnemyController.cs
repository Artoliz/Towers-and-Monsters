using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    #region PrivateVariables

    private GameObject _destination;
    
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
        else
        {
            _agent.isStopped = false;
            _agent.SetDestination(_destination.transform.position);
        }
    }

    #endregion

    #region PublicMethods

    public NavMeshAgent GetAgent()
    {
        return _agent;
    }
    
    public void SetDestination(GameObject destination)
    {
        _destination = destination;
    }

    #endregion
}
