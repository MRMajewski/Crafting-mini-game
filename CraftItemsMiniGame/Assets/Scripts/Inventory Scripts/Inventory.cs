using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public ItemDatabase itemDatabase; 

    private List<ItemData> inventoryItems = new List<ItemData>();

    public int maxInventorySize = 9;

    [SerializeField]
    private Vector3 dropPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool AddItem(string itemID)
    {
        ItemData itemToAdd = itemDatabase.GetItemByID(itemID);

        if (itemToAdd != null)
        {
            if (inventoryItems.Count < maxInventorySize)
            {
                inventoryItems.Add(itemToAdd);
                Debug.Log($"{itemToAdd.itemName} zosta� dodany do ekwipunku.");
                return true;
            }
            else
            {
                Debug.LogWarning("Ekwipunek jest pe�ny!");
            }
        }
        else
        {
            Debug.LogWarning($"Nie znaleziono przedmiotu z ID: {itemID}");
        }

        return false;
    }

    public bool RemoveItem(string itemID)
    {
        ItemData itemToRemove = inventoryItems.Find(item => item.itemID == itemID);

        if (itemToRemove != null)
        {
            inventoryItems.Remove(itemToRemove);
            Debug.Log($"{itemToRemove.itemName} zosta� usuni�ty z ekwipunku.");
            return true;
        }
        else
        {
            Debug.LogWarning($"Przedmiot z ID {itemID} nie znajduje si� w ekwipunku.");
        }

        return false;
    }

    public List<ItemData> GetInventoryItems()
    {
        return inventoryItems;
    }
    public void DropItem(string itemID)
    {
        ItemData itemToDrop = inventoryItems.Find(item => item.itemID == itemID);

        if (itemToDrop != null)
        {
            Instantiate(itemToDrop.prefab, dropPosition, Quaternion.identity);
            RemoveItem(itemID);
            Debug.Log($"{itemToDrop.itemName} zosta� wyrzucony na scen�.");
        }
        else
        {
            Debug.LogWarning($"Przedmiot z ID {itemID} nie znajduje si� w ekwipunku.");
        }
    }

    public List<ItemData> GetItems()
    {
        return inventoryItems;
    }
}
