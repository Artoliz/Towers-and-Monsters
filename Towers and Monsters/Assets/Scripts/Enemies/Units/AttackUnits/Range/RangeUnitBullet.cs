using UnityEngine;

public class RangeUnitBullet : MonoBehaviour
{
    #region PrivateVariables

    private int _damage;

    private Transform _target;

    #endregion

    #region SeriazableVariables

    [SerializeField] private float timeBeforeDestroyBullet = 0.05f;
    [SerializeField] private float particleTime = 1f;

    #endregion

    #region PublicVariables

    public float speed = 2f;

    public Vector3 impactNormal;

    public GameObject impactParticle;

    #endregion

    #region MonoBehavior

    private void Update()
    {
        if (_target)
        {
            Transform transformSave;
            
            (transformSave = transform).LookAt(_target);
            transform.position = Vector3.MoveTowards(transformSave.position, _target.position, Time.deltaTime * speed);
        }
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform == _target)
        {
            _target.GetComponent<Tower>().Damage(_damage);
            Destroy(gameObject, timeBeforeDestroyBullet);

            impactParticle = Instantiate(impactParticle, transform.position,
                Quaternion.FromToRotation(Vector3.up, impactNormal));
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