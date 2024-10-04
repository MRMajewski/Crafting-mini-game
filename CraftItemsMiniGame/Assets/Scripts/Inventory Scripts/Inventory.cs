using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public ItemDatabase itemDatabase; 

    [SerializeField]
    private List<ItemData> inventoryItems = new List<ItemData>();
    public List<ItemData> InventoryItems { get => inventoryItems; }

    public int maxInventorySize = 9;

    [SerializeField]
    private float dropDistance;

    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Transform itemsParent;

    [SerializeField]
    private InventoryUI inventoryUI;

    public InventoryUI InventoryUI { get => inventoryUI; }

    public event Action OnInventoryChange; 

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

        if (itemToAdd != null)
        {
            if (inventoryItems.Count < maxInventorySize)
            {

                inventoryItems.Add(itemToAdd);
                OnInventoryChange?.Invoke();
                return true;
            }
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

    public void DropItem(string itemName)
    {
        ItemData itemToDrop = inventoryItems.Find(item => item.itemName == itemName);

        if (itemToDrop != null)
        {
            Vector3 dropPosition = playerTransform.position + playerTransform.GetChild(0).forward * dropDistance;

            Instantiate(itemToDrop.prefab, dropPosition, Quaternion.identity, itemsParent);
            RemoveItem(itemName);
        }
    }

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

    public void InstantlyAddItemToInventory(ItemData itemData)
    {
       InventoryItems.Add(itemData);

       inventoryUI.UpdateInventoryUI();
        OnInventoryChange?.Invoke();
    }
    public void InstantlyRemoveItemFromInventory(ItemData itemData)
    {
        InventoryItems.Remove(itemData);
        inventoryUI.UpdateInventoryUI();
        OnInventoryChange?.Invoke();
    }
}
