using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    #region PublicVariables

    public int gridSizeX;
    public int gridSizeZ;
    
    public float gridSpacingOffset = 1f;

    public List<Vector2Int> blockedPositions;
    
    public GameObject[] gridCellToPickFrom;

    #endregion

    #region PrivateVariables

    private readonly Vector3 _gridOrigin = Vector3.zero;
    
    private GameObject[,] _grid;

    #endregion
    
    #region MonoBehavior

    private void Awake()
    {
        _grid = new GameObject[gridSizeX, gridSizeZ];

        SpawnGrid();
    }

    #endregion

    #region PrivateFunctions

    private void SpawnGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                if (blockedPositions.Contains(new Vector2Int(x, z)))
                {
                    _grid[x, z] = gameObject;
                }
                Vector3 spawnPosition = new Vector3(x * gridSpacingOffset, -0.5f, z * gridSpacingOffset) + _gridOrigin;
                PickAndSpawnGridCase(spawnPosition, Quaternion.identity);
            }
        }
    }

    private void PickAndSpawnGridCase(Vector3 positionToSpawn, Quaternion rotationToSpawn)
    {
        int randomIndex = Random.Range(0, gridCellToPickFrom.Length);

        Instantiate(gridCellToPickFrom[randomIndex], positionToSpawn, rotationToSpawn);
    }

    #endregion
    
    #region PublicFunctions

    #region Getter/Setter

    public GameObject[,] GetGrid()
    {
        return _grid;
    }

    #endregion
    
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