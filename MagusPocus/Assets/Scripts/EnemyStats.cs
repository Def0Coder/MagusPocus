using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyData enemyData;

    public int baseVelocity;

    public int CurrentDamage { get; private set; }
    public int CurrentHealth { get; private set; }

    public int CurrentShields { get; set; }

    public int CurrentVelocity { get; set; }


    public void Init()
    {
        CurrentDamage = enemyData.baseDamage;
        CurrentHealth = enemyData.baseHealth;
        CurrentShields = enemyData.baseShields;
    }

    public void TakeDamage(int amount)
    {
        if (CurrentShields > 0)
            CurrentShields = Mathf.Max(CurrentShields - amount, 0);

        else
            CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);

    }

    public bool IsDead() => CurrentHealth <= 0;
}
