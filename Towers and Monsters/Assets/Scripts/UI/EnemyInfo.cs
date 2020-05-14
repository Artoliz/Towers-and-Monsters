using UnityEngine;
using UnityEngine.UI;

public class EnemyInfo : MonoBehaviour
{
    [SerializeField] private GameObject _unitHealth = null;
    [SerializeField] private GameObject _unitGolds = null;
    [SerializeField] private GameObject _unitDamageToBase = null;
    [SerializeField] private GameObject _unitDamageToTower = null;
    [SerializeField] private GameObject _unitSpeed = null;
    [SerializeField] private GameObject _unitSpeedAttack = null;

    public void SetInformations(Informations.EnemyData data)
    {
        _unitHealth.GetComponentInChildren<Text>().text = data._hp.ToString();
        _unitGolds.GetComponentInChildren<Text>().text = data._golds.ToString();
        _unitDamageToBase.GetComponentInChildren<Text>().text = data._damageToBase.ToString();
        _unitDamageToTower.GetComponentInChildren<Text>().text = data._damageToTower.ToString();
        _unitSpeed.GetComponentInChildren<Text>().text = data._speed.ToString() + "/s";
        _unitSpeedAttack.GetComponentInChildren<Text>().text = data._speedAttack.ToString() + "/s";
    }
}
