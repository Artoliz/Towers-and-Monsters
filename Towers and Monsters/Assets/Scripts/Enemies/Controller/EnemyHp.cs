using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    public int enemyHp = 30;

    public void Dmg(int damage)
    {
        enemyHp -= damage;
    }

    private void Update()
    {
        if (enemyHp <= 0)
        {
            gameObject.tag = "Dead";
        }
    }
}