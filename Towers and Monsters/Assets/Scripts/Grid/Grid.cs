using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grid : MonoBehaviour
{
    #region PrivateVariables

    private readonly List<Vector2Int> _blockedPositions = new List<Vector2Int>();

    private GameObject[,] _grid;

    #endregion

    #region SerializableVariables

    [SerializeField] private int gridSizeX = 32;
    [SerializeField] private int gridSizeZ = 32;

    [SerializeField] private float gridSpacingOffset = 1f;

    #endregion

    #region PublicVariables

    public static Grid Instance = null;

    public GameObject obstacles;
    public GameObject playerBase;
    public GameObject spawns;

    public Pathfinder _pathfinder;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        _grid = new GameObject[gridSizeX, gridSizeZ];

        _pathfinder = new Pathfinder(gridSizeX, gridSizeZ);

        GetBlockedPositions();
        BlockPositions();
        Instance = this;
    }

    #endregion

    #region PrivateFunctions

    private void GetBlockedPositions()
    {
        GetBlockObstacles();
        GetBlockSpawns();
        GetBlockBase();
    }

    private void GetBlockObstacles()
    {
        var pos = new Vector2Int();

        foreach (var child in obstacles.GetComponentsInChildren<NavMeshObstacle>())
        {
            var pivot = child.gameObject.transform.Find("Pivot");
            if (pivot == null)
            {
                Debug.LogWarning("Oops... This object has not 'Pivot' child: " + child.name);
                continue;
            }

            var childPos = pivot.position;

            pos.x = Mathf.RoundToInt(childPos.x / gridSpacingOffset);
            pos.y = Mathf.RoundToInt(childPos.z / gridSpacingOffset);

            _blockedPositions.Add(pos);

            _pathfinder.SetBlockedPosition(pos.x, pos.y);
        }
    }

    private void GetBlockSpawns()
    {
        var pos = new Vector2Int();

        foreach (Transform spawn in spawns.transform)
        {
            var childPos = spawn.position;

            pos.x = Mathf.RoundToInt(childPos.x / gridSpacingOffset);
            pos.y = Mathf.RoundToInt(childPos.z / gridSpacingOffset);
            _blockedPositions.Add(pos);

            _pathfinder.SetSource(pos.x, pos.y);

            Vector2Int tmpPos;
            if (pos.x + 1 < gridSizeX)
            {
                tmpPos = pos;
                tmpPos.x += 1;
                _blockedPositions.Add(tmpPos);
            }

            if (pos.x - 1 >= 0)
            {
                tmpPos = pos;
                tmpPos.x -= 1;
                _blockedPositions.Add(tmpPos);
            }

            if (pos.y + 1 < gridSizeZ)
            {
                tmpPos = pos;
                tmpPos.y += 1;
                _blockedPositions.Add(tmpPos);
            }

            if (pos.y - 1 >= 0)
            {
                tmpPos = pos;
                tmpPos.y -= 1;
                _blockedPositions.Add(tmpPos);
            }

            if (pos.x - 1 >= 0 && pos.x + 1 < gridSizeX) {
                _pathfinder.SetBlockedPosition(pos.x - 1, pos.y);
                _pathfinder.SetBlockedPosition(pos.x + 1, pos.y);
            }
            if (pos.y - 1 >= 0 && pos.y + 1 < gridSizeZ)
            {
                _pathfinder.SetBlockedPosition(pos.x, pos.y - 1);
                _pathfinder.SetBlockedPosition(pos.x, pos.y + 1);
            }
        }
    }

    private void GetBlockBase()
    {
        var playerBasePos = playerBase.transform.position;
        var pos = new Vector2Int
        {
            x = Mathf.RoundToInt(playerBasePos.x / gridSpacingOffset),
            y = Mathf.RoundToInt(playerBasePos.z / gridSpacingOffset)
        };

        _blockedPositions.Add(pos);
        _pathfinder.SetDestination(pos.x, pos.y);

        Vector2Int tmpPos;
        if (pos.x + 1 < gridSizeX)
        {
            tmpPos = pos;
            tmpPos.x += 1;
            _blockedPositions.Add(tmpPos);
            _pathfinder.SetBlockedPosition(tmpPos.x, tmpPos.y);
        }

        if (pos.x - 1 >= 0)
        {
            tmpPos = pos;
            tmpPos.x -= 1;
            _blockedPositions.Add(tmpPos);
            _pathfinder.SetBlockedPosition(tmpPos.x, tmpPos.y);
        }

        if (pos.y + 1 < gridSizeZ)
        {
            tmpPos = pos;
            tmpPos.y += 1;
            _blockedPositions.Add(tmpPos);
            _pathfinder.SetBlockedPosition(tmpPos.x, tmpPos.y);
        }

        if (pos.y - 1 >= 0)
        {
            tmpPos = pos;
            tmpPos.y -= 1;
            _blockedPositions.Add(tmpPos);
            _pathfinder.SetBlockedPosition(tmpPos.x, tmpPos.y);
        }

        if (pos.x + 1 < gridSizeX && pos.y + 1 < gridSizeZ)
        {
            tmpPos = pos;
            tmpPos.x += 1;
            tmpPos.y += 1;
            _blockedPositions.Add(tmpPos);
            _pathfinder.SetBlockedPosition(tmpPos.x, tmpPos.y);
        }

        if (pos.x - 1 >= 0 && pos.y - 1 >= 0)
        {
            tmpPos = pos;
            tmpPos.x -= 1;
            tmpPos.y -= 1;
            _blockedPositions.Add(tmpPos);
            _pathfinder.SetBlockedPosition(tmpPos.x, tmpPos.y);
        }

        if (pos.x + 1 < gridSizeX && pos.y - 1 >= 0)
        {
            tmpPos = pos;
            tmpPos.x += 1;
            tmpPos.y -= 1;
            _blockedPositions.Add(tmpPos);
            _pathfinder.SetBlockedPosition(tmpPos.x, tmpPos.y);
        }

        if (pos.x - 1 >= 0 && pos.y + 1 < gridSizeZ)
        {
            tmpPos = pos;
            tmpPos.x -= 1;
            tmpPos.y += 1;
            _blockedPositions.Add(tmpPos);
            _pathfinder.SetBlockedPosition(tmpPos.x, tmpPos.y);
        }

        pos = new Vector2Int
        {
            x = Mathf.RoundToInt(playerBase.transform.Find("Gate").position.x / gridSpacingOffset),
            y = Mathf.RoundToInt(playerBase.transform.Find("Gate").position.z / gridSpacingOffset)
        };

        _pathfinder.RemoveBlockedPosition(pos.x, pos.y);
    }

    private void BlockPositions()
    {
        for (var x = 0; x < gridSizeX; x++)
        {
            for (var z = 0; z < gridSizeZ; z++)
            {
                if (_blockedPositions.Contains(new Vector2Int(x, z)))
                {
                    _grid[x, z] = gameObject;
                }
            }
        }
    }

    #endregion

    #region PublicFunctions

    #region Getter/Setter

    public float GetGridSpacingOffset()
    {
        return gridSpacingOffset;
    }

    #endregion

    public void RemoveElementInGrid(Vector3 clickPoint)
    {
        var xCount = Mathf.RoundToInt(clickPoint.x / gridSpacingOffset);
        var zCount = Mathf.RoundToInt(clickPoint.z / gridSpacingOffset);

        if (xCount < 0 || xCount >= gridSizeX || zCount < 0 || zCount >= gridSizeZ)
            return;

        _grid[xCount, zCount] = null;
        _pathfinder.RemoveBlockedPosition(xCount, zCount);
    }

    public bool AppendElementInGrid(Vector3 clickPoint, GameObject obj)
    {
        var xCount = Mathf.RoundToInt(clickPoint.x / gridSpacingOffset);
        var zCount = Mathf.RoundToInt(clickPoint.z / gridSpacingOffset);

        if (xCount < 0 || xCount >= gridSizeX || zCount < 0 || zCount >= gridSizeZ)
            return false;

        if (_grid[xCount, zCount] == null)
        {
            _grid[xCount, zCount] = obj;
            _pathfinder.SetBlockedPosition(xCount, zCount);
            return true;
        }

        return false;
    }

    public bool AppendElementInGrid(Vector2Int position, GameObject obj)
    {
        if (position.x < 0 || position.x >= gridSizeX || position.y < 0 || position.y >= gridSizeZ)
            return false;

            if (_grid[position.x, position.y] == null)
        {
            _grid[position.x, position.y] = obj;
            _pathfinder.SetBlockedPosition(position.x, position.y);
            return true;
        }

        return false;
    }

    public bool IsElementInGrid(Vector3 position)
    {
        var xCount = Mathf.RoundToInt(position.x / gridSpacingOffset);
        var zCount = Mathf.RoundToInt(position.z / gridSpacingOffset);

        if (_grid[xCount, zCount] != null)
            return true;

        return false;
    }

    public bool IsElementInGrid(Vector2Int position)
    {
        if (_grid[position.x, position.y] != null)
            return true;

        return false;
    }

    public bool IsElementInGrid(Vector2Int position, string tag)
    {
        if (position.x < 0 || position.x >= gridSizeX || position.y < 0 || position.y >= gridSizeZ)
            return false;

        if (_grid[position.x, position.y] != null && _grid[position.x, position.y].tag == tag)
            return true;

        return false;
    }

    public Vector2Int GetPositionInGrid(Vector3 position)
    {
        var xCount = Mathf.RoundToInt(position.x / gridSpacingOffset);
        var zCount = Mathf.RoundToInt(position.z / gridSpacingOffset);

        if (_grid[xCount, zCount] != null)
            return new Vector2Int(xCount, zCount);

        return new Vector2Int(-1, -1);
    }

    public Vector3 GetPositionFromGrid(Vector2Int position)
    {
        if (position.x < 0 || position.x >= gridSizeX || position.y < 0 || position.y >= gridSizeZ)
            return Vector3.zero;

        float x = position.x * gridSpacingOffset;
        float z = position.y * gridSpacingOffset;

        if (_grid[position.x, position.y] != null)
            return new Vector3(x, 0, z);

        return Vector3.zero;
    }

    public Vector3 CalculatePositionFromGrid(Vector2Int position)
    {
        if (position.x < 0 || position.x >= gridSizeX || position.y < 0 || position.y >= gridSizeZ)
            return new Vector3(-1, -1, -1);

        return new Vector3(position.x * gridSpacingOffset, 0, position.y * gridSpacingOffset);
    }

    public Vector2Int CalculatePositionInGrid(Vector3 position)
    {
        var xCount = Mathf.RoundToInt(position.x / gridSpacingOffset);
        var zCount = Mathf.RoundToInt(position.z / gridSpacingOffset);

        return new Vector2Int(xCount, zCount);
    }

    public bool IsPositionBlocked(Vector3 position)
    {
        var xCount = Mathf.RoundToInt(position.x / gridSpacingOffset);
        var zCount = Mathf.RoundToInt(position.z / gridSpacingOffset);

        return _blockedPositions.Contains(new Vector2Int(xCount, zCount));
    }

    public bool IsPositionBlocked(Vector2Int position)
    {
        return _blockedPositions.Contains(position);
    }

    public Vector2Int GetGridSize()
    {
        return new Vector2Int(gridSizeX, gridSizeZ);
    }

    #endregion
}