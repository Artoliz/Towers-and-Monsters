using UnityEngine;
using UnityEngine.UI;

public class WallInfo : MonoBehaviour
{
    [SerializeField] private GameObject _unitHealth = null;
    [SerializeField] private GameObject _unitDestroy = null;

    public void SetInformations(Informations.WallData data)
    {
        _unitHealth.GetComponentInChildren<Text>().text = data._hp.ToString();

        _unitDestroy.GetComponentInChildren<Text>().text = data._sell.ToString();
    }

    public void SetListener(Wall wall)
    {
        _unitDestroy.GetComponent<Button>().onClick.RemoveAllListeners();

        _unitDestroy.GetComponent<Button>().onClick.AddListener(wall.Destroy);
    }
}
