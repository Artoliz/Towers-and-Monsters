using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    #region PrivateVariables

    private Vector3 _lastBulletPosition;

    [SerializeField] private float particleTime = 3;

    #endregion

    #region PublicVariables

    public float speed;

    public GameObject impactParticle;

    public Transform target;

    public Tower twr;

    #endregion

    #region MonoBehaviour

    private void OnTriggerEnter(Collider other)
    {
        if (!WavesManager.gameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (other.gameObject.transform == target)
            {
                target.GetComponent<Enemy>().Damage(twr.dmg);
                Destroy(gameObject, 0.05f); // destroy bullet
                impactParticle = Instantiate(impactParticle, target.transform.position,
                    Quaternion.FromToRotation(Vector3.up, Vector3.zero));
                impactParticle.transform.parent = target.transform;
                Destroy(impactParticle, particleTime);
            }
        }
    }

    private void Update()
    {
        if (!WavesManager.gameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (target)
            {
                Transform transform1;
                (transform1 = transform).LookAt(target);
                transform.position = Vector3.MoveTowards(transform1.position, target.position, Time.deltaTime * speed);
                _lastBulletPosition = target.transform.position;
            }

            else
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, _lastBulletPosition, Time.deltaTime * speed);

                if (transform.position == _lastBulletPosition)
                {
                    Destroy(gameObject, 0.05f);

                    if (impactParticle != null)
                    {
                        impactParticle = Instantiate(impactParticle, transform.position,
                            Quaternion.FromToRotation(Vector3.up, Vector3.zero));
                        Destroy(impactParticle, particleTime);
                    }
                }
            }
        }
    }

    #endregion
}