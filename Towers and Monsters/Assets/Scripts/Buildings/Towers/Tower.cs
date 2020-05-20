using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    #region PrivateVariables

    private float _homeY;

    private bool _isShoot;
    
    public enum towerType {bullet, effect, aoe};

    private Vector3 _particleExplosionPosition;

    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Pose = Animator.StringToHash("T_pose");

    private GameObject _selected;

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

    public float shootDelay;

    public bool catcher;

    public GameObject bullet;
    public GameObject destroyParticle;

    public Transform target;
    public Transform lookAtObj;
    public Transform shootElement;
    public GameObject upgradePrefab;
    public Animator anim;

    #endregion

    #region ProtectedVariables

    protected Informations.TowerData TowerData;

    protected GameObject ParentGO;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        _particleExplosionPosition = transform.position;
        _particleExplosionPosition.y = 1;

        anim = GetComponent<Animator>();

        //Temporary assignement
        lookAtObj = gameObject.transform;

        _homeY = lookAtObj.transform.localRotation.eulerAngles.y;
        SetTowerData();

        ParentGO = transform.parent.gameObject;
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

            if (!_isShoot)
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

            if (hp <= 0)
            {
                Destroy(gameObject);
                destroyParticle = Instantiate(destroyParticle, _particleExplosionPosition,
                    Quaternion.FromToRotation(Vector3.up, Vector3.zero));
                Destroy(destroyParticle, 1);
            }
        }
    }

    private void OnMouseDown()
    {
        TowerData._hp = this.hp;

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
        TowerData._repair = this.repairCost;
        TowerData._upgrade = this.upgradeCost;
        TowerData._damageToEnemy = this.dmg;
        TowerData._speedAttack = this.shootDelay;
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
                Instantiate(upgradePrefab, transform.position, transform.rotation);
                Destroy(ParentGO);
            }
            else
                StartCoroutine(GameManager.DisplayError("Not enough golds !"));
        }
    }

    public void Destroy()
    {
        Informations.Instance.ResetSelected();
        Destroy(ParentGO);
    }

    public void Repair()
    {
        if (hp < maxHp)
        {
            if (this.repairCost <= GameManager.Instance.GetGolds())
            {
                GameManager.Instance.RemoveGolds(this.repairCost);
                hp = maxHp;
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
        hp -= damage;
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
                    radius = child.radius;
                    break;
                }
            }
            if (radius == -1)
                radius = 5;
            _selected.transform.localScale = new Vector3(radius * 2, radius * 2, 1);
        }
    }

    #endregion
}