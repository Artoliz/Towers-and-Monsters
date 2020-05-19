using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public float rotationSpeed = 1f;

    private float xMin = -20.0f;
    private float xMax = 78.24f;
    private float yMin = 1.82f;
    private float yMax = 30.0f;
    private float zMin = -20.0f;
    private float zMax = 78.24f;

    private Vector3 _mouseReference;

    private void Update()
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            var position = transform.position;

            var tmp = transform;
            tmp.eulerAngles = new Vector3(0.0f, tmp.eulerAngles.y, tmp.eulerAngles.z);

            if (horizontal != 0.0f || vertical != 0.0f)
                tmp.Translate(horizontal * moveSpeed, 0, vertical * moveSpeed, Space.Self);

            transform.position = tmp.position;
            transform.eulerAngles = new Vector3(40.0f, tmp.eulerAngles.y, tmp.eulerAngles.z);

            position = transform.position;

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKey(KeyCode.E))
                position.y -= moveSpeed / 2.0f;
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
                position.y += moveSpeed / 2.0f;

            position.x = Mathf.Clamp(position.x, xMin, xMax);
            position.y = Mathf.Clamp(position.y, yMin, yMax);
            position.z = Mathf.Clamp(position.z, zMin, zMax);

            if (Input.GetMouseButtonDown(2))
                _mouseReference = Input.mousePosition;
            if (Input.GetMouseButton(2))
            {
                float offset = 0;
                if ((Input.mousePosition - _mouseReference).x > 0.0f)
                    offset = 1;
                else if ((Input.mousePosition - _mouseReference).x < 0.0f)
                    offset = -1;
                transform.Rotate(new Vector3(0.0f, offset * rotationSpeed, 0.0f), Space.World);
            }

            transform.position = position;
        }
    }
}