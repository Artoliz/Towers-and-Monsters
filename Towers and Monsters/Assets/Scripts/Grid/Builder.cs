using UnityEngine;

public class Builder : MonoBehaviour
{
    #region PrivateVariables

    private Camera _camera;
    private bool _isCameraNotNull;

    private Grid _gridObject;

    #endregion

    #region PublicVariables

    public GameObject tower;

    #endregion

    #region MonoBehavior

    private void Awake()
    {
        _camera = Camera.main;
        _isCameraNotNull = _camera != null;

        _gridObject = FindObjectOfType<Grid>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_isCameraNotNull)
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var click))
                {
                    PlaceTower(click.point);
                }
            }
        }
    }

    #endregion

    #region PrivateFunctions

    private void PlaceTower(Vector3 clickPoint)
    {
        var gridSpacingOffset = _gridObject.gridSpacingOffset;
        
        if (_gridObject.AppendElementInGrid(clickPoint, tower))
        {
            var xCount = Mathf.RoundToInt(clickPoint.x / gridSpacingOffset);
            var zCount = Mathf.RoundToInt(clickPoint.z / gridSpacingOffset);

            Vector3 result = new Vector3(
                xCount * gridSpacingOffset,
                1,
                zCount * gridSpacingOffset);

            Instantiate(tower).transform.position = result;
        }
    }

    #endregion
}