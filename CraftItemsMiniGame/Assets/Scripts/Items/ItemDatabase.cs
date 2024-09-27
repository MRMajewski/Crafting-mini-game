using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    private Dictionary<string, ItemData> itemDictionary = new Dictionary<string, ItemData>();

    public List<ItemData> allItems;

    private void OnEnable()
    {
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        itemDictionary.Clear();

        foreach (ItemData item in allItems)
        {
            if (!itemDictionary.ContainsKey(item.itemName))
            {
                itemDictionary.Add(item.itemName, item);
            }
            else
            {
                Debug.LogWarning($" Item {item.itemName} not added.");
            }
        }
    }

    public ItemData GetItemByID(string itemName)
    {
        if (itemDictionary.TryGetValue(itemName, out ItemData item))
        {
            return item;
        }

        Debug.LogWarning($"Item with ID {itemName} not found");
        return null;
    }
}