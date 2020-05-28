using UnityEngine;
using System.Collections;

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

    private void Start()
    {  
        speed = 6;
        particleTime = 6;

        if (twr.type == Tower.towerType.aoe) {
            Instantiate(impactParticle, this.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
        { 
            if (other.gameObject.transform == target)
            {
                //float ennemySpeed = target.GetComponent<Enemy>().GetSpeed();
                var enemy = target.GetComponent<Enemy>();

                enemy.Damage(twr.dmg);
              
                if (twr.type == Tower.towerType.effect && !enemy.GetOnEffectState())
                {
                    StartCoroutine(ApplyEffect(enemy, twr.effectDammage));
                } 
                else if (twr.type == Tower.towerType.aoe && !enemy.GetOnAOEState()) 
                {

                } 
                else 
                {
                   DestroyMe();
                }
                //target.GetComponent<Enemy>().SetSpeed(ennemySpeed / 2);
            }
        }
    }

    private void Update()
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
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

    #region PrivateFunctions
    
    IEnumerator ApplyEffect(Enemy enemy, int dmg)
    {
        int loop = 0;
        while (loop < 5) {
            enemy.Damage(dmg / 5);
            yield return new WaitForSeconds(1.0f);
            loop += 1;
        }
        DestroyMe();
    }

    private void DestroyMe()
    {
        Destroy(gameObject, 0.05f); // destroy bullet
        impactParticle = Instantiate(impactParticle, target.transform.position,
            Quaternion.FromToRotation(Vector3.up, Vector3.zero));
        impactParticle.transform.parent = target.transform;
        Destroy(impactParticle, particleTime);
    }

    #endregion
}