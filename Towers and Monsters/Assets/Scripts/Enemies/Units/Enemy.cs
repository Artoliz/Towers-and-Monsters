using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region PrivateVariables

    [SerializeField] private int enemyWeight = 1;
    [SerializeField] private int waveNumberApparition = 1;

    [SerializeField] private float destinationReachedPadding = 1.5f;

    private int _reward;

    private float _dyingTime;

    private bool _isBoosted;

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
        else if (enemyHp <= 0 && !gameObject.CompareTag($"Dead"))
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

    public int GetEnemyWaveNumberApparition()
    {
        return waveNumberApparition;
    }

    public int GetEnemyWeight()
    {
        return enemyWeight;
    }

    public bool IsBoosted()
    {
        return _isBoosted;
    }
    
    public GameObject GetBase()
    {
        return Base;
    }

    #endregion

    #region Setters

    public void SetPlayerBase(GameObject playerBase)
    {
        Base = playerBase;
    }

    public void SetIsStopped(bool isStopped)
    {
        Agent.isStopped = isStopped;
    }

    public void SetIsBoosted(bool isBoosted)
    {
        _isBoosted = isBoosted;
    }
    
    public void SetAnimation(int trigger, bool activate)
    {
        Anim.SetBool(trigger, activate);
    }

    #endregion

    public IEnumerator KillEnemy()
    {
        SetIsStopped(true);

        gameObject.tag = "Dead";
        SetAnimation(Death, true);

        GameManager.Instance.AddGolds(_reward);

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