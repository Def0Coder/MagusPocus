using UnityEngine;



public enum ChestRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

[CreateAssetMenu(fileName = "New Chest", menuName = "Loot/Chest Data")]
public class ChestData : ScriptableObject
{
   
    public string chestName;
    public ChestRarity rarity;

    [System.Serializable]
    public struct LootItem
    {
        public ItemData itemData;
       // public float dropChance;

        public GameObject itemPrefab;
        [Range(0, 1)] public float dropChance;  // 0.0 to 1.0
    }

  

    

    public LootItem[] possibleLoot;
}


