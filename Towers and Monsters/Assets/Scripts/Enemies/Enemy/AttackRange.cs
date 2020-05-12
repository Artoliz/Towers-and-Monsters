using UnityEngine;

public class AttackRange : MonoBehaviour
{
    #region PrivateVariables

    //Variable used to optimize and not call what update do to many times
    private bool _updateIsDone;
    
    private GameObject _base;
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

    private void Start()
    {
        _base = _enemy.GetBase();
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

            //Reset update is done to be able to re-update the enemy
            _updateIsDone = false;
        }
    }

    private void Update()
    {
        if (!_tower && !_updateIsDone)
        {
            _enemy.SetTarget(_base);
            _enemy.SetBetweenAttack(false);
            _enemy.SetIsAttacking(false);
            _enemy.SetIsStopped(false);
            _enemy.SetAnimation(Attack, false);
            _enemy.SetAnimation(Run, true);

            //Update is done stop doing it until the enemy stops again
            _updateIsDone = true;
        }
    }

    #endregion
}