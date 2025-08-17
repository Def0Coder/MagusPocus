using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    public PlayerStats playerStats;
    public TMP_Text attackText;
   // public Slider healthBar;
    public TMP_Text healthbar;

    void Update()
    {
        if (playerStats != null)
        {
            attackText.text = $"Attacco: {playerStats.CurrentDamage}";
            healthbar.text = $"HP:  {playerStats.CurrentHealth}";
           // healthBar.maxValue = playerStats.MaxHealth;
           // healthBar.value = playerStats.CurrentHealth;

        }
    }
}
