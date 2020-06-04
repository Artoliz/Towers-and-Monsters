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
    [SerializeField] private GameObject _unitEffectDamage = null;

    public void SetInformations(Informations.TowerData data)
    {
        char upArrow = '\u25B2';

        string hp = data._hp.ToString();
        string damage = data._damageToEnemy.ToString();
        string speedAttack = "0/s";
        string effectDamage = data._damageEffect.ToString() + "/s";
        if (data._speedAttack != 0)
            speedAttack = System.Math.Round((1.0f / data._speedAttack), 1).ToString() + "/s";

        if (data._isUpgrade)
        {
            hp += "\n<color=green>" + upArrow.ToString() + ((int)(data._maxHp * 2)).ToString() + "</color>";
            if (data._type != Tower.towerType.aoe)
            {
                damage += "\n<color=green>" + upArrow.ToString() + ((int)(data._damageToEnemy * 2)).ToString() + "</color>";
                speedAttack += "\n<color=green>" + upArrow.ToString() + "12.5%</color>";
            }
            if (data._type == Tower.towerType.aoe || data._type == Tower.towerType.effect)
            {
                effectDamage = data._damageEffect.ToString() + "/s\n<color=green>" + upArrow.ToString() + ((int)(data._damageEffect * 2)).ToString() + "/s </color>";
            }
        }

        _unitHealth.GetComponentInChildren<Text>().text = hp;

        _unitRepair.GetComponentInChildren<Text>().text = data._repair.ToString();

        _unitUpgrade.GetComponentInChildren<Text>().text = data._upgrade.ToString();

        _unitDamageToEnemy.GetComponentInChildren<Text>().text = damage;

        _unitSpeedAttack.GetComponentInChildren<Text>().text = speedAttack;

        _unitDestroy.GetComponentInChildren<Text>().text = "+" + data._sell.ToString();

        _unitEffectDamage.GetComponentInChildren<Text>().text = effectDamage;
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
