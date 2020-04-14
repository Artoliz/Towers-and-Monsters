using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    #region PrivateVariables

    private NavMeshAgent _agent;

    #endregion

    #region PublicVariables

    public GameObject destination;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _agent.SetDestination(destination.transform.position);
    }

    #endregion
}
