using System.Collections.Generic;
using UnityEngine;

public class CraftingController : MonoBehaviour
{
    [SerializeField] private CraftingUI craftingUI;
    [SerializeField] private Inventory inventory;
    [SerializeField] private CraftingSystem craftingSystem;

    private readonly List<ItemData> itemsToCraft = new();

    private void Start()
    {
        craftingUI.SetCraftButtonInteractable(false);
        craftingUI.OnInventorySlotClicked += AddItemToCrafting;
        craftingUI.OnCraftingSlotClicked += RemoveItemFromCrafting;
        craftingUI.OnCraftButtonPressed += OnCraftButtonClicked;

        craftingUI.GetResultSlot().SetOnClickListener(CollectCraftingResult);
    }

    private void AddItemToCrafting(ItemData item)
    {
        if (itemsToCraft.Count >= craftingUI.GetCraftingSlots().Count) return;

        itemsToCraft.Add(item);
        inventory.RemoveItem(item.itemName);
        UpdateCraftingSlotsUI();
        UpdateCraftButtonState();
    }

    public void AddItemToCrafting(InventorySlot slot)
    {
        if (!slot.IsFilled || itemsToCraft.Count >= craftingUI.GetCraftingSlots().Count)
            return;

        itemsToCraft.Add(slot.CurrentItem);
        inventory.RemoveItem(slot.CurrentItem);
        UpdateCraftingSlotsUI();
        UpdateCraftButtonState();
    }

    public void ClearCrafting()
    {
        foreach (var item in itemsToCraft)
            inventory.AddItem(item);

        itemsToCraft.Clear();
        craftingUI.ClearCraftingSlots();
        craftingUI.ClearResultSlot();
        UpdateCraftButtonState();
    }

    private void RemoveItemFromCrafting(int index)
    {
        if (index < 0 || index >= itemsToCraft.Count) return;

        ItemData item = itemsToCraft[index];
        itemsToCraft.RemoveAt(index);
        inventory.AddItem(item.itemName);

        UpdateCraftingSlotsUI();
        UpdateCraftButtonState();
    }

    private void OnCraftButtonClicked()
    {
        bool success = craftingSystem.TryCraft(itemsToCraft, out var resultItem, out var resultMessage);
        craftingUI.ShowResult(resultItem, resultMessage);

        if (success)
        {
            itemsToCraft.Clear();
        }
        else
        {
            foreach (var item in itemsToCraft)
                inventory.AddItem(item.itemName);

            itemsToCraft.Clear();
        }

        UpdateCraftingSlotsUI();
        UpdateCraftButtonState();
    }

    private void CollectCraftingResult()
    {
        var resultItem = craftingUI.GetResultSlot().CurrentItem;
        if (resultItem != null)
        {
            inventory.AddItem(resultItem);
            craftingUI.ClearResultSlot();
            UpdateCraftButtonState();
        }
    }

    private void UpdateCraftingSlotsUI()
    {
        for (int i = 0; i < craftingUI.GetCraftingSlots().Count; i++)
        {
            if (i < itemsToCraft.Count)
                craftingUI.SetCraftingSlot(i, itemsToCraft[i]);
            else
                craftingUI.ClearCraftingSlot(i);
        }
    }

    private void UpdateCraftButtonState()
    {
        bool canCraft = itemsToCraft.Count == craftingUI.GetCraftingSlots().Count;
        craftingUI.SetCraftButtonInteractable(canCraft);
    }
}
