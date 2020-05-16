using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    #region PrivateVariables

    private float _homeY;

    private bool _isShoot;
    
    private Vector3 _particleExplosionPosition;

    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Pose = Animator.StringToHash("T_pose");

    #endregion

    #region PublicVariables

    public int cost = 100;

    public int hp = 20;
    public int dmg = 10;

    public float shootDelay;

    public bool catcher;

    public Vector3 impactNormal;

    public GameObject bullet;
    public GameObject destroyParticle;

    public Transform target;
    public Transform lookAtObj;
    public Transform shootElement;

    public Animator anim;

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
            destroyParticle = Instantiate(destroyParticle, _particleExplosionPosition,
                Quaternion.FromToRotation(Vector3.up, impactNormal));
            Destroy(destroyParticle, 1);
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

    #endregion

    #region PublicMethods

    public void Damage(int damage)
    {
        hp -= damage;
    }

    #endregion
}