using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    #region PrivateVariables

    private Vector3 _particleExplosionPosition;
    
    [SerializeField] private GameObject wallIntersect = null;

    private readonly List<GameObject> _intersections = new List<GameObject>();

    private int maxHp = 20;

    #endregion

    #region PublicVariables

    public int cost = 100;

    public int hp = 20;

    public GameObject destroyParticle;

    public Informations.WallData WallData;

    public bool IsSelected = false;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        _particleExplosionPosition = transform.position;
        _particleExplosionPosition.y = 1;
        maxHp = hp;

        SoundsManager.Instance.PlaySound(SoundsManager.Audio.Construct);
    }

    private void Update()
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused && hp <= 0)
            RemoveWall();
    }

    private void OnDestroy()
    {
        foreach (GameObject intersection in _intersections)
            Destroy(intersection);
        _intersections.Clear();
    }

    private void OnMouseDown()
    {
        SetWallData();
        Informations.Instance.SetInformations(WallData, this);
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

    private void SetWallData()
    {
        WallData._hp = this.hp;
        WallData._sell = (this.cost / 2) * (this.hp / this.maxHp);
    }

    private void RemoveWall()
    {
        SoundsManager.Instance.PlaySound(SoundsManager.Audio.Destruct);
        Vector2Int posInGrid = Grid.Instance.CalculatePositionInGrid(this.transform.position);
        Grid.Instance._pathfinder.RemoveBlockedPosition(posInGrid.x, posInGrid.y);
        Grid.Instance.RemoveElementInGrid(this.transform.position);
        if (IsSelected)
            Informations.Instance.ResetSelected();
        Destroy(gameObject);
        destroyParticle = Instantiate(destroyParticle, _particleExplosionPosition,
            Quaternion.FromToRotation(Vector3.up, Vector3.zero));
        Destroy(destroyParticle, 1);
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
        this.hp -= damage;
        SetWallData();
    }

    public List<GameObject> GetIntersects()
    {
        return _intersections;
    }

    public void Destroy()
    {
        GameManager.Instance.AddGolds((this.cost / 2) * (this.hp / this.maxHp));
        RemoveWall();
    }

    #endregion
}