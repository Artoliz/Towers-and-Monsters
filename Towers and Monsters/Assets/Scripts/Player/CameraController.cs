using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public float scrollSpeed = 1000f;

    private void Update()
    {
        if (!WavesManager.gameIsFinished && !PauseMenu.gameIsPaused)
        {
            var position = transform.position;

            position +=
                moveSpeed * new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            position += scrollSpeed * new Vector3(0, -Input.GetAxis("Mouse ScrollWheel"), 0);
            transform.position = position;
        }
    }
}