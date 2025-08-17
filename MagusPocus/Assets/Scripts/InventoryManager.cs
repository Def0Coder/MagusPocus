using JetBrains.Annotations;
using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public ItemData[] items;
    public int[] quantities;
    public int inventorySize = 20;

    public Action onItemChangedCallback;

    public PlayerStats playerStats;

    void Awake()
    {
        playerStats = FindAnyObjectByType<PlayerStats>();

        if (instance != null)
        {
            Debug.LogWarning("Multiple InventoryManagers found!");
            Destroy(this);
            return;
        }

        instance = this;

        // Inizializza gli array se necessari
        if (items == null || items.Length != inventorySize)
        {
            items = new ItemData[inventorySize];
            quantities = new int[inventorySize];
        }
    }

    // Aggiunge provando prima a stackare, poi cercando uno slot vuoto
    public bool AddItem(ItemData item)
    {
        // Prova a stackare
        for (int i = 0; i < inventorySize; i++)
        {
            if (items[i] != null && items[i] == item && quantities[i] < item.maxStack)
            {
                quantities[i]++;
                onItemChangedCallback?.Invoke();
                return true;
            }
        }

        // Cerca slot vuoto
        for (int i = 0; i < inventorySize; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                quantities[i] = 1;
                onItemChangedCallback?.Invoke();

                // Aggiorna statistiche player
                //if (playerStats != null)
                    playerStats.AddItem(item);

                return true;
            }
        }

        Debug.Log("Inventory full.");
        return false;
    }

    public void RemoveItemAt(int index)
    {
        if (index < 0 || index >= inventorySize)
        {
            Debug.LogWarning($"RemoveItemAt: index {index} out of range.");
            return;
        }

        if (items[index] == null)
        {
            Debug.Log($"RemoveItemAt: slot {index} is already empty.");
            return;
        }

        Debug.Log($"Removing one '{items[index].name}' from slot {index} (before qty = {quantities[index]})");

        ItemData removedItem = items[index];
        quantities[index]--;

        if (quantities[index] <= 0)
        {
            items[index] = null;
            quantities[index] = 0;

            // Aggiorna statistiche player
            if (playerStats != null && removedItem != null)
                playerStats.RemoveItem(removedItem);
        }

        onItemChangedCallback?.Invoke();
    }
}