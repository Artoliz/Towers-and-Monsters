using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region PrivateVariables

    [SerializeField] private int enemyWeight = 1;
    [SerializeField] private int waveNumberApparition = 1;

    [SerializeField] private float destinationReachedPadding = 1.5f;

    private float _dyingTime;

    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Death = Animator.StringToHash("Death");

    #endregion

    #region ProtectedVariables

    [SerializeField] protected int enemyHp = 100;
    [SerializeField] protected int damageToBase = 100;

    protected GameObject Base;
    
    protected NavMeshAgent Agent;

    protected Animator Anim;

    protected static readonly int Idle = Animator.StringToHash("Idle");
    protected static readonly int Victory = Animator.StringToHash("Victory");

    #endregion
    
    #region MonoBehavior

    private void Awake()
    {
        Anim = GetComponent<Animator>();
        SetAnimationsTimes();

        Agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        SetIsStopped(false);
        Agent.SetDestination(Base.transform.position);
        SetAnimation(Run, true);
    }

    #endregion

    #region VirtualMethods

    protected virtual void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            SetIsStopped(true);
            SetAnimation(Idle, true);
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