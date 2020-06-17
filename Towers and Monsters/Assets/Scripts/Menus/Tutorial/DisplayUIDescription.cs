using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayUIDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region SerializableVariables

    [SerializeField] private GameObject description;
    
    [SerializeField] private Image uiInGame;
    [SerializeField] private Sprite mainImageUi;
    [SerializeField] private Sprite newImageUi;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        description.SetActive(false);
    }

    #endregion

    public void OnPointerEnter(PointerEventData eventData)
    {
        description.SetActive(true);

        if (uiInGame != null)
        {
            uiInGame.sprite = newImageUi;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        description.SetActive(false);
        
        if (uiInGame != null)
        {
            uiInGame.sprite = mainImageUi;
        }
    }
}