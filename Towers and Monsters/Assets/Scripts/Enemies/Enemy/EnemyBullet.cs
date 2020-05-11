using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    #region PrivateVariables

    private int _damage;

    private Vector3 _lastBulletPosition;

    private Transform _target;

    #endregion

    #region SeriazableVariables

    [SerializeField] private float timeBeforeDestroyBullet = 0.05f;
    [SerializeField] private float particleTime = 3;

    #endregion
    
    #region PublicVariables

    public float speed;

    public Vector3 impactNormal;

    public GameObject impactParticle;

    #endregion

    #region MonoBehavior

    private void Update()
    {
        if (_target)
        {
            Transform transform1;
            (transform1 = transform).LookAt(_target);
            transform.position = Vector3.MoveTowards(transform1.position, _target.position, Time.deltaTime * speed);
            _lastBulletPosition = _target.transform.position;
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _lastBulletPosition, Time.deltaTime * speed);

            if (transform.position == _lastBulletPosition)
            {
                Destroy(gameObject, timeBeforeDestroyBullet);

                if (impactParticle != null)
                {
                    impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));
                    Destroy(impactParticle, particleTime);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform == _target)
        {
            _target.GetComponent<Tower>().Damage(_damage);
            Destroy(gameObject, timeBeforeDestroyBullet);
            
            impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));
            impactParticle.transform.parent = _target.transform;
            Destroy(impactParticle, particleTime);
        }
    }

    #endregion

    #region PublicMethods

    public void SetDamage(int damage)
    {
        _damage = damage;
    }
    
    public void SetTarget(Transform target)
    {
        _target = target;
    }

    #endregion
}