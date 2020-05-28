using UnityEngine;
using System.Collections.Generic;

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

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        twr = GetComponentInParent<Tower>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (twr.type == Tower.towerType.aoe)
            return;
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (other.CompareTag("Enemy"))
            {
                enemies.Add(other.gameObject);
                if (!lockE)
                {
                  //  var o = other.gameObject;
                    var o = enemies[0];
                    twr.target = o.transform;
                    curTarget = o;
                    lockE = true;
                }
                
            }
        }
    }

    private void Update()
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (twr.type == Tower.towerType.aoe)
                return;
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
                        if (enemies[0]) {
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
        if (twr.type == Tower.towerType.aoe)
            return;
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (other.CompareTag("Enemy"))
            {
                enemies.Remove(other.gameObject);
                if (other.gameObject == curTarget)
                {
                    lockE = false;
                    twr.target = null;
                    curTarget = null;
                    if (enemies.Count > 0) 
                    {
                        var o = enemies[0];
                        twr.target = o.transform;
                        curTarget = o;
                        lockE = true;
                    }
                }
            }
        }
    }

    #endregion
}