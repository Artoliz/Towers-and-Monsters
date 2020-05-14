using UnityEngine;

public class Wall : MonoBehaviour
{
    #region PrivateVariables

    private GameObject _wallBug;

    [SerializeField] private GameObject _wallIntersect = null;

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

        PlaceWallIntersections();
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

    #region PrivateMethods

    private void PlaceWallIntersections()
    {
        Vector2Int posInGrid = Grid.Instance.GetPositionInGrid(this.transform.position);

        for (int y = posInGrid.y + 1; y < posInGrid.y - 1; y++)
        {
            for (int x = posInGrid.x - 1; x < posInGrid.x + 1; x++)
            {
                if (Grid.Instance.IsElementInGrid(new Vector2Int(x, y)))
                {

                }
            }
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