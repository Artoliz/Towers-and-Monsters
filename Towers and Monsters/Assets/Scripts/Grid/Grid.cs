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

    public GameObject obstacles;
    public GameObject playerBase;
    public GameObject spawns;

    #endregion
    
    #region MonoBehavior

    private void Awake()
    {
        _grid = new GameObject[gridSizeX, gridSizeZ];

        GetBlockedPositions();
        BlockPositions();
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
        if (pos.x + 1 < gridSizeX && pos.y + 1 < gridSizeZ)
        {
            tmpPos = pos;
            tmpPos.x += 1;
            tmpPos.y += 1;
            _blockedPositions.Add(tmpPos);
        }
        if (pos.x - 1 >= 0 && pos.y - 1 >= 0)
        {
            tmpPos = pos;
            tmpPos.x -= 1;
            tmpPos.y -= 1;
            _blockedPositions.Add(tmpPos);
        }
        if (pos.x + 1 < gridSizeX && pos.y - 1 >= 0)
        {
            tmpPos = pos;
            tmpPos.x += 1;
            tmpPos.y -= 1;
            _blockedPositions.Add(tmpPos);
        }
        if (pos.x - 1 >= 0 && pos.y + 1 < gridSizeZ)
        {
            tmpPos = pos;
            tmpPos.x -= 1;
            tmpPos.y += 1;
            _blockedPositions.Add(tmpPos);
        }
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

        _grid[xCount, zCount] = null;
    }
    
    public bool AppendElementInGrid(Vector3 clickPoint, GameObject obj)
    {
        var xCount = Mathf.RoundToInt(clickPoint.x / gridSpacingOffset);
        var zCount = Mathf.RoundToInt(clickPoint.z / gridSpacingOffset);

        if (_grid[xCount, zCount] == null)
        {
            _grid[xCount, zCount] = obj;
            return true;
        }

        return false;
    }

    #endregion
}