using UnityEngine;

public class Highlight : MonoBehaviour
{
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnMouseEnter()
    {
        if (!WavesManager.gameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (Builder.isBuilding && !Grid.Instance.IsPositionBlocked(transform.position))
                _renderer.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
        }
    }

    private void OnMouseExit()
    {
        if (!Grid.Instance.IsPositionBlocked(transform.position))
            _renderer.material.shader = Shader.Find("Diffuse");
    }
}