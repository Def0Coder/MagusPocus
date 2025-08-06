using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData itemData;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PickUp();
        }
    }

    void PickUp()
    {
        Debug.Log("Picking up " + itemData.name);
        bool wasPickedUp = InventoryManager.instance.AddItem(itemData);

        if (wasPickedUp)
        {
            Destroy(gameObject);
        }
    }
}
