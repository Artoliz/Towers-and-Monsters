using UnityEngine;

public class Wall : MonoBehaviour
{
    #region PrivateVariables

    private GameObject _wallBug;

    #endregion

    #region PublicVariables

    public int cost = 100;

    public int hp = 20;

    public Vector3 impactNormal;

    public GameObject destroyParticle;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        _wallBug = transform.Find("Wall_Bug").gameObject;
    }

    private void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
            destroyParticle = Instantiate(destroyParticle, _wallBug.transform.position,
                Quaternion.FromToRotation(Vector3.up, impactNormal));
            Destroy(destroyParticle, 1);
        }
    }

    #endregion

    #region PublicMethods

    public void Damage(int damage)
    {
        hp -= damage;
    }

    #endregion
}