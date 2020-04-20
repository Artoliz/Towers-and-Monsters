using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    #region SerializableVariables

    [SerializeField] private int gridSizeX = 32;
    [SerializeField] private int gridSizeZ = 32;
    
    [SerializeField] private float gridSpacingOffset = 1f;

    [SerializeField] private List<Vector2Int> blockedPositions = new List<Vector2Int>();

    #endregion

    #region PrivateVariables

    private GameObject[,] _grid;

    #endregion
    
    #region MonoBehavior

    private void Awake()
    {
        _grid = new GameObject[gridSizeX, gridSizeZ];

        BlockPositions();
    }

    #endregion

    #region PrivateFunctions

    private void BlockPositions()
    {
        for (var x = 0; x < gridSizeX; x++)
        {
            for (var z = 0; z < gridSizeZ; z++)
            {
                if (blockedPositions.Contains(new Vector2Int(x, z)))
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