using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    #region PrivateVariables

    private const float I = 0.05f;

    private Vector3 _lastBulletPosition;

    #endregion

    #region PublicVariables

    public float speed;

    public Vector3 impactNormal;

    public GameObject impactParticle;

    public Transform target;

    public Enemy enemy;

    #endregion

    private void Update()
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
            transform.position = Vector3.MoveTowards(transform.position, _lastBulletPosition, Time.deltaTime * speed);

            if (transform.position == _lastBulletPosition)
            {
                Destroy(gameObject, I);

                if (impactParticle != null)
                {
                    impactParticle = Instantiate(impactParticle, transform.position,
                        Quaternion.FromToRotation(Vector3.up, impactNormal));
                    Destroy(impactParticle, 3);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform == target)
        {
            target.GetComponent<TowerHP>().Dmg_2(enemy.damageToBuildings);
            Destroy(gameObject, I);
            impactParticle =
                Instantiate(impactParticle, transform.position,
                    Quaternion.FromToRotation(Vector3.up, impactNormal));
            impactParticle.transform.parent = target.transform;
            Destroy(impactParticle, 3);
        }
    }
}