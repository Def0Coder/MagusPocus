using UnityEngine;
using UnityEngine.UI;

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

        // Crea gli slot fissi (uno per posizione)
        slots = new InventorySlot[inventoryManager.inventorySize];
        for (int i = 0; i < inventoryManager.inventorySize; i++)
        {
            GameObject slotGO = Instantiate(inventorySlotPrefab, itemsParent);
            InventorySlot slot = slotGO.GetComponent<InventorySlot>();
            slots[i] = slot;
            slot.slotIndex = i; // assegna l'indice

            // Imposta il listener in modo sicuro (evita problemi di closure)
            if (slot.removeButton != null)
            {
                slot.removeButton.onClick.RemoveAllListeners();
                int index = i; // copia locale per la closure
                slot.removeButton.onClick.AddListener(() => inventoryManager.RemoveItemAt(index));
            }
        }

        UpdateUI();
    }

    void OnDestroy()
    {
        if (inventoryManager != null) inventoryManager.onItemChangedCallback -= UpdateUI;
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            ItemData item = inventoryManager.items[i];
            int qty = inventoryManager.quantities[i];

            if (item != null)
            {
                slots[i].AddItem(item, qty);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}