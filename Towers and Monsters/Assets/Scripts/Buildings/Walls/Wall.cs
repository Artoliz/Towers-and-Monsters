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

        for (int y = posInGrid.y + 1; y >= posInGrid.y - 1; y--)
        {
            for (int x = posInGrid.x - 1; x <= posInGrid.x + 1; x++)
            {
                Vector2Int gridPos = new Vector2Int(x, y);
                if (posInGrid != gridPos && Grid.Instance.IsElementInGrid(gridPos, "Wall"))
                {
                    Vector3 finalPos = (Grid.Instance.GetPositionFromGrid(gridPos) + this.transform.position) / 2;
                    Quaternion quat = Quaternion.identity;
                    Vector3 finalRot = new Vector3();

                    if (posInGrid.y == gridPos.y - 1 || posInGrid.y == gridPos.y + 1)
                        finalRot.y = 90;

                    if ((posInGrid.x == gridPos.x + 1 && posInGrid.y == gridPos.y - 1) ||
                        (posInGrid.x == gridPos.x - 1 && posInGrid.y == gridPos.y + 1))
                        finalRot.y = 45;

                    if ((posInGrid.x == gridPos.x + 1 && posInGrid.y == gridPos.y + 1) ||
                        (posInGrid.x == gridPos.x - 1 && posInGrid.y == gridPos.y - 1))
                        finalRot.y = -45;

                    quat.eulerAngles = finalRot;
                    GameObject tmp = Instantiate(_wallIntersect, finalPos, quat);
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