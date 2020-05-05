using UnityEngine;

public class EnemyVSquad : MonoBehaviour
{
    #region PrivateVariables

    private int _curWayPointIndex;

    private Transform _target;

    #endregion

    #region PublicVariables

    public int creatureDamage = 10;

    public float speed;
    public float previousSpeed;

    public GameObject bullet;
    public GameObject enemyTarget;

    public Transform shootElement;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        previousSpeed = speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Castle"))
        {
            speed = 0;
            enemyTarget = other.gameObject;
            _target = enemyTarget.transform;
        }
    }

    private void Update()
    {
        if (enemyTarget)
        {
            if (enemyTarget.CompareTag($"Castle_Destroyed"))
            {
                speed = previousSpeed;
                enemyTarget = null;
            }
        }
    }

    #endregion

    #region PrivateMethods

    private void Shooting()
    {
        //if (EnemyTarget)
        // {           
        var с = Instantiate(bullet, shootElement.position, Quaternion.identity);
        с.GetComponent<EnemyBullet>().target = _target;
        с.GetComponent<EnemyBullet>().twr = this;
        // }  
    }

    private void GetDamage()
    {
        enemyTarget.GetComponent<TowerHP>().Dmg_2(creatureDamage);
    }

    #endregion
}