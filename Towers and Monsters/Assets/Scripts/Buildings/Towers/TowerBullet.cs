﻿using UnityEngine;
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
        if (twr.type == Tower.towerType.aoe)
        {
            GameObject b = Instantiate(impactParticle, this.transform.position, Quaternion.identity, this.transform);
            b.transform.localScale = new Vector3(5, 5, 5);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
        { 
            if (other.gameObject.transform == target)
            {
                var enemy = target.GetComponent<Enemy>();

                enemy.Damage(twr.dmg);
              
                if (twr.type == Tower.towerType.effect && !enemy.GetOnEffectState())
                {
                    enemy.SetOnEffect(true);
                    StartCoroutine(ApplyEffect(enemy, twr.effectDammage));
                }
                else if (twr.type != Tower.towerType.aoe)
                {
                   DestroyMe();
                }
            }
        }
    }

    private void Update()
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (target)
            {
                if (target.CompareTag($"Dead"))
                    Destroy(gameObject, 0.5f);
                else
                {
                    Transform transform1;
                    (transform1 = transform).LookAt(target);
                    Vector3 targetPosition = target.position;
                    targetPosition.y = 0.7f;
                    transform.position = Vector3.MoveTowards(transform1.position, targetPosition, Time.deltaTime * speed);
                    _lastBulletPosition = targetPosition;
                }
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
        enemy.SetOnEffect(false);
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