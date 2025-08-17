using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyData enemyData;

    public int CurrentDamage { get; private set; }
    public int CurrentHealth { get; private set; }

    public void Init()
    {
        CurrentDamage = enemyData.baseDamage;
        CurrentHealth = enemyData.baseHealth;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
    }

    public bool IsDead() => CurrentHealth <= 0;
}
