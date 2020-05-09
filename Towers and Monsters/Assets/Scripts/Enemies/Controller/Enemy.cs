using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region PrivateVariables

    private float _dyingTime;

    private bool _isAttacking;

    private GameObject _base;
    private GameObject _target;

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

    #region PublicVariables

    public int damageToBuildings;
    
    public GameObject bulletPrefab;

    public Transform shootElement;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        SetAnimationsTimes();
        
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (PauseMenu.GameIsPaused)
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
            StartCoroutine(KillEnemy());
        }
        else if (HasReachedBase())
        {
            Base.instance.LoseHealth(damageToBase);
            DestroyEnemy();
        }
        else if (_isAttacking)
        {
            Attacking();
        }
        else
        {
            _agent.isStopped = false;
            _agent.SetDestination(_target.transform.position);
            _anim.SetBool(Run, true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            _target = other.gameObject;
            _isAttacking = true;
            _agent.isStopped = true;
            _anim.SetBool(Attack, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == _target.transform)
        {
            _isAttacking = false;
        }
    }

    #endregion

    #region PrivateMethods

    private void SetAnimationsTimes()
    {
        var clips = _anim.runtimeAnimatorController.animationClips;
        foreach(var clip in clips)
        {
            switch(clip.name)
            {
                case "Dying":
                    _dyingTime = clip.length;
                    break;
            }
        }
    }
    
    private bool HasReachedBase()
    {
        var distanceToTarget = Vector3.Distance(transform.position, _base.transform.position);

        return distanceToTarget < destinationReachedPadding;
    }

    private void Attacking()
    {
        _target.GetComponent<TowerHP>().Damage(damageToBuildings);
    }

    private void Shooting()
    {
        var bullet = Instantiate(bulletPrefab, shootElement.position, Quaternion.identity);
        bullet.GetComponent<EnemyBullet>().SetTarget(_target.transform);
        bullet.GetComponent<EnemyBullet>().SetDamage(damageToBuildings);
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

    public IEnumerator KillEnemy()
    {
        _agent.isStopped = true;
        gameObject.tag = "Dead";
        _anim.SetBool(Death, true);
        yield return new WaitForSeconds(_dyingTime);
        DestroyEnemy();
    }

    public void DestroyEnemy()
    {
        EnemiesSpawns.Instance.RemoveEnemy(gameObject);
    }

    public void SetPlayerBase(GameObject playerBase)
    {
        _base = playerBase;
        _target = _base;
    }

    public void Damage(int damage)
    {
        enemyHp -= damage;
    }

    #endregion
}