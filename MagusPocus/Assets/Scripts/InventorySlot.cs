using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI quantityText;
    public Button removeButton;

    [HideInInspector] public int slotIndex = -1;

    private ItemData item;

    public void AddItem(ItemData newItem, int quantity)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        quantityText.text = quantity > 1 ? quantity.ToString() : "";
        if (removeButton != null) removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        quantityText.text = "";
        if (removeButton != null) removeButton.interactable = false;
    }

    // opzionale: se preferisci usare il metodo sul prefab invece di wiring da codice
    public void OnRemoveButton()
    {
        Debug.Log($"Slot {slotIndex} remove pressed (item: {(item != null ? item.name : "null")})");
        InventoryManager.instance.RemoveItemAt(slotIndex);
    }
}