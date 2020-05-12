using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region PrivateVariables

    private float _dyingTime;
    private float _attackTime;

    private bool _isAttacking;
    private bool _betweenAttack;

    private GameObject _base;
    private GameObject _target;

    private NavMeshAgent _agent;

    private Animator _anim;

    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Death = Animator.StringToHash("Death");
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

    private void Start()
    {
        _agent.isStopped = false;
        _agent.SetDestination(_base.transform.position);
        _anim.SetBool(Run, true);
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
        else if (_isAttacking && _target && _target != _base)
        {
            if (!_betweenAttack)
            {
                StartCoroutine(shootElement ? Shooting() : Attacking());
            }
        }
        else
        {
            _agent.SetDestination(_base.transform.position);
        }
    }

    #endregion

    #region PrivateMethods

    private void SetAnimationsTimes()
    {
        var clips = _anim.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            switch (clip.name)
            {
                case "Dying":
                    _dyingTime = clip.length;
                    break;
                case "Attack":
                    _attackTime = clip.length;
                    break;
            }
        }
    }

    private bool HasReachedBase()
    {
        var distanceToTarget = Vector3.Distance(transform.position, _base.transform.position);

        return distanceToTarget < destinationReachedPadding;
    }

    private IEnumerator Attacking()
    {
        _betweenAttack = true;

        var look = _target.transform.position;
        look.y = 0;
        transform.LookAt(look);

        yield return new WaitForSeconds(_attackTime);
        if (_target && _target != _base)
        {
            _target.GetComponentInParent<Tower>().Damage(damageToBuildings);
            _betweenAttack = false;
        }
    }

    private IEnumerator Shooting()
    {
        _betweenAttack = true;

        var look = _target.transform.position;
        look.y = 0;
        transform.LookAt(look);

        yield return new WaitForSeconds(_attackTime / 2);
        if (_target && _target != _base)
        {
            var bullet = Instantiate(bulletPrefab, shootElement.position, Quaternion.identity);
            bullet.GetComponent<EnemyBullet>().SetTarget(_target.transform);
            bullet.GetComponent<EnemyBullet>().SetDamage(damageToBuildings);

            yield return new WaitForSeconds(_attackTime / 2);
            _betweenAttack = false;
        }
    }

    #endregion

    #region PublicMethods

    #region Getters

    public int GetEnemyWaveNumberApparition()
    {
        return waveNumberApparition;
    }

    public int GetEnemyWeight()
    {
        return enemyWeight;
    }

    public GameObject GetBase()
    {
        return _base;
    }

    #endregion

    #region Setters

    public void SetPlayerBase(GameObject playerBase)
    {
        _base = playerBase;
        _target = _base;
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    public void SetIsAttacking(bool isAttacking)
    {
        _isAttacking = isAttacking;
    }

    public void SetBetweenAttack(bool isBetweenAttack)
    {
        _betweenAttack = isBetweenAttack;
    }

    public void SetIsStopped(bool isStopped)
    {
        _agent.isStopped = isStopped;
    }

    public void SetAnimation(int trigger, bool activate)
    {
        _anim.SetBool(trigger, activate);
    }

    #endregion

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

    public void Damage(int damage)
    {
        enemyHp -= damage;
    }

    #endregion
}