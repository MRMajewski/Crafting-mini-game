using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public ItemDatabase itemDatabase; 

    private List<ItemData> inventoryItems = new List<ItemData>();
    public List<ItemData> InventoryItems { get => inventoryItems; }

    public int maxInventorySize = 9;

    [SerializeField]
    private float dropDistance;

    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Transform itemsParent;

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
                Debug.Log($"{itemToAdd.itemName} zosta³ dodany do ekwipunku.");
                return true;
            }
            else
            {
                Debug.LogWarning("Ekwipunek jest pe³ny!");
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
            Debug.Log($"{itemToRemove.itemName} zosta³ usuniêty z ekwipunku.");
            return true;
        }
        else
        {
            Debug.LogWarning($"Przedmiot z ID {itemID} nie znajduje siê w ekwipunku.");
        }
        return false;
    }

    public void DropItem(string itemID)
    {
        ItemData itemToDrop = inventoryItems.Find(item => item.itemID == itemID);

        if (itemToDrop != null)
        {
            Vector3 dropPosition = playerTransform.position + playerTransform.GetChild(0).forward * dropDistance;

            Instantiate(itemToDrop.prefab, dropPosition, Quaternion.identity, itemsParent);
            RemoveItem(itemID);
            Debug.Log($"{itemToDrop.itemName} zosta³ wyrzucony na scenê.");
        }
        else
        {
            Debug.LogWarning($"Przedmiot z ID {itemID} nie znajduje siê w ekwipunku.");
        }
    }
}
