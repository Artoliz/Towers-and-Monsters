using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Builder : MonoBehaviour
{
    #region PrivateVariables

    private Camera _camera;
    private bool _isCameraNotNull;

    private Vector3 _playerBasePosition;

    private NavMeshPath _path;
    
    private List<Transform> _spawns = new List<Transform>();
    
    private Grid _gridObject;

    private Buildings _buildingEnum = Buildings.None;

    #endregion

    #region PublicVariables

    public GameObject playerBase;
    
    public GameObject[] buildingsPrefab;
    
    public Dictionary<Buildings, GameObject> buildings;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        _camera = Camera.main;
        _isCameraNotNull = _camera != null;

        _path = new NavMeshPath();

        _playerBasePosition = AdjustPositionOfPlayerBase(playerBase.transform.position);
        
        _gridObject = FindObjectOfType<Grid>();

        InitBuildings();
    }

    private void Start()
    {
        _spawns = EnemiesSpawns.instance.GetEnemiesSpawns();
    }

    private void Update()
    {
        if (!PauseMenu.gameIsPaused)
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
    }

    #endregion

    #region PrivateFunctions

    private static Vector3 AdjustPositionOfPlayerBase(Vector3 spawn)
    {
        if (spawn.x < 0)
        {
            spawn.x = 0;
        }
        if (spawn.x > 31)
        {
            spawn.x = 31;
        }
        if (spawn.z < 0)
        {
            spawn.z = 0;
        }
        if (spawn.z > 31)
        {
            spawn.z = 31;
        }

        return spawn;
    }

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
    }

    private void PlaceBuildingOnGrid(Vector3 clickPoint)
    {
        var gridSpacingOffset = _gridObject.GetGridSpacingOffset();

        if (_gridObject.AppendElementInGrid(clickPoint, buildings[_buildingEnum]))
        {
            var xCount = Mathf.RoundToInt(clickPoint.x / gridSpacingOffset);
            var zCount = Mathf.RoundToInt(clickPoint.z / gridSpacingOffset);

            var result = new Vector3(
                xCount * gridSpacingOffset,
                1,
                zCount * gridSpacingOffset);

            var tmpBuilding = Instantiate(buildings[_buildingEnum], result, Quaternion.identity);
            tmpBuilding.GetComponent<MeshRenderer>().enabled = false;

            StartCoroutine(CheckPathBeforeBuild(tmpBuilding));
        }
    }

    private IEnumerator CheckPathBeforeBuild(GameObject tmpBuilding)
    {
        yield return new WaitForSeconds(0);

        foreach (var spawn in _spawns)
        {
            NavMesh.CalculatePath(spawn.position, _playerBasePosition, NavMesh.AllAreas, _path);
             
            if (_path.status == NavMeshPathStatus.PathInvalid || _path.status == NavMeshPathStatus.PathPartial)
            {
                //When you can't place a tower, do something
                Debug.Log("You can't build a building here.");
                
                _gridObject.RemoveElementInGrid(tmpBuilding.transform.position);
                Destroy(tmpBuilding);
                break;
            }
        }

        tmpBuilding.GetComponent<MeshRenderer>().enabled = true;
    }
    
    #endregion
}