using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    #region PrivateVariables

    private float _homeY;

    private Camera m_MainCamera;

    private float _maxSizeX;

    private bool _isShoot;

    private Vector3 _size;

    private bool _isDamaged;
    
    public enum towerType {bullet, effect, aoe};

    private Vector3 _particleExplosionPosition;

    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Pose = Animator.StringToHash("T_pose");

    private GameObject _selected;

    #endregion

    #region SerializableVariables

    [SerializeField] private Transform health;
    [SerializeField] private Transform progress;

    #endregion

    #region PublicVariables

    public towerType type;

    public int cost;

    public int hp;

    public int maxHp;

    public int dmg;

    public int upgradeCost;

    public int repairCost;

    public int effectDammage;

    public int sell;

    public float shootDelay;

    public bool catcher;

    public GameObject bullet;
    public GameObject destroyParticle;
    public GameObject healthBar;

    public Transform target;
    public Transform lookAtObj;
    public Transform shootElement;
    public GameObject upgradePrefab;
    public Animator anim;

    public Informations.TowerData TowerData;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        var localScale = progress.transform.localScale;

        healthBar = transform.Find("Health").gameObject;

        _particleExplosionPosition = transform.position;
        _particleExplosionPosition.y = 1;
        _isDamaged = false;

        _maxSizeX = localScale.x;
        _size = localScale;

        anim = GetComponent<Animator>();

        //Temporary assignement
        lookAtObj = gameObject.transform;

        _homeY = lookAtObj.transform.localRotation.eulerAngles.y;
        SetTowerData();
    }

    private void Update()
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (target)
            {
                var dir = target.transform.position - lookAtObj.transform.position;
                dir.y = 0;
                var rot = Quaternion.LookRotation(dir);
                lookAtObj.transform.rotation = Quaternion.Slerp(lookAtObj.transform.rotation, rot, 5 * Time.deltaTime);
            }
            else
            {
                var home = new Quaternion(0, _homeY, 0, 1);

                lookAtObj.transform.rotation = Quaternion.Slerp(lookAtObj.transform.rotation, home, Time.deltaTime);
            }

            if (!_isShoot && type != towerType.aoe)
            {
                StartCoroutine(Shoot());
            }

            if (catcher)
            {
                if (!target || target.CompareTag("Dead"))
                {
                    StopCatcherAttack();
                }
            }

            if (_isDamaged == false) 
            {
               healthBar.SetActive(false);
            }

            if (_isDamaged == true) 
            {
                healthBar.SetActive(true);
                health.LookAt(Camera.main.transform);
            }

            if (hp <= 0)
                Destroy();
        }
    }

    private void OnMouseDown()
    {
        TowerData._hp = this.hp;

        if (TowerData._hp < 0)
            TowerData._hp = 0;

        SetTowerData();
        Informations.Instance.SetInformations(TowerData, this);
    }

    #endregion

    #region PrivateMethods

    private void GetDamage()
    {
        if (target)
        {
            target.GetComponent<Enemy>().Damage(dmg);
        }
    }

    private IEnumerator Shoot()
    {
        _isShoot = true;
        yield return new WaitForSeconds(shootDelay);

        if (target && catcher == false)
        {
            GameObject b = Instantiate(bullet, shootElement.position, Quaternion.identity);
            b.GetComponent<TowerBullet>().target = target;
            b.GetComponent<TowerBullet>().twr = this;
        }

        if (target && catcher)
        {
            anim.SetBool(Attack, true);
            anim.SetBool(Pose, false);
        }

        _isShoot = false;
    }

    private void StopCatcherAttack()
    {
        target = null;

        anim.SetBool(Attack, false);
        anim.SetBool(Pose, true);
    }

    private void SetTowerData()
    {
        TowerData._hp = this.hp;
        TowerData._maxHp = this.maxHp;
        TowerData._repair = this.repairCost;
        TowerData._upgrade = this.upgradeCost;
        TowerData._damageToEnemy = this.dmg;
        TowerData._speedAttack = this.shootDelay;
        TowerData._damageEffect = this.effectDammage;
        TowerData._sell = (this.cost / 2) * (this.hp / this.maxHp);
        TowerData._type = type;
        TowerData._isUpgrade = true;
        if (upgradePrefab == null)
            TowerData._isUpgrade = false;
    }

    #endregion

    #region PublicMethods

    public void Upgrade()
    {
        if (upgradePrefab != null)
        {
            if (this.upgradeCost <= GameManager.Instance.GetGolds())
            {
                GameManager.Instance.RemoveGolds(this.upgradeCost);
                GameObject tmp = Instantiate(upgradePrefab, transform.position, transform.rotation);
                if (_selected != null)
                    Informations.Instance.ResetSelected();
                tmp.GetComponent<Tower>().SetTowerData();
                Informations.Instance.SetInformations(tmp.GetComponent<Tower>().TowerData, tmp.GetComponent<Tower>());
                Destroy(this.gameObject);
            }
            else
                StartCoroutine(GameManager.DisplayError("Not enough golds !"));
        }
    }

    public void Destroy()
    {
        GameManager.Instance.AddGolds((this.cost / 2) * (this.hp / this.maxHp));
        Informations.Instance.ResetSelected();
        Vector2Int posInGrid = Grid.Instance.CalculatePositionInGrid(this.transform.position);
        Grid.Instance._pathfinder.RemoveBlockedPosition(posInGrid.x, posInGrid.y);
        Grid.Instance.RemoveElementInGrid(this.transform.position);
        if (_selected != null)
            Informations.Instance.ResetSelected();
        Destroy(this.gameObject);
        destroyParticle = Instantiate(destroyParticle, _particleExplosionPosition, Quaternion.FromToRotation(Vector3.up, Vector3.zero));
        Destroy(destroyParticle, 1);
    }

    public void Repair()
    {
        if (hp < maxHp)
        {
            if (this.repairCost <= GameManager.Instance.GetGolds())
            {
                GameManager.Instance.RemoveGolds(this.repairCost);
                hp = maxHp;
                SetTowerData();
                if (_selected != null)
                    Informations.Instance.ResetSelected();
                float currentSizeX = (hp * _maxSizeX) / maxHp;
                progress.localScale = new Vector3(currentSizeX, _size.y, _size.z);

                float currentPosX = -((_maxSizeX - currentSizeX) / 2.0f);
                progress.localPosition = new Vector3(currentPosX, 0, 0);
                _isDamaged = false;
            }
            else
                StartCoroutine(GameManager.DisplayError("Not enough golds !"));
        }
    }

    public Informations.TowerData GetTowerData()
    {
        return TowerData;
    }

    public void Damage(int damage)
    {

        if (_isDamaged == false) 
        {
            _isDamaged = true;
        }
        hp -= damage;
        float currentSizeX = (hp * _maxSizeX) / maxHp;
        progress.localScale = new Vector3(currentSizeX, _size.y, _size.z);

        float currentPosX = -((_maxSizeX - currentSizeX) / 2.0f);
        progress.localPosition = new Vector3(currentPosX, 0, 0);

        if (hp >= 0)
            TowerData._hp = hp;

        if (_selected != null)
        {
            SetTowerData();
            Informations.Instance.SetInformations(TowerData, this);
        }
    }

    public void IsSelected(GameObject selected)
    {
        if (selected == null)
        {
            Destroy(_selected);
            _selected = null;
        }
        else
        {
            _selected = Instantiate(selected, this.transform);
            float radius = -1;
            var childs = GetComponentsInChildren<SphereCollider>();
            foreach (var child in childs)
            {
                if (child.name.Contains("Zone"))
                {
                    radius = child.radius / 4;
                    break;
                }
            }

            if (radius <= 0)
                radius = 1;

            radius = radius / 1.82f / 0.85f;

            _selected.transform.localScale = new Vector3(radius, radius, radius);
        }
    }

    #endregion
}