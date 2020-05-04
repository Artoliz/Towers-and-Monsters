using UnityEngine;

public class EnemyVSquad : MonoBehaviour
{
    #region PrivateVariables

    private int _curWayPointIndex;

    private static readonly int Run = Animator.StringToHash("RUN");
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Victory = Animator.StringToHash("Victory");

    #endregion
    
    #region PublicVariables

    public int creatureDamage = 10;
    
    public float speed;
    public float previousSpeed;

    public GameObject bullet;
    public GameObject enemyBug;
    public GameObject enemyTarget;

    public Transform target;
    public Transform shootElement;
    public Transform[] wayPoints;

    public Animator anim;

    public EnemyHp enemyHp;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        anim = GetComponent<Animator>();
        enemyHp = enemyBug.GetComponent<EnemyHp>();
        previousSpeed = speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Castle"))
        {
            speed = 0;
            var o = other.gameObject;
            enemyTarget = o;
            target = o.transform;
            var position = enemyTarget.transform.position;
            var targetPosition = new Vector3(position.x, transform.position.y, position.z);
            transform.LookAt(targetPosition);
            anim.SetBool(Run, false);
            anim.SetBool(Attack, true);
        }
    }

    private void Update()
    {
        if (_curWayPointIndex < wayPoints.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, wayPoints[_curWayPointIndex].position,
                Time.deltaTime * speed);

            if (!enemyTarget)
            {
                transform.LookAt(wayPoints[_curWayPointIndex].position);
            }

            if (Vector3.Distance(transform.position, wayPoints[_curWayPointIndex].position) < 0.5f)
            {
                _curWayPointIndex++;
            }
        }

        else
        {
            anim.SetBool(Victory, true);
        }

        if (enemyHp.enemyHp <= 0)
        {
            speed = 0;
            Destroy(gameObject, 5f);
            anim.SetBool(Death, true);
        }

        if (enemyTarget)
        {
            if (enemyTarget.CompareTag($"Castle_Destroyed"))
            {
                anim.SetBool(Attack, false);
                anim.SetBool(Run, true);
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
        с.GetComponent<EnemyBullet>().target = target;
        с.GetComponent<EnemyBullet>().twr = this;
        // }  
    }

    private void GetDamage()
    {
        enemyTarget.GetComponent<TowerHP>().Dmg_2(creatureDamage);
    }

    #endregion
}