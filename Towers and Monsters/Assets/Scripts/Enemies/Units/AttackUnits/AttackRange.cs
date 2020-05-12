using UnityEngine;

public class AttackRange : MonoBehaviour
{
    #region PrivateVariables

    //Variable used to optimize and not call what update do to many times
    private bool _updateIsDone;
    
    private GameObject _base;
    private GameObject _tower;

    private AttackUnit _attackUnit;

    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Attack = Animator.StringToHash("Attack");

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        _attackUnit = GetComponentInParent<AttackUnit>();
    }

    private void Start()
    {
        _base = _attackUnit.GetBase();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            _tower = other.gameObject;

            _attackUnit.SetTarget(_tower);
            _attackUnit.SetIsAttacking(true);
            _attackUnit.SetIsStopped(true);
            _attackUnit.SetAnimation(Run, false);
            _attackUnit.SetAnimation(Attack, true);

            //Reset update is done to be able to re-update the enemy
            _updateIsDone = false;
        }
    }

    private void Update()
    {
        if (!_tower && !_updateIsDone)
        {
            _attackUnit.SetTarget(_base);
            _attackUnit.SetBetweenAttack(false);
            _attackUnit.SetIsAttacking(false);
            _attackUnit.SetIsStopped(false);
            _attackUnit.SetAnimation(Attack, false);
            _attackUnit.SetAnimation(Run, true);

            //Update is done stop doing it until the enemy stops again
            _updateIsDone = true;
        }
    }

    #endregion
}