using System.Collections.Generic;
using UnityEngine;

public class RequiredItemsChecker : MonoBehaviour
{
    [SerializeField]
    private List<InventoryItem> requiredItems;

    [SerializeField]
    private Inventory playerInventory;
    public List<InventoryItem> RequiredItems => requiredItems;

    public bool AreAllItemsSupplied()
    {
        foreach (var item in requiredItems)
        {
            int count = playerInventory.GetItemCount(item.itemData);
            item.isSupplied = count >= item.requiredAmount;
            if (!item.isSupplied)
                return false;
        }
        return true;
    }

    public InventoryItem GetItemAt(int index) => requiredItems[index];

    public int Count => requiredItems.Count;
}

[System.Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int requiredAmount;
    public bool isSupplied = false;
}