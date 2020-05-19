using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Builder : MonoBehaviour
{
    #region PrivateVariables

    private Camera _camera;
    private bool _isCameraNotNull;

    private NavMeshPath _path;
    
    private List<Transform> _spawns = new List<Transform>();
    
    private Grid _gridObject;

    private Buildings _buildingEnum = Buildings.None;

    private const int LayerBuildingMask = 1 << 12;

    private float _timeSinceStart;
    private float _timeWhenPause;
    private bool _destroyIsPaused;
    private GameObject _spriteCrossToDelete;

    [SerializeField] private float spriteCrossDuration;
    
    [SerializeField] private Sprite[] _buildingImages = null;
    [SerializeField] private Image _buildingSelected = null;
    
    [SerializeField] private GameObject noBuildingPossible;
    [SerializeField] private GameObject errorMessage;

    #endregion

    #region PublicVariables

    public GameObject playerBase;
    
    public GameObject[] buildingsPrefab;
    
    public Dictionary<Buildings, GameObject> buildings;

    public static bool isBuilding = false;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        _camera = Camera.main;
        _isCameraNotNull = _camera != null;

        _path = new NavMeshPath();

        _gridObject = FindObjectOfType<Grid>();

        InitBuildings();

        _buildingSelected.transform.localScale /= 6;

        errorMessage.SetActive(false);
    }

    private void Start()
    {
        _spawns = EnemiesSpawns.Instance.GetEnemiesSpawns();
    }

    private void Update()
    {
        if (!WavesManager.gameIsFinished && !PauseMenu.GameIsPaused)
        {
            SelectedBuilding();

            if (_buildingEnum != Buildings.None)
                isBuilding = true;
            else
                isBuilding = false;

            DisplayBuildingSelected();

            if (Input.GetMouseButtonDown(0) && _buildingEnum != Buildings.None)
            {
                if (_isCameraNotNull)
                {
                    var ray = _camera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out var click, 1000, LayerBuildingMask))
                    {
                        PlaceBuildingOnGrid(click.point);
                    }
                }
            }
        }
        if (PauseMenu.GameIsPaused && ! _destroyIsPaused)
        {
             _destroyIsPaused = true;
            _timeWhenPause = Time.unscaledTime - _timeSinceStart;
            CancelInvoke(nameof(DestroySpriteCross));
        }
        if (!PauseMenu.GameIsPaused &&  _destroyIsPaused)
        {
             _destroyIsPaused = false;
            Invoke(nameof(DestroySpriteCross), spriteCrossDuration - _timeWhenPause);
        }
    }

    #endregion

    #region PrivateFunctions

    private void DisplayBuildingSelected()
    {
        if (_buildingEnum != Buildings.None)
        {
            if (_buildingSelected.sprite == null || !_buildingSelected.sprite.name.Contains(_buildingEnum.ToString()))
            {
                foreach (Sprite building in _buildingImages)
                {
                    if (building.name.Contains(_buildingEnum.ToString()))
                    {
                        _buildingSelected.gameObject.SetActive(true);
                        _buildingSelected.sprite = building;
                        _buildingSelected.SetNativeSize();
                        _buildingSelected.color = new Color(1, 1, 1, 0.3f);
                        break;
                    }
                }
            }
            _buildingSelected.transform.position = Input.mousePosition;
        } else if (_buildingSelected.sprite != null)
        {
            _buildingSelected.sprite = null;
            _buildingSelected.gameObject.SetActive(false);
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

        if (_gridObject.AppendElementInGrid(clickPoint, buildings[_buildingEnum]))
        {
            var xCount = Mathf.RoundToInt(clickPoint.x / gridSpacingOffset);
            var zCount = Mathf.RoundToInt(clickPoint.z / gridSpacingOffset);

            var result = new Vector3(
                xCount * gridSpacingOffset,
                0,
                zCount * gridSpacingOffset);

            var tmpBuilding = Instantiate(buildings[_buildingEnum], result, Quaternion.identity);

            foreach (var mesh in tmpBuilding.GetComponentsInChildren<MeshRenderer>())
                mesh.enabled = false;

            StartCoroutine(CheckPathBeforeBuild(tmpBuilding));
        }
    }

    private IEnumerator CheckPathBeforeBuild(GameObject tmpBuilding)
    {
        yield return new WaitForSeconds(0);

        foreach (var spawn in _spawns)
        {
            
            NavMesh.CalculatePath(spawn.position, playerBase.transform.position, NavMesh.AllAreas, _path);

            if (_path.status == NavMeshPathStatus.PathInvalid || _path.status == NavMeshPathStatus.PathPartial)
            {
                var position = tmpBuilding.transform.position;
                
                var spriteCannotBuild = position;
                spriteCannotBuild.y = 0.005f;
                var cannotBuildRotation = Quaternion.Euler(90, 0, 0);

                _spriteCrossToDelete = Instantiate(noBuildingPossible, spriteCannotBuild, cannotBuildRotation);
                _timeSinceStart = Time.unscaledTime;
                Invoke(nameof(DestroySpriteCross), spriteCrossDuration);

                _gridObject.RemoveElementInGrid(position);
                Destroy(tmpBuilding);
                break;
            }
        }

        if ((tmpBuilding.GetComponent<Tower>() != null && tmpBuilding.GetComponent<Tower>().cost > GameManager.Instance.GetGolds()) ||
            (tmpBuilding.GetComponent<Wall>() != null && tmpBuilding.GetComponent<Wall>().cost > GameManager.Instance.GetGolds()))
        {
            StartCoroutine(DisplayError("Not enough golds !"));
            _gridObject.RemoveElementInGrid(tmpBuilding.transform.position);
            Destroy(tmpBuilding);
        } else
        {
            if (tmpBuilding.GetComponent<Tower>() != null)
                GameManager.Instance.RemoveGolds(tmpBuilding.GetComponent<Tower>().cost);
            else if (tmpBuilding.GetComponent<Wall>() != null)
            {
                GameManager.Instance.RemoveGolds(tmpBuilding.GetComponent<Wall>().cost);
                tmpBuilding.GetComponent<Wall>().PlaceWallIntersections();
            }
            foreach (var mesh in tmpBuilding.GetComponentsInChildren<MeshRenderer>())
                mesh.enabled = true;
        }
    }

    IEnumerator DisplayError(string message)
    {
        errorMessage.GetComponent<Text>().text = message;
        errorMessage.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        errorMessage.SetActive(false);
    }

    private void DrawPathDebug()
    {
        for (int i = 0; i < _path.corners.Length - 1; i++)
            Debug.DrawLine(_path.corners[i], _path.corners[i + 1], Color.red);
    }
    
    private void DestroySpriteCross()
    {
        Destroy(_spriteCrossToDelete);
    }

    #endregion
}