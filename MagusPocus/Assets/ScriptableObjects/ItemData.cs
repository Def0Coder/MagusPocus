using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName = "New Item";
    public Sprite icon = null;
    public string description = "Item Description";
    
    public int maxStack;

    public int count = 0;



    public int boostHealth;
    public int boostDamage;

  
}
