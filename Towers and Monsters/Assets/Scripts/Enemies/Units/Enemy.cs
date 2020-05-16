using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region PrivateVariables

    [SerializeField] private int enemyWeight = 1;
    [SerializeField] private int waveNumberApparition = 1;

    [SerializeField] private float destinationReachedPadding = 1.5f;
    
    [SerializeField] private GameObject effectOfDeath;

    private int _reward;
    private int _maxHp;

    private float _dyingTime;

    private bool _isSpeedBoosted;
    private bool _isMaxHpBoosted;
    private bool _isDamageToBaseBoosted;

    private static readonly int Death = Animator.StringToHash("Death");

    #endregion

    #region ProtectedVariables

    [SerializeField] protected int enemyHp = 100;
    [SerializeField] protected int damageToBase = 100;

    protected GameObject Base;

    protected NavMeshAgent Agent;

    protected Animator Anim;

    protected Informations.EnemyData EnemyData;

    protected static readonly int Run = Animator.StringToHash("Run");
    protected static readonly int Idle = Animator.StringToHash("Idle");
    protected static readonly int Victory = Animator.StringToHash("Victory");

    #endregion

    #region MonoBehavior

    private void Start()
    {
        _maxHp = enemyHp;

        SetIsStopped(false);
        Agent.SetDestination(Base.transform.position);

        FillEnemyData();
    }

    #endregion

    #region VirtualMethods

    protected virtual void Awake()
    {
        _reward = enemyWeight * 10;

        Anim = GetComponent<Animator>();
        SetAnimationsTimes();

        Agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        if (!gameObject.CompareTag($"Dead"))
        {
            if (PauseMenu.GameIsPaused)
            {
                SetIsStopped(true);
                SetAnimation(Idle, true);
                SetAnimation(Run, false);
            }
            else if (global::Base.instance.IsBaseDestroyed())
            {
                SetIsStopped(true);
                SetAnimation(Victory, true);
            }
            else if (enemyHp <= 0)
            {
                StartCoroutine(KillEnemy());
            }
            else if (HasReachedBase())
            {
                global::Base.instance.LoseHealth(damageToBase);
                DestroyEnemy();
            }
            else
            {
                SetIsStopped(false);
                SetAnimation(Idle, false);
                SetAnimation(Run, true);
                Agent.SetDestination(Base.transform.position);
            }
        }
    }

    protected virtual void SetAnimationsTimes()
    {
        var clips = Anim.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            switch (clip.name)
            {
                case "Dying":
                    _dyingTime = clip.length;
                    break;
            }
        }
    }

    protected virtual void FillEnemyData()
    {
        EnemyData._golds = _reward;
        EnemyData._hp = enemyHp;
        EnemyData._speed = Agent.speed;
        EnemyData._damageToBase = damageToBase;
    }

    #endregion

    #region ProtectedMethods

    protected bool HasReachedBase()
    {
        var distanceToTarget = Vector3.Distance(transform.position, Base.transform.position);

        return distanceToTarget < destinationReachedPadding;
    }

    #endregion

    #region PublicMethods

    #region Getters

    public Informations.EnemyData GetEnemyData()
    {
        return EnemyData;
    }

    public GameObject GetBase()
    {
        return Base;
    }

    public int GetEnemyWaveNumberApparition()
    {
        return waveNumberApparition;
    }

    public float GetSpeed()
    {
        return Agent.speed;
    }

    public int GetMaxHp()
    {
        return _maxHp;
    }

    public int GetEnemyHp()
    {
        return enemyHp;
    }

    public int GetDamageToBase()
    {
        return damageToBase;
    }

    public int GetEnemyWeight()
    {
        return enemyWeight;
    }

    public bool IsSpeedBoosted()
    {
        return _isSpeedBoosted;
    }

    public bool IsMaxHpBoosted()
    {
        return _isMaxHpBoosted;
    }

    public bool IsDamageToBaseBoosted()
    {
        return _isDamageToBaseBoosted;
    }

    #endregion

    #region Setters

    public void SetSpeed(float speed)
    {
        Agent.speed = speed;
    }

    public void SetMaxHp(int maxHp)
    {
        _maxHp = maxHp;
    }

    public void SetEnemyHp(int hp)
    {
        enemyHp = hp;
    }

    public void SetDamageToBase(int damage)
    {
        damageToBase = damage;
    }

    public void SetPlayerBase(GameObject playerBase)
    {
        Base = playerBase;
    }

    public void SetAnimation(int trigger, bool activate)
    {
        Anim.SetBool(trigger, activate);
    }

    public void SetIsStopped(bool isStopped)
    {
        Agent.isStopped = isStopped;
    }

    public void SetIsSpeedBoosted(bool boosted)
    {
        _isSpeedBoosted = boosted;
    }

    public void SetIsMaxHpBoosted(bool boosted)
    {
        _isMaxHpBoosted = boosted;
    }

    public void SetIsDamageToBaseBoosted(bool boosted)
    {
        _isDamageToBaseBoosted = boosted;
    }

    #endregion

    public IEnumerator KillEnemy()
    {
        SetIsStopped(true);

        gameObject.tag = "Dead";
        SetAnimation(Death, true);

        GameManager.Instance.AddGolds(_reward);

        if (effectOfDeath)
            Instantiate(effectOfDeath, transform.position, Quaternion.identity);
        
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