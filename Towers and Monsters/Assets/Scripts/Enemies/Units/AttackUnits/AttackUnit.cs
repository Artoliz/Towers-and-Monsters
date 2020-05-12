using System.Collections;
using UnityEngine;

public class AttackUnit : Enemy
{
    #region PrivateVariables

    private bool _isAttacking;

    #endregion

    #region ProtectedVariables

    [SerializeField] protected int damageToBuildings;

    protected float AttackTime;

    protected bool BetweenAttack;
    
    protected GameObject Target;

    #endregion
    
    #region OverrideMethods

    protected override void SetAnimationsTimes()
    {
        base.SetAnimationsTimes();
        
        var clips = Anim.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            switch (clip.name)
            {
                case "Attack":
                    AttackTime = clip.length;
                    break;
            }
        }
    }

    protected override void Update()
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
        else if (_isAttacking && Target && Target != Base)
        {
            if (!BetweenAttack)
                StartCoroutine(Attacking());
        }
        else
        {
            SetIsStopped(false);
            SetAnimation(Idle, false);
            SetAnimation(Run, true);
            Agent.SetDestination(Base.transform.position);
        }
    }

    #endregion

    #region VirtualMethods

    protected virtual IEnumerator Attacking()
    {
        //Override this method in inherited class.
        yield return null;
    }

    #endregion

    #region PublicMethods

    public void SetTarget(GameObject target)
    {
        Target = target;
    }

    public void SetIsAttacking(bool isAttacking)
    {
        _isAttacking = isAttacking;
    }
    
    public void SetBetweenAttack(bool isBetweenAttack)
    {
        BetweenAttack = isBetweenAttack;
    }

    #endregion
}
