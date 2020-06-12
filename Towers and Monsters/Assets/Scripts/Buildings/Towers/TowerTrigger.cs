using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TowerTrigger : MonoBehaviour
{
    #region PublicVariables

    public bool lockE;

    public GameObject curTarget;

    //public Tower twr;

    #endregion

    #region PrivateVariables

    private Tower twr;
    private List<GameObject> enemies = new List<GameObject>();

    private List<GameObject> aoeImpacts = new List<GameObject>();

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        twr = GetComponentInParent<Tower>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (other.CompareTag("Enemy"))
            {
                enemies.Add(other.gameObject);
                if (twr.type == Tower.towerType.aoe && !other.GetComponent<Enemy>().GetOnAOEState())
                {
                    GameObject b = Instantiate(twr.bullet, other.transform.position, Quaternion.identity);
                    b.GetComponent<TowerBullet>().twr = twr;
                    b.GetComponent<TowerBullet>().target = other.transform;
                    aoeImpacts.Add(b);
                    other.GetComponent<Enemy>().SetOnAOE(true);
                    other.GetComponent<Enemy>().SetSpeed(other.GetComponent<Enemy>().GetSpeed() / 2.0f);
                } else if (!lockE)
                {
                    var o = enemies[0];
                    if (o != null)
                    {
                        twr.target = o.transform;
                        curTarget = o;
                        lockE = true;
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (other.CompareTag("Enemy"))
            {
                if (twr.type == Tower.towerType.aoe && !other.GetComponent<Enemy>().GetOnAOEState())
                {
                    GameObject b = Instantiate(twr.bullet, other.transform.position, Quaternion.identity);
                    b.GetComponent<TowerBullet>().twr = twr;
                    b.GetComponent<TowerBullet>().target = other.transform;
                    aoeImpacts.Add(b);
                    other.GetComponent<Enemy>().SetOnAOE(true);                   
                    other.GetComponent<Enemy>().SetSpeed(other.GetComponent<Enemy>().GetSpeed() / 2.0f);
                }
            }
        }
    }

    private void Update()
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (curTarget)
            {
                if (curTarget.CompareTag($"Dead"))
                {
                    enemies.Remove(curTarget);
                    lockE = false;
                    twr.target = null;
                    curTarget = null;
                    if (enemies.Count > 0) 
                    {
                        if (enemies[0])
                        {
                            var o = enemies[0];
                            twr.target = o.transform;
                            curTarget = o;
                            lockE = true;
                        }
                    }
                }
            }
            else
            {
                lockE = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (other.CompareTag("Enemy"))
            {
                enemies.Remove(other.gameObject);
                if (twr.type == Tower.towerType.aoe && other.GetComponent<Enemy>().GetOnAOEState())
                {
                    foreach (var item in aoeImpacts)
                    {
                        if (item && item.GetComponent<TowerBullet>() && item.GetComponent<TowerBullet>().target && item.GetComponent<TowerBullet>().target.gameObject == other.transform.gameObject) {
                            GameObject b = item;
                            aoeImpacts.Remove(item);
                            Destroy(b);
                            break;
                        }
                    }
                    other.GetComponent<Enemy>().SetOnAOE(false);
                    other.GetComponent<Enemy>().SetSpeed(other.GetComponent<Enemy>().GetSpeed() * 2.0f);
                } else if (other.gameObject == curTarget)
                {
                    lockE = false;
                    twr.target = null;
                    curTarget = null;
                    if (enemies.Count > 0) 
                    {
                        var o = enemies[0];
                        if (o != null)
                        {
                            twr.target = o.transform;
                            curTarget = o;
                            lockE = true;
                        }
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (twr.type == Tower.towerType.aoe)
        {
            foreach (var item in aoeImpacts)
            {
                GameObject current = item;
                TowerBullet towerBullet = null;
                if (current != null)
                    towerBullet = current.GetComponent<TowerBullet>();
                if (towerBullet != null && towerBullet.target != null)
                {
                    Enemy enemy = towerBullet.target.GetComponent<Enemy>();
                    enemy.SetOnAOE(false);
                    enemy.SetSpeed(enemy.GetSpeed() * 2.0f);
                }
                Destroy(item);
            }
        }
    }

    #endregion
}