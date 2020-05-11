using UnityEngine;

public class AttackRange : MonoBehaviour
{

    #region PrivateVariables

    private GameObject _tower;
    
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
            _tower = other.gameObject;
            
            _enemy.SetTarget(_tower);
            _enemy.SetIsAttacking(true);
            _enemy.SetIsStopped(true);
            _enemy.SetAnimation(Run, false);
            _enemy.SetAnimation(Attack, true);
        }
    }

    private void Update()
    {
        if (!_tower)
        {
            _enemy.SetIsAttacking(false);
            _enemy.SetAnimation(Attack, false);
        }
    }

    #endregion
}
