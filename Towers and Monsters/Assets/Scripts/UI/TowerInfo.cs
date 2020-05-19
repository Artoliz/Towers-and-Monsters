using UnityEngine;
using UnityEngine.UI;

public class TowerInfo : MonoBehaviour
{
    [SerializeField] private GameObject _unitHealth = null;
    [SerializeField] private GameObject _unitRepair = null;
    [SerializeField] private GameObject _unitUpgrade = null;
    [SerializeField] private GameObject _unitDamageToEnemy = null;
    [SerializeField] private GameObject _unitSpeedAttack = null;
    [SerializeField] private GameObject _unitDestroy = null;

    public void SetInformations(Informations.TowerData data)
    {
        _unitHealth.GetComponentInChildren<Text>().text = data._hp.ToString();
        _unitRepair.GetComponentInChildren<Text>().text = data._repair.ToString();
        _unitUpgrade.GetComponentInChildren<Text>().text = data._upgrade.ToString();
        _unitDamageToEnemy.GetComponentInChildren<Text>().text = data._damageToEnemy.ToString();
        _unitSpeedAttack.GetComponentInChildren<Text>().text = data._speedAttack.ToString() + "/s";
    }

    public void SetListener(Tower tower)
    {
        _unitUpgrade.GetComponent<Button>().onClick.RemoveAllListeners();
        _unitRepair.GetComponent<Button>().onClick.RemoveAllListeners();
        _unitDestroy.GetComponent<Button>().onClick.RemoveAllListeners();

        _unitUpgrade.GetComponent<Button>().onClick.AddListener(tower.Upgrade);
        _unitRepair.GetComponent<Button>().onClick.AddListener(tower.Repair);
        _unitDestroy.GetComponent<Button>().onClick.AddListener(tower.Destroy);
    }
}
