﻿using System.Collections;
using UnityEngine;

public class AttackUnit : Enemy
{
    #region PrivateVariables

    private bool _isAttacking;
    private bool _isDamageToBuildingsBoosted;

    #endregion

    #region ProtectedVariables

    [SerializeField] protected int damageToBuildings;

    protected float AttackTime;

    protected bool BetweenAttack;

    protected string TagToAttack = "Tower";

    protected GameObject Target;

    #endregion

    #region OverrideMethods

    protected override void Update()
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
    }

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

    protected override void FillEnemyData()
    {
        EnemyData._speedAttack = AttackTime;
        EnemyData._damageToTower = damageToBuildings;

        base.FillEnemyData();
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

    #region Getters

    public int GetDamageToBuildings()
    {
        return damageToBuildings;
    }

    public string GetTagToAttack()
    {
        return TagToAttack;
    }

    #endregion

    #region Getters

    public bool IsDamageToBuildingsBoosted()
    {
        return _isDamageToBuildingsBoosted;
    }

    #endregion

    #region Setters

    public void SetDamageToBuildings(int damage)
    {
        damageToBuildings = damage;
    }

    public void SetTarget(GameObject target)
    {
        Target = target;
    }

    public void SetIsDamageToBuildingsBoosted(bool boosted)
    {
        _isDamageToBuildingsBoosted = boosted;
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

    #endregion
}