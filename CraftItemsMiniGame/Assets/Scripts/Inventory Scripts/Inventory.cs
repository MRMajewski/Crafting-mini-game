using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    private List<ItemData> items = new List<ItemData>();

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

    public void AddItem(ItemData item)
    {
        items.Add(item);
        Debug.Log("Item added: " + item.itemName);
        // Tutaj mo¿esz dodaæ kod do aktualizacji UI ekwipunku
    }

    public bool RemoveItem(string itemID)
    {
        ItemData itemToRemove = items.Find(item => item.itemID == itemID);
        if (itemToRemove != null)
        {
            items.Remove(itemToRemove);
            Debug.Log("Item removed: " + itemToRemove.itemName);
            // Tutaj mo¿esz dodaæ kod do aktualizacji UI ekwipunku
            return true;
        }
        return false;
    }

    public List<ItemData> GetItems()
    {
        return items;
    }
}
