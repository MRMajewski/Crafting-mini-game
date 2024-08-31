using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [SerializeField]
    private List<InventorySlot> craftingSlots = new List<InventorySlot>(); // Sloty na przedmioty do craftingu
    [SerializeField]
    private InventorySlot resultSlot;
    [SerializeField]
    private Button craftButton;

    private List<ItemData> itemsToCraft = new List<ItemData>();

    [SerializeField]
    private TextMeshProUGUI resultNameText;

    private void Start()
    {
        craftButton.interactable = false; 
    }
    public void OnInventorySlotClicked(InventorySlot inventorySlot)
    {
        if (inventorySlot.IsFilled && itemsToCraft.Count < craftingSlots.Count)
        {
            InventorySlot emptyCraftingSlot = FindFirstEmptyCraftingSlot();

            itemsToCraft.Add(inventorySlot.currentItem);
            emptyCraftingSlot.SetItem(inventorySlot.currentItem);
            inventorySlot.ClearSlot();
            UpdateCraftButtonState();
        }
    }
    public void OnCraftSlotClicked(InventorySlot craftingSlot)
    {
        if (craftingSlot.IsFilled)
        {
            itemsToCraft.Remove(craftingSlot.currentItem);
            InventorySlot emptyInventorySlot = Inventory.Instance.InventoryUI.FindFirstEmptyInventorySlot();
            if (emptyInventorySlot != null)
            {
                emptyInventorySlot.SetItem(craftingSlot.currentItem);
            }
            craftingSlot.ClearSlot();
            UpdateCraftButtonState();
        }
    }

    public void OnCraftingSlotClicked(InventorySlot craftingSlot)
    {
        if (craftingSlot.IsFilled)
        {
            ItemData itemToReturn = craftingSlot.currentItem;
            InventorySlot emptyInventorySlot = Inventory.Instance.InventoryUI.FindFirstEmptyInventorySlot();

            if (emptyInventorySlot != null)
            {
                emptyInventorySlot.SetItem(itemToReturn);
                craftingSlot.ClearSlot();
                Inventory.Instance.InventoryUI.UpdateInventoryUI();
                UpdateCraftButtonState();
            }
        }
    }

    private InventorySlot FindFirstEmptyCraftingSlot()
    {
        return craftingSlots.Find(slot => !slot.IsFilled);
    }

    private ItemData GetNextItemForCrafting()
    {
        return Inventory.Instance.InventoryItems.Find(item => !itemsToCraft.Contains(item));
    }

    private void UpdateCraftButtonState()
    {
        craftButton.interactable = itemsToCraft.Count == craftingSlots.Count;
    }

    public void OnCraftButtonClicked()
    {
        if (itemsToCraft.Count == craftingSlots.Count)
        {
            ItemData resultItem = CraftingSystem.Instance.Craft(itemsToCraft);
            itemsToCraft.Clear();

            foreach (var slot in craftingSlots)
            {
                slot.ClearSlot();
            }
            if (resultItem != null)
            {
                UpdateMainCraftSlot(resultItem);
                Inventory.Instance.InventoryUI.UpdateInventoryUI();
            }
        }
    }

    public void UpdateCraftingSlots()
    {
        for (int i = 0; i < craftingSlots.Count; i++)
        {
            if (i < itemsToCraft.Count)
            {
                craftingSlots[i].SetItem(itemsToCraft[i]);
            }
            else
            {
                craftingSlots[i].ClearSlot();
            }
        }
        UpdateCraftButtonState();
    }

    public void ClearCraftingPanel()
    {
        foreach (var craftingSlot in craftingSlots)
        {
            craftingSlot.ClearSlot();
        }
        resultSlot.ClearSlot();
    }
    public void UpdateMainCraftSlot(ItemData results)
    {
        resultSlot.SetItem(results);
        resultNameText.text = results.itemName;
        UpdateCraftButtonState();
    }
}