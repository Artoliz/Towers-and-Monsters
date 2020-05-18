using System.Collections;
using System.Collections.Generic;
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
        if (Builder.isBuilding && !Grid.Instance.IsPositionBlocked(this.transform.position))
            _renderer.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
    }

    private void OnMouseExit()
    {
        if (Builder.isBuilding && !Grid.Instance.IsPositionBlocked(this.transform.position))
            _renderer.material.shader = Shader.Find("Diffuse");
    }
}
