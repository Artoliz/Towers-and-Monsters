using UnityEngine;

public class TowerHP : MonoBehaviour
{
    #region PublicVariables

    public int castleHp = 20;

    #endregion

    #region MonoBehaviour

    private void Update()
    {
        if (castleHp <= 0)
        {
            var o = gameObject;

            o.tag = "Castle_Destroyed";
            Destroy(o);
        }
    }

    #endregion

    #region PublicMethods

    public void Damage(int damage2)
    {
        castleHp -= damage2;
    }

    #endregion
}