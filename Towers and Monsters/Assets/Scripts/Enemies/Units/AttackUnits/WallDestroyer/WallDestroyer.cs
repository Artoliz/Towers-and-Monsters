using System.Collections;
using UnityEngine;

public class WallDestroyer : AttackUnit
{
    #region OverrideMethods

    protected override void Awake()
    {
        TagToAttack = "Wall";

        base.Awake();
    }

    protected override IEnumerator Attacking()
    {
        BetweenAttack = true;

        var look = Target.transform.position;
        look.y = 0;
        transform.LookAt(look);

        yield return new WaitForSeconds(AttackTime / 2);
        if (Target && Target != Base)
        {
            Target.GetComponent<Wall>().Damage(damageToBuildings);

            yield return new WaitForSeconds(AttackTime / 2);
            BetweenAttack = false;
        }
    }

    #endregion
}