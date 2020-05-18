﻿using UnityEngine;

public class EffectOfDeath : MonoBehaviour
{
    #region SerializedVariables

    private Collider _enemyCollider;

    private Enemy _enemy;

    [SerializeField] private bool boostSpeed;
    [SerializeField] private bool boostMaxHp;
    [SerializeField] private bool boostDamageToBase;
    [SerializeField] private bool boostDamageToBuildings;

    [SerializeField] private float eodDuration;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        Destroy(gameObject, eodDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _enemyCollider = other;
            _enemy = _enemyCollider.GetComponent<Enemy>();

            if (boostSpeed && !_enemy.IsSpeedBoosted())
            {
                _enemy.SetIsSpeedBoosted(true);
                BoostSpeed();
            }
            else if (boostMaxHp && !_enemy.IsMaxHpBoosted())
            {
                _enemy.SetIsMaxHpBoosted(true);
                BoostMaxHp();
            }
            else if (boostDamageToBase && !_enemy.IsDamageToBaseBoosted())
            {
                _enemy.SetIsDamageToBaseBoosted(true);
                BoostDamageToBase();
            }
            else if (boostDamageToBuildings)
            {
                BoostDamageToBuildings();
            }
        }
    }

    #endregion

    #region PrivateMethods

    private void BoostSpeed()
    {
        var speed = _enemy.GetSpeed();

        speed += (0.2f * speed);
        _enemy.SetSpeed(speed);
    }

    private void BoostMaxHp()
    {
        var maxHp = _enemy.GetMaxHp();
        var enemyHp = _enemy.GetEnemyHp();

        var hpToAdd = (0.1f * maxHp);

        var newMaxHp = hpToAdd + maxHp;
        _enemy.SetMaxHp(Mathf.RoundToInt(newMaxHp));

        var newEnemyHp = hpToAdd + enemyHp;
        _enemy.SetEnemyHp(Mathf.RoundToInt(newEnemyHp));
    }

    private void BoostDamageToBase()
    {
        var damageToBase = _enemy.GetDamageToBase();

        var newDamageToBase = (0.1f * damageToBase) + damageToBase;
        _enemy.SetDamageToBase(Mathf.RoundToInt(newDamageToBase));
    }

    private void BoostDamageToBuildings()
    {
        var attackUnit = _enemyCollider.GetComponent<AttackUnit>();
        if (attackUnit)
        {
            attackUnit.SetIsDamageToBuildingsBoosted(true);
            var damageToBuildings = attackUnit.GetDamageToBuildings();

            var newDamageToBuildings = (0.1f * damageToBuildings) + damageToBuildings;
            attackUnit.SetDamageToBuildings(Mathf.RoundToInt(newDamageToBuildings));
        }
    }

    #endregion
}