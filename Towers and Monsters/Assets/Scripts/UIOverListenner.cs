using UnityEngine;
using UnityEngine.EventSystems;

public class UIOverListenner : MonoBehaviour
{
    public static bool isUIOverride { get; private set; }

    void Update()
    {
        // It will turn true if hovering any UI Elements
        isUIOverride = EventSystem.current.IsPointerOverGameObject();
    }
}
