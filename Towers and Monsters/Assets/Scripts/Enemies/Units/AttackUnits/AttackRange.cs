using UnityEngine;

public class AttackRange : MonoBehaviour
{
    #region PrivateVariables

    //Variable used to optimize and not call what update do to many times
    private bool _updateIsDone;

    private string _tagToAttack;

    private GameObject _base;
    private GameObject _building;

    private AttackUnit _attackUnit;

    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Attack = Animator.StringToHash("Attack");

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        _attackUnit = GetComponentInParent<AttackUnit>();
        _tagToAttack = _attackUnit.GetTagToAttack();
    }

    private void Start()
    {
        _base = _attackUnit.GetBase();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!WavesManager.gameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (other.CompareTag(_tagToAttack))
            {
                _building = other.gameObject;

                _attackUnit.SetTarget(_building);
                _attackUnit.SetIsAttacking(true);
                _attackUnit.SetIsStopped(true);
                _attackUnit.SetAnimation(Run, false);
                _attackUnit.SetAnimation(Attack, true);

                //Reset update is done to be able to re-update the enemy
                _updateIsDone = false;
            }
        }
    }

    private void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            _attackUnit.SetAnimation(Attack, false);
            _attackUnit.SetAnimation(Idle, true);
        }
        if (!WavesManager.gameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (!_building && !_updateIsDone)
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
            else if (_building)
            {
                _attackUnit.SetAnimation(Attack, true);
                _attackUnit.SetAnimation(Idle, false);
            }
        }
    }

    #endregion
}