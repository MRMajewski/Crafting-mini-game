using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [SerializeField]
    private ItemDatabase itemDatabase;

    [SerializeField]
    private List<ItemData> inventoryItems = new List<ItemData>();
    public List<ItemData> InventoryItems { get => inventoryItems; }

    [SerializeField]
    private int maxInventorySize = 9;


    public event Action OnInventoryChange;

    public event Action<ItemData> OnItemDropped;

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
    public bool AddItem(string itemName)
    {
        ItemData itemToAdd = itemDatabase.GetItemByID(itemName);

        if (itemToAdd != null && inventoryItems.Count < maxInventorySize)
        {
            inventoryItems.Add(itemToAdd);
            OnInventoryChange?.Invoke();
            return true;
        }
        return false;
    }

    public bool RemoveItem(string itemName)
    {
        ItemData itemToRemove = inventoryItems.Find(item => item.itemName == itemName);

        if (itemToRemove != null)
        {
            inventoryItems.Remove(itemToRemove);
            inventoryItems.TrimExcess();
            OnInventoryChange?.Invoke();
            return true;
        }
        return false;
    }
    //public bool RemoveItem(ItemData itemData)
    //{
    //    if (inventoryItems.Remove(itemData))
    //    {
    //        inventoryItems.TrimExcess();
    //        OnInventoryChange?.Invoke();
    //        return true;
    //    }
    //    return false;
    //}
    //public void DropItem(string itemName)
    //{
    //    ItemData itemToDrop = inventoryItems.Find(item => item.itemName == itemName);

    //    if (itemToDrop != null)
    //    {
    //        Vector3 dropPosition = playerTransform.position + playerTransform.GetChild(0).forward * dropDistance;

    //        Instantiate(itemToDrop.prefab, dropPosition, Quaternion.identity, itemsParent);
    //        RemoveItem(itemName);
    //    }
    //}

    public int GetItemCount(ItemData item)
    {
        int count = 0;
        foreach (ItemData inventoryItem in inventoryItems)
        {
            if (item.itemName.Equals(inventoryItem.itemName))
                count++;
        }
        return count;
    }

    public bool AddItem(ItemData itemData)
    {
        if (inventoryItems.Count < maxInventorySize)
        {
            inventoryItems.Add(itemData);
            OnInventoryChange?.Invoke();
            return true;
        }
        return false;
    }
    public bool RemoveItem(ItemData itemData)
    {
        if (inventoryItems.Remove(itemData))
        {
            inventoryItems.TrimExcess();
            OnInventoryChange?.Invoke();
            return true;
        }
        return false;
    }

    public void DropItem(string itemName)
    {
        ItemData itemToDrop = inventoryItems.Find(item => item.itemName == itemName);
        if (itemToDrop != null)
        {
            inventoryItems.Remove(itemToDrop);
            inventoryItems.TrimExcess();

            OnInventoryChange?.Invoke();
            OnItemDropped?.Invoke(itemToDrop);
        }
    }
}
