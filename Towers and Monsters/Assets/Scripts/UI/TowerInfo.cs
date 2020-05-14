using UnityEngine;
using UnityEngine.UI;

public class TowerInfo : MonoBehaviour
{
    [SerializeField] private GameObject _unitHealth = null;
    [SerializeField] private GameObject _unitRepair = null;
    [SerializeField] private GameObject _unitUpgrade = null;
    [SerializeField] private GameObject _unitDamageToEnemy = null;
    [SerializeField] private GameObject _unitSpeedAttack = null;

    public void SetInformations(Informations.TowerData data)
    {
        _unitHealth.GetComponentInChildren<Text>().text = data._hp.ToString();
        _unitRepair.GetComponentInChildren<Text>().text = data._repair.ToString();
        _unitUpgrade.GetComponentInChildren<Text>().text = data._upgrade.ToString();
        _unitDamageToEnemy.GetComponentInChildren<Text>().text = data._damageToEnemy.ToString();
        _unitSpeedAttack.GetComponentInChildren<Text>().text = data._speedAttack.ToString() + "/s";
    }
}
