using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region PrivateVariables

    private GameObject _base;

    private NavMeshAgent _agent;

    private Animator _anim;

    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Victory = Animator.StringToHash("Victory");

    #endregion

    #region SerializableVariables

    [SerializeField] private int enemyHp = 100;
    [SerializeField] private int enemyWeight = 1;
    [SerializeField] private int waveNumberApparition = 1;
    [SerializeField] private int damageToBase = 100;

    [SerializeField] private float destinationReachedPadding = 1.5f;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (PauseMenu.gameIsPaused)
        {
            _agent.isStopped = true;
            _anim.SetBool(Idle, true);
        }
        else if (Base.instance.IsBaseDestroyed())
        {
            _agent.isStopped = true;
            _anim.SetBool(Victory, true);
        }
        else if (enemyHp <= 0)
        {
            _agent.isStopped = true;
            gameObject.tag = "Dead";
            _anim.SetBool(Death, true);
            DestroyEnemy();
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
            _anim.SetBool(Run, true);
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

    public void Damage(int damage)
    {
        enemyHp -= damage;
    }

    #endregion
}