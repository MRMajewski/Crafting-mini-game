using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public List<InventorySlot> slots; 
    private List<ItemData> itemsInInventory;

    public void UpdateInventoryUI()
    {
        itemsInInventory.Clear();
        itemsInInventory.TrimExcess();
        itemsInInventory = Inventory.Instance.GetInventoryItems();

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < itemsInInventory.Count)
            {
                slots[i].SetItem(itemsInInventory[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
    public void ToggleInventoryUI()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        if (gameObject.activeSelf)
        {
            UpdateInventoryUI();
        }
    }
}