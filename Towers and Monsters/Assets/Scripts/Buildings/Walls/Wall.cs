using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    #region PrivateVariables

    private Vector3 _particleExplosionPosition;
    
    [SerializeField] private GameObject wallIntersect;

    private readonly List<GameObject> _intersections = new List<GameObject>();

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
        _particleExplosionPosition = transform.position;
        _particleExplosionPosition.y = 1;

        PlaceWallIntersections();
    }

    private void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
            destroyParticle = Instantiate(destroyParticle, _particleExplosionPosition, 
                Quaternion.FromToRotation(Vector3.up, impactNormal));
            Destroy(destroyParticle, 1);
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject intersection in _intersections)
            Destroy(intersection);
        _intersections.Clear();
    }

    #endregion

    #region PrivateMethods

    private void PlaceWallIntersections()
    {
        Vector2Int posInGrid = Grid.Instance.GetPositionInGrid(transform.position);

        for (int y = posInGrid.y + 1; y >= posInGrid.y - 1; y--)
        {
            for (int x = posInGrid.x - 1; x <= posInGrid.x + 1; x++)
            {
                Vector2Int gridPos = new Vector2Int(x, y);
                if (posInGrid != gridPos && Grid.Instance.IsElementInGrid(gridPos, "Wall"))
                {
                    Vector3 finalPos = (Grid.Instance.GetPositionFromGrid(gridPos) + transform.position) / 2;
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
                    GameObject tmp = Instantiate(wallIntersect, finalPos, quat);
                    _intersections.Add(tmp);
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