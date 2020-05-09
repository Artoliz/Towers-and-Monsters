using UnityEngine;

public class AttackRange : MonoBehaviour
{

    #region PrivateVariables

    private Enemy _enemy;
    
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Attack = Animator.StringToHash("Attack");

    #endregion
    
    #region MonoBehavior

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            _enemy.SetTarget(other.gameObject);
            _enemy.SetIsAttacking(true);
            _enemy.SetIsStopped(true);
            _enemy.SetAnimation(Run, false);
            _enemy.SetAnimation(Attack, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == _enemy.GetTarget().transform)
        {
            _enemy.SetIsAttacking(false);
            _enemy.SetAnimation(Attack, false);
        }
    }

    #endregion
}
