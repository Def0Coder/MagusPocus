using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventorySlotPrefab;

    private InventoryManager inventoryManager;
    private InventorySlot[] slots;

    void Start()
    {
        inventoryManager = InventoryManager.instance;
        inventoryManager.onItemChangedCallback += UpdateUI;

        // Crea un numero fisso di slot
        slots = new InventorySlot[inventoryManager.inventorySize];
        for (int i = 0; i < inventoryManager.inventorySize; i++)
        {
            GameObject slotGO = Instantiate(inventorySlotPrefab, itemsParent);
            slots[i] = slotGO.GetComponent<InventorySlot>();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventoryManager.items.Count)
            {
                ItemData item = inventoryManager.items[i];
                int qty = inventoryManager.quantities[i];
                slots[i].AddItem(item, qty);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}