using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Builder : MonoBehaviour
{
    #region PrivateVariables

    private Camera _camera;
    private bool _isCameraNotNull;

    private NavMeshPath _path;
    
    private Grid _gridObject;

    private Buildings _buildingEnum = Buildings.None;

    private const int LayerBuildingMask = 1 << 12;

    [SerializeField] private float spriteCrossDuration = 0.0f;
    
    [SerializeField] private Sprite[] buildingImages = null;
    [SerializeField] private Image buildingSelected = null;
    
    [SerializeField] private GameObject noBuildingPossible = null;

    private Coroutine _checkBeforeBuild;

    #endregion

    #region PublicVariables

    public GameObject[] buildingsPrefab;
    
    public Dictionary<Buildings, GameObject> buildings;

    public static bool IsBuilding;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        _camera = Camera.main;
        _isCameraNotNull = _camera != null;

        _path = new NavMeshPath();

        _gridObject = FindObjectOfType<Grid>();

        InitBuildings();

        buildingSelected.transform.localScale /= 6;

        NavMesh.pathfindingIterationsPerFrame = 500;
    }

    private void Update()
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
        {
            SelectedBuilding();

            IsBuilding = _buildingEnum != Buildings.None;

            DisplayBuildingSelected();

            if (Input.GetMouseButtonDown(0) && _buildingEnum != Buildings.None)
            {
                if (_isCameraNotNull && !EventSystem.current.IsPointerOverGameObject(-1))
                {
                    var ray = _camera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out var click, 1000, LayerBuildingMask))
                    {
                        PlaceBuildingOnGrid(click.point);
                    }
                }
            } else if (Input.GetMouseButtonDown(1) && _buildingEnum != Buildings.None)
            {
                _buildingEnum = Buildings.None;
            }
        }
    }

    #endregion

    #region PrivateFunctions

    private void DisplayBuildingSelected()
    {
        if (_buildingEnum != Buildings.None)
        {
            if (buildingSelected.sprite == null || !buildingSelected.sprite.name.Contains(_buildingEnum.ToString()))
            {
                foreach (Sprite building in buildingImages)
                {
                    if (building.name.Contains(_buildingEnum.ToString()))
                    {
                        buildingSelected.gameObject.SetActive(true);
                        buildingSelected.sprite = building;
                        buildingSelected.SetNativeSize();
                        buildingSelected.color = new Color(1, 1, 1, 0.3f);
                        break;
                    }
                }
            }
            buildingSelected.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + (buildingSelected.sprite.rect.width / 8), 0);
        } else if (buildingSelected.sprite != null)
        {
            buildingSelected.sprite = null;
            buildingSelected.gameObject.SetActive(false);
        }
    }

    private void InitBuildings()
    {
        buildings = new Dictionary<Buildings, GameObject>
        {
            [Buildings.Wall] = buildingsPrefab[0],
            [Buildings.HeavyBullet] = buildingsPrefab[1],
            [Buildings.LongRange] = buildingsPrefab[2],
            [Buildings.EffectShot] = buildingsPrefab[3],
            [Buildings.AoeShot] = buildingsPrefab[4]
        };
    }

    public void SelectedBuilding(string building)
    {
        if (building == Buildings.Wall.ToString())
        {
            _buildingEnum = _buildingEnum == Buildings.Wall ? Buildings.None : Buildings.Wall;
        }
        else if (building == Buildings.HeavyBullet.ToString())
        {
            _buildingEnum = _buildingEnum == Buildings.HeavyBullet ? Buildings.None : Buildings.HeavyBullet;
        }
        else if (building == Buildings.LongRange.ToString())
        {
            _buildingEnum = _buildingEnum == Buildings.LongRange ? Buildings.None : Buildings.LongRange;
        }
        else if (building == Buildings.EffectShot.ToString())
        {
            _buildingEnum = _buildingEnum == Buildings.EffectShot ? Buildings.None : Buildings.EffectShot;
        }
        else if (building == Buildings.AoeShot.ToString())
        {
            _buildingEnum = _buildingEnum == Buildings.AoeShot ? Buildings.None : Buildings.AoeShot;
        }
    }

    private void SelectedBuilding()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _buildingEnum = _buildingEnum == Buildings.Wall ? Buildings.None : Buildings.Wall;
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
            _buildingEnum = _buildingEnum == Buildings.HeavyBullet ? Buildings.None : Buildings.HeavyBullet;
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
            _buildingEnum = _buildingEnum == Buildings.LongRange ? Buildings.None : Buildings.LongRange;

        if (Input.GetKeyDown(KeyCode.Alpha4))
            _buildingEnum = _buildingEnum == Buildings.EffectShot ? Buildings.None : Buildings.EffectShot;

        if (Input.GetKeyDown(KeyCode.Alpha5))
            _buildingEnum = _buildingEnum == Buildings.AoeShot ? Buildings.None : Buildings.AoeShot;
    }

    private void PlaceBuildingOnGrid(Vector3 clickPoint)
    {
        var gridSpacingOffset = _gridObject.GetGridSpacingOffset();

        var xCount = Mathf.RoundToInt(clickPoint.x / gridSpacingOffset);
        var zCount = Mathf.RoundToInt(clickPoint.z / gridSpacingOffset);

        var result = new Vector3(xCount * gridSpacingOffset, 0, zCount * gridSpacingOffset);

        if (Grid.Instance.IsEnemyOnGrid(xCount, zCount))
        {
            DrawCannotBuildHere(result);
            return;
        }

        Grid.Instance._pathfinder.SetBlockedPosition(xCount, zCount);

        if (Grid.Instance._pathfinder.AStarSearch())
        {
            if (_gridObject.AppendElementInGrid(clickPoint, buildings[_buildingEnum]))
            {
                var tmpBuilding = Instantiate(buildings[_buildingEnum], result, Quaternion.identity);

                if (tmpBuilding != null)
                {
                    if ((tmpBuilding.GetComponentInChildren<Tower>() != null && tmpBuilding.GetComponentInChildren<Tower>().cost > GameManager.Instance.GetGolds()) ||
                        (tmpBuilding.GetComponent<Wall>() != null && tmpBuilding.GetComponent<Wall>().cost > GameManager.Instance.GetGolds()))
                    {
                        StartCoroutine(GameManager.DisplayError("Not enough golds !"));
                        _gridObject.RemoveElementInGrid(tmpBuilding.transform.position);
                        Destroy(tmpBuilding);
                    }
                    else
                    {
                        if (tmpBuilding != null && tmpBuilding.GetComponentInChildren<Tower>() != null)
                            GameManager.Instance.RemoveGolds(tmpBuilding.GetComponentInChildren<Tower>().cost);
                        else if (tmpBuilding.GetComponent<Wall>() != null)
                        {
                            GameManager.Instance.RemoveGolds(tmpBuilding.GetComponent<Wall>().cost);
                            tmpBuilding.GetComponent<Wall>().PlaceWallIntersections();
                        }
                        foreach (var mesh in tmpBuilding.GetComponentsInChildren<MeshRenderer>())
                            mesh.enabled = true;

                        GameManager.Instance.ReDrawPathFindingForAll();
                    }
                }
            }
        }
        else
        {
            Grid.Instance._pathfinder.RemoveBlockedPosition(xCount, zCount);

            DrawCannotBuildHere(result);
        }
    }

    private void DrawCannotBuildHere(Vector3 pos)
    {
        var spriteCannotBuild = pos;
        spriteCannotBuild.y = 0.005f;
        var cannotBuildRotation = Quaternion.Euler(90, 0, 0);

        var spriteCrossDelete = Instantiate(noBuildingPossible, spriteCannotBuild, cannotBuildRotation);
        Destroy(spriteCrossDelete, spriteCrossDuration);
    }

    #endregion
}