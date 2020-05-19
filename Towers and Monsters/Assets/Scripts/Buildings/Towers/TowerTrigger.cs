using UnityEngine;

public class TowerTrigger : MonoBehaviour
{
    #region PublicVariables

    public bool lockE;

    public GameObject curTarget;

    public Tower twr;

    #endregion

    #region MonoBehaviour

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !lockE)
        {
            var o = other.gameObject;

            twr.target = o.transform;
            curTarget = o;
            lockE = true;
        }
    }
    */

    private void Update()
    {
        if (curTarget)
        {
            if (curTarget.CompareTag($"Dead"))
            {
                lockE = false;
                twr.target = null;
            }
        }

        if (!curTarget)
        {
            lockE = false;
        }
    }

    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && other.gameObject == curTarget)
        {
            lockE = false;
            twr.target = null;
        }
    }
    */

    #endregion
}