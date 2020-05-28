using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region PrivateVariables

    [SerializeField] private int enemyWeight = 1;
    [SerializeField] private int waveNumberApparition = 1;

    [SerializeField] private float destinationReachedPadding = 1.5f;
    
    [SerializeField] private GameObject effectOfDeath = null;

    private int _maxHp;

    private float _dyingTime;

    private bool _isSpeedBoosted;
    private bool _isMaxHpBoosted;
    private bool _isDamageToBaseBoosted;
    public bool _IsOnEffect;
    public bool _IsOnAOE;

    private static readonly int Death = Animator.StringToHash("Death");

    private GameObject _selected;

    #endregion

    #region ProtectedVariables

    [SerializeField] protected int reward;
    [SerializeField] protected int enemyHp;
    [SerializeField] protected int damageToBase;
    [SerializeField] protected float speed;

    protected GameObject Base;

    protected Animator Anim;

    protected Informations.EnemyData EnemyData;

    protected static readonly int Run = Animator.StringToHash("Run");
    protected static readonly int Idle = Animator.StringToHash("Idle");
    protected static readonly int Victory = Animator.StringToHash("Victory");

    protected Pathfinder _pathfinder;
    protected Stack<Pair<int, int>> _path;

    protected Pair<int, int> _nextNode;
    protected Pair<int, int> _lastNode;

    protected Vector3 _nextPos;

    #endregion

    #region MonoBehavior

    private void Start()
    {
        _maxHp = enemyHp;

        SetIsStopped(false);

        FillEnemyData();

        _pathfinder = new Pathfinder(Grid.Instance.GetGridSize().x, Grid.Instance.GetGridSize().y);
        _pathfinder.SetGrid(Grid.Instance._pathfinder.GetGrid());

        Vector2Int src = Grid.Instance.CalculatePositionInGrid(transform.position);
        Vector2Int dest = Grid.Instance.CalculatePositionInGrid(Base.transform.position);
        _pathfinder.SetDestination(dest.x, dest.y);
        _pathfinder.SetSource(src.x, src.y);

        _path = _pathfinder.Search();

        if (_path != null)
            _nextNode = _path.Pop();

        GameManager.Instance.AddEnemy(this);
    }

    private void OnMouseDown()
    {
        EnemyData._hp = enemyHp;

        if (EnemyData._hp < 0)
            EnemyData._hp = 0;

        Informations.Instance.SetInformations(EnemyData, this);
    }

    #endregion

    #region VirtualMethods

    protected virtual void Awake()
    {
        Anim = GetComponent<Animator>();
        SetAnimationsTimes();
    }

    protected virtual void Update()
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
            else
            {
                SetIsStopped(false);
                SetAnimation(Idle, false);
                SetAnimation(Run, true);

                if (_lastNode != null)
                {
                    _nextPos = Grid.Instance.CalculatePositionFromGrid(new Vector2Int(_lastNode.First, _lastNode.Second));
                    transform.position = Vector3.MoveTowards(transform.position, _nextPos, speed * Time.deltaTime);
                    transform.LookAt(_nextPos);
                    if (Vector3.Distance(transform.position, _nextPos) <= 0.1)
                        _lastNode = null;
                }
                else
                {
                    _nextPos = Grid.Instance.CalculatePositionFromGrid(new Vector2Int(_nextNode.First, _nextNode.Second));
                    transform.position = Vector3.MoveTowards(transform.position, _nextPos, speed * Time.deltaTime);
                    transform.LookAt(_nextPos);
                    if (Vector3.Distance(transform.position, _nextPos) <= 0.1)
                    {
                        if (_path != null && _path.Count > 0)
                            _nextNode = _path.Pop();
                    }
                }
            }
        }
    }

    protected virtual void SetAnimationsTimes()
    {
        var clips = Anim.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            switch (clip.name)
            {
                case "Dying":
                    _dyingTime = clip.length;
                    break;
            }
        }
    }

    protected virtual void FillEnemyData()
    {
        EnemyData._golds = reward;
        EnemyData._hp = enemyHp;
        EnemyData._speed = speed;
        EnemyData._damageToBase = damageToBase;
    }

    #endregion

    #region ProtectedMethods

    protected IEnumerator KillEnemy()
    {
        SetIsStopped(true);

        gameObject.tag = "Dead";
        SetAnimation(Death, true);

        GameManager.Instance.AddGolds(reward);

        if (effectOfDeath)
            Instantiate(effectOfDeath, transform.position, Quaternion.identity);
        
        yield return new WaitForSeconds(_dyingTime);
        GameManager.Instance._enemiesKilled += 1;
        DestroyEnemy();
    }

    protected bool HasReachedBase()
    {
        var distanceToTarget = Vector3.Distance(transform.position, Base.transform.position);

        return distanceToTarget < destinationReachedPadding;
    }

    #endregion

    #region PublicMethods

    #region Getters

    public bool GetOnEffectState()
    {
        return _IsOnEffect;
    }

    public bool GetOnAOEState()
    {
        return _IsOnAOE;
    }

    public Informations.EnemyData GetEnemyData()
    {
        return EnemyData;
    }

    public GameObject GetBase()
    {
        return Base;
    }

    public int GetEnemyWaveNumberApparition()
    {
        return waveNumberApparition;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public int GetMaxHp()
    {
        return _maxHp;
    }

    public int GetEnemyHp()
    {
        return enemyHp;
    }

    public int GetDamageToBase()
    {
        return damageToBase;
    }

    public int GetEnemyWeight()
    {
        return enemyWeight;
    }

    public bool IsSpeedBoosted()
    {
        return _isSpeedBoosted;
    }

    public bool IsMaxHpBoosted()
    {
        return _isMaxHpBoosted;
    }

    public bool IsDamageToBaseBoosted()
    {
        return _isDamageToBaseBoosted;
    }

    #endregion

    #region Setters


    public void SetOnEffect(bool newEffectState)
    {
        _IsOnEffect = newEffectState;
    }

    public void SetOnAOE(bool newAOEState)
    {
        _IsOnAOE = newAOEState;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetMaxHp(int maxHp)
    {
        _maxHp = maxHp;
    }

    public void SetEnemyHp(int hp)
    {
        enemyHp = hp;
    }

    public void SetDamageToBase(int damage)
    {
        damageToBase = damage;
    }

    public void SetPlayerBase(GameObject playerBase)
    {
        Base = playerBase;
    }

    public void SetAnimation(int trigger, bool activate)
    {
        Anim.SetBool(trigger, activate);
    }

    public void SetIsStopped(bool isStopped)
    {
        //Agent.isStopped = isStopped;
    }

    public void SetIsSpeedBoosted(bool boosted)
    {
        _isSpeedBoosted = boosted;
    }

    public void SetIsMaxHpBoosted(bool boosted)
    {
        _isMaxHpBoosted = boosted;
    }

    public void SetIsDamageToBaseBoosted(bool boosted)
    {
        _isDamageToBaseBoosted = boosted;
    }

    #endregion

    public void ReDrawPathFinding()
    {
        _lastNode = _nextNode;

        _pathfinder.SetGrid(Grid.Instance._pathfinder.GetGrid());

        Vector2Int src = Grid.Instance.CalculatePositionInGrid(transform.position);
        Vector2Int dest = Grid.Instance.CalculatePositionInGrid(Base.transform.position);
        _pathfinder.SetDestination(dest.x, dest.y);
        _pathfinder.SetSource(src.x, src.y);

        _path = _pathfinder.Search();

        if (_path != null)
        {
            _path.Pop();
            _nextNode = _path.Pop();
        }
    }

    public void DestroyEnemy()
    {
        if (_selected != null)
            Informations.Instance.ResetSelected();
        GameManager.Instance.RemoveEnemy(this);
        SpawnManager.Instance.RemoveEnemy(gameObject);
    }

    public void Damage(int damage)
    {
        enemyHp -= damage;

        if (enemyHp >= 0)
            EnemyData._hp = enemyHp;

        if (_selected != null)
            Informations.Instance.SetInformations(EnemyData, this);
    }

    public void IsSelected(GameObject selected)
    {
        if (selected == null)
        {
            Destroy(_selected);
            _selected = null;
        }
        else
            _selected = Instantiate(selected, this.transform);
    }

    #endregion
}