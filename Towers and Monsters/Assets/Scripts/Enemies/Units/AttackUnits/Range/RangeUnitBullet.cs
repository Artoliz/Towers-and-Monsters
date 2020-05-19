using UnityEngine;

public class RangeUnitBullet : MonoBehaviour
{
    #region PrivateVariables

    private int _damage;

    private Transform _target;

    #endregion

    #region SeriazableVariables

    #endregion

    #region PublicVariables

    public float speed = 2f;

    public GameObject impactParticle;

    #endregion

    #region MonoBehavior

    private void Update()
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (_target)
            {
                Transform transformSave;

                (transformSave = transform).LookAt(_target);
                transform.position =
                    Vector3.MoveTowards(transformSave.position, _target.position, Time.deltaTime * speed);
            }
            else
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (other.gameObject.transform == _target)
            {
                _target.GetComponent<Tower>().Damage(_damage);
                Destroy(gameObject, 0.05f);

                impactParticle = Instantiate(impactParticle, transform.position,
                    Quaternion.FromToRotation(Vector3.up, Vector3.zero));
                impactParticle.transform.parent = _target.transform;
                Destroy(impactParticle, 1);
            }
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