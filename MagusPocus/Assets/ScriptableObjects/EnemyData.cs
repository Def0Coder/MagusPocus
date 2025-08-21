using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int baseDamage;
    public int baseHealth;
    public int baseShields;
    public int baseVelocity;
    public Sprite sprite;
}