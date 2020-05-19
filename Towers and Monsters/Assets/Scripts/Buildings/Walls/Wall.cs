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

    public GameObject destroyParticle;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        _particleExplosionPosition = transform.position;
        _particleExplosionPosition.y = 1;
    }

    private void Update()
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused && hp <= 0)
        {
            Destroy(gameObject);
            destroyParticle = Instantiate(destroyParticle, _particleExplosionPosition, 
                Quaternion.FromToRotation(Vector3.up, Vector3.zero));
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

    private bool IsAlreadyWallIntersect(Vector3 finalPos)
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Wall");

        foreach (GameObject go in gos)
        {
            if (go.name.Contains("Wall_Intersect") && go.transform.position == finalPos)
                return true;
        }

        return false;
    }

    #endregion

    #region PublicMethods

    public void PlaceWallIntersections()
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

                    if (IsAlreadyWallIntersect(finalPos))
                        continue;

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
                    _intersections.Add(Instantiate(wallIntersect, finalPos, quat));
                }
            }
        }
    }

    public void Damage(int damage)
    {
        hp -= damage;
    }

    public List<GameObject> GetIntersects()
    {
        return _intersections;
    }

    #endregion
}