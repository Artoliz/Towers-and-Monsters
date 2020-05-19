using UnityEngine;

public class Highlight : MonoBehaviour
{
    private Renderer _renderer;
    private Transform _pivot;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _pivot = transform.Find("Pivot");
    }

    private void OnMouseEnter()
    {
        if (!WavesManager.GameIsFinished && !PauseMenu.GameIsPaused)
        {
            if (Builder.isBuilding && !Grid.Instance.IsPositionBlocked(_pivot.position))
                _renderer.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
        }
    }

    private void OnMouseExit()
    {
        if (!Grid.Instance.IsPositionBlocked(_pivot.position))
            _renderer.material.shader = Shader.Find("Diffuse");
    }
}