using UnityEngine;

public class BoostDamageToBuildings : EffectOfDeath
{
    #region SerializedVariables

    private AttackUnit _attackUnit;
    
    [SerializeField] private float boostDamageToBuildings;
    
    #endregion

    #region OverrideMethods

    protected override void BoostEnemy()
    {
        _attackUnit = EnemyCollider.GetComponent<AttackUnit>();
        if (_attackUnit)
        {
            var newDamage = boostDamageToBuildings * _attackUnit.GetDamageToBuildings();
            _attackUnit.SetDamageToBuildings(Mathf.RoundToInt(newDamage));
        }
    }

    #endregion
}
