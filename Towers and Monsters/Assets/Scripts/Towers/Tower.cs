using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    #region PrivateVariables

    private float _homeY;

    private bool _isShoot;

    private GameObject _towerBug;
    
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Pose = Animator.StringToHash("T_pose");

    #endregion

    #region PublicVariables

    public int hp = 20;
    public int dmg = 10;

    public float shootDelay;

    public bool catcher;

    public Vector3 impactNormal2;

    public GameObject bullet;
    public GameObject destroyParticle;

    public Transform target;
    public Transform lookAtObj;
    public Transform shootElement;

    public Animator anim2;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        anim2 = GetComponent<Animator>();

        _towerBug = transform.Find("Tower_Bug").gameObject;
        
        //Temporary assignement
        lookAtObj = gameObject.transform;

        _homeY = lookAtObj.transform.localRotation.eulerAngles.y;
    }

    private void Update()
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
            destroyParticle = Instantiate(destroyParticle, _towerBug.transform.position,
                Quaternion.FromToRotation(Vector3.up, impactNormal2));
            Destroy(destroyParticle, 3);
        }
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
            anim2.SetBool(Attack, true);
            anim2.SetBool(Pose, false);
        }

        _isShoot = false;
    }

    private void StopCatcherAttack()
    {
        target = null;

        anim2.SetBool(Attack, false);
        anim2.SetBool(Pose, true);
    }

    #endregion

    #region PublicMethods

    public void Damage(int damage)
    {
        hp -= damage;
    }

    #endregion
}