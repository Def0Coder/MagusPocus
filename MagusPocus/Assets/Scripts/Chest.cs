using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Chest : MonoBehaviour
{
    public ChestData chestData;
    private bool opened = false;

    public Sprite openedSprite;

    public bool isUIOpen = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (opened) return;
        if (other.CompareTag("Player"))
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        opened = true;
        Debug.Log($"Opening {chestData.chestName} ({chestData.rarity})");

       
        var shuffledLoot = new List<ChestData.LootItem>(chestData.possibleLoot);
        ShuffleList(shuffledLoot);

        foreach (var loot in shuffledLoot)
        {
            if (Random.value <= loot.dropChance)
            {
                Debug.Log("Trovato oggetto " + loot.itemData.name);

                ShowLoot();

                bool added = InventoryManager.instance.AddItem(loot.itemData);
                if (!added)
                {
                    Debug.Log("Inventario pieno! Oggetto non aggiunto.");
                }

                break; 
            }
        }

        GetComponent<SpriteRenderer>().sprite = openedSprite;
        GetComponent<Collider2D>().enabled = false;
    }

    // Utility per mescolare
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    public void ShowLoot()
    {


    }




}