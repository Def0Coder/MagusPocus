using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI quantityText;
    public Button removeButton;

    private ItemData item;

    public void AddItem(ItemData newItem, int quantity)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;

        quantityText.text = quantity > 1 ? quantity.ToString() : "";

        if (removeButton != null)
            removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        quantityText.text = "";

        if (removeButton != null)
            removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        if (item != null)
            InventoryManager.instance.RemoveItem(item);
    }
}