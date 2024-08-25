using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    // Dictionary mapping item ID to ItemData
    private Dictionary<string, ItemData> itemDictionary = new Dictionary<string, ItemData>();

    // List of all items in the database, set in the Inspector
    public List<ItemData> allItems;

    // Initialize the dictionary with the item data
    private void OnEnable()
    {
        InitializeDatabase();
    }

    // Method to initialize the dictionary
    private void InitializeDatabase()
    {
        itemDictionary.Clear(); // Clear the dictionary before populating it

        foreach (ItemData item in allItems)
        {
            if (!itemDictionary.ContainsKey(item.itemID))
            {
                itemDictionary.Add(item.itemID, item);
            }
            else
            {
                Debug.LogWarning($"Duplicate item ID found: {item.itemID}. Item {item.itemName} not added.");
            }
        }
    }

    // Retrieve item by ID
    public ItemData GetItemByID(string itemID)
    {
        if (itemDictionary.TryGetValue(itemID, out ItemData item))
        {
            return item;
        }

        Debug.LogWarning($"Item with ID {itemID} not found");
        return null;
    }

    // Add a new item to the database (optionally)
    public void AddItem(ItemData newItem)
    {
        if (!itemDictionary.ContainsKey(newItem.itemID))
        {
            itemDictionary.Add(newItem.itemID, newItem);
            allItems.Add(newItem);
        }
        else
        {
            Debug.LogWarning($"Item with ID {newItem.itemID} already exists");
        }
    }

    // Remove an item from the database (optionally)
    public void RemoveItem(string itemID)
    {
        if (itemDictionary.ContainsKey(itemID))
        {
            ItemData itemToRemove = itemDictionary[itemID];
            itemDictionary.Remove(itemID);
            allItems.Remove(itemToRemove);
        }
        else
        {
            Debug.LogWarning($"Item with ID {itemID} does not exist");
        }
    }
}