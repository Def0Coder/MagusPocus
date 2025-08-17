using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{

    [Header("Valori base")]
    public int baseDamage = 10;
    public int baseHealth = 100;

    public int CurrentDamage { get; private set; }
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    private List<ItemData> equippedItems = new List<ItemData>();

    void Start()
    {
        RecalculateStats();
        CurrentHealth = MaxHealth;
    }

    public void AddItem(ItemData item)
    {
        equippedItems.Add(item);
        Debug.Log("Equipped " + item.name);

        RecalculateStats();

        // Se la vita attuale è minore della nuova max, la aumentiamo di conseguenza
        CurrentHealth = MaxHealth;
    }

    public void RemoveItem(ItemData item)
    {
        equippedItems.Remove(item);
        RecalculateStats();

        // Se la vita attuale supera la nuova max, la riduciamo
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
    }

    private void RecalculateStats()
    {
        // Reset ai valori base
        CurrentDamage = baseDamage;
        MaxHealth = baseHealth;

        // Aggiungi i bonus degli oggetti equipaggiati
        foreach (var item in equippedItems)
        {
            CurrentDamage += item.boostDamage;
            MaxHealth += item.boostHealth;
        }
    }
}
