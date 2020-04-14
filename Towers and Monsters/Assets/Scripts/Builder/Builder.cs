using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    #region PrivateVariables

    private Camera _camera;
    private bool _isCameraNotNull;

    private Grid _gridObject;

    private Buildings _buildingEnum = Buildings.None;

    #endregion
    
    public GameObject[] buildingsPrefab;
    public Dictionary<Buildings, GameObject> buildings;
    
    #region PublicVariables

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        _camera = Camera.main;
        _isCameraNotNull = _camera != null;

        _gridObject = FindObjectOfType<Grid>();

        InitBuildings();
    }

    private void Update()
    {
        SelectedBuilding();

        if (Input.GetMouseButtonDown(0) && _buildingEnum != Buildings.None)
        {
            if (_isCameraNotNull)
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var click))
                {
                    PlaceBuildingOnGrid(click.point);
                }
            }
        }
    }

    #endregion

    #region PrivateFunctions

    private void InitBuildings()
    {
        buildings = new Dictionary<Buildings, GameObject>
        {
            [Buildings.Wall] = buildingsPrefab[0],
            [Buildings.TowerRed] = buildingsPrefab[1],
            [Buildings.TowerBlue] = buildingsPrefab[2],
            [Buildings.TowerWhite] = buildingsPrefab[3]
        };

    }

    private void SelectedBuilding()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _buildingEnum = _buildingEnum == Buildings.Wall ? Buildings.None : Buildings.Wall;
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
            _buildingEnum = _buildingEnum == Buildings.TowerRed ? Buildings.None : Buildings.TowerRed;
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
            _buildingEnum = _buildingEnum == Buildings.TowerBlue ? Buildings.None : Buildings.TowerBlue;

        if (Input.GetKeyDown(KeyCode.Alpha4))
            _buildingEnum = _buildingEnum == Buildings.TowerWhite ? Buildings.None : Buildings.TowerWhite;

        if (Input.GetKeyDown(KeyCode.Escape))
            _buildingEnum = Buildings.None;
    }

    private void PlaceBuildingOnGrid(Vector3 clickPoint)
    {
        var gridSpacingOffset = _gridObject.gridSpacingOffset;

        if (_gridObject.AppendElementInGrid(clickPoint, buildings[_buildingEnum]))
        {
            var xCount = Mathf.RoundToInt(clickPoint.x / gridSpacingOffset);
            var zCount = Mathf.RoundToInt(clickPoint.z / gridSpacingOffset);

            var result = new Vector3(
                xCount * gridSpacingOffset,
                1,
                zCount * gridSpacingOffset);

            Instantiate(buildings[_buildingEnum]).transform.position = result;
        }
    }

    #endregion
}