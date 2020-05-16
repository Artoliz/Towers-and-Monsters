using UnityEngine;

public class EffectOfDeath : MonoBehaviour
{
    #region SerializedVariables

    [SerializeField] private float eodDuration;
    
    #endregion

    #region ProtectedVariables

    protected Collider EnemyCollider;

    #endregion
    
    #region MonoBehaviour

    private void Start()
    {
        Destroy(this, eodDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyCollider = other;
            var enemy = EnemyCollider.GetComponent<Enemy>();

            if (!enemy.IsBoosted())
            {
                enemy.SetIsBoosted(true);
                BoostEnemy();
            }
        }
    }

    
    #endregion

    #region VirtualMethods

    protected virtual void BoostEnemy()
    {
        //Override this method in inherited classes.
    }

    #endregion
}
