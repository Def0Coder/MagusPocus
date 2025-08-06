using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public List<ItemData> items = new List<ItemData>();
    public List<int> quantities = new List<int>();
    public int inventorySize;

    

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple InventoryManagers found!");
            return;
        }
        instance = this;
    }

    public bool AddItem(ItemData item)
    {



            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == item && quantities[i] < item.maxStack)
                {
                    quantities[i]++;
                    onItemChangedCallback?.Invoke();
                    return true;
                }
            }

            if (items.Count >= inventorySize)
            {
                Debug.Log("Inventory full.");
                return false;
            }

            items.Add(item);
            quantities.Add(1);
            onItemChangedCallback?.Invoke();
            return true;
        

        
        
            
       
       
    }

    public void RemoveItem(ItemData item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == item)
            {
                quantities[i]--;
                if (quantities[i] <= 0)
                {
                    items.RemoveAt(i);
                    quantities.RemoveAt(i);
                 
                   // item.count--;

                }
                onItemChangedCallback?.Invoke();

                

                return;
            }
        }
    }
}