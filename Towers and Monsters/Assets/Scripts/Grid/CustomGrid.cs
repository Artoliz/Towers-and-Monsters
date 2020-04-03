using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    #region PrivateVariables

    private Vector3 _truePos;
    private Vector3 _targetPos;

    #endregion

    #region PublicVariables

    public float gridSize;
    
    public GameObject target;
    public GameObject structure;

    #endregion

    #region MonoBehavior

    private void LateUpdate()
    {
        _targetPos = target.transform.position;

        _truePos.x = Mathf.Floor(_targetPos.x / gridSize) * gridSize;
        _truePos.y = 0;
        _truePos.z = Mathf.Floor(_targetPos.z / gridSize) * gridSize;

        structure.transform.position = _truePos;
    }

    #endregion
    
}
