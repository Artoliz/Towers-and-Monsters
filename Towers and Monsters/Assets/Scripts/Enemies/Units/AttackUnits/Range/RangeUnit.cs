﻿using System.Collections;
using UnityEngine;

public class RangeUnit : AttackUnit
{
    #region PrivateVariables

    [SerializeField] private Transform shootElement;
    [SerializeField] private GameObject bulletPrefab;

    #endregion

    #region PrivateMethods

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, shootElement.position, Quaternion.identity);
        bullet.GetComponent<RangeUnitBullet>().SetTarget(Target.transform);
        bullet.GetComponent<RangeUnitBullet>().SetDamage(damageToBuildings);
    }

    #endregion

    #region OverrideMethods

    protected override IEnumerator Attacking()
    {
        BetweenAttack = true;

        var look = Target.transform.position;
        look.y = 0;
        transform.LookAt(look);

        yield return new WaitForSeconds(AttackTime / 2);
        if (Target && Target != Base)
        {
            Shoot();

            yield return new WaitForSeconds(AttackTime / 2);
            BetweenAttack = false;
        }
    }

    #endregion
}