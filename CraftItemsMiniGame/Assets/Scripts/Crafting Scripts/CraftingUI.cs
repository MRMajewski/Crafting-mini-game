using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [SerializeField]
    private List<InventorySlot> craftingSlots = new List<InventorySlot>();
    [SerializeField]
    private InventorySlot resultSlot;
    [SerializeField]
    private Button craftButton;

    [SerializeField]
    private List<ItemData> itemsToCraft = new List<ItemData>();

    [SerializeField]
    private TextMeshProUGUI resultNameText;

    [SerializeField]
    private TextMeshProUGUI resultInfoText;

    public TextMeshProUGUI ResultInfoText { get => resultInfoText; set => resultInfoText = value; }

    [SerializeField]
    private Sprite emptySprite;
    private void Start()
    {
        UpdateCraftButtonState();
    }
    public void OnInventorySlotClicked(InventorySlot inventorySlot)
    {
        if (inventorySlot.IsFilled && itemsToCraft.Count < craftingSlots.Count)
        {
            AddItemToCrafting(inventorySlot.currentItem);
            Inventory.Instance.InventoryUI.UpdateInventoryUI();
        }
    }

    private void AddItemToCrafting(ItemData item)
    {
        InventorySlot emptyCraftingSlot = FindFirstEmptyCraftingSlot();
        itemsToCraft.Add(item);
        emptyCraftingSlot.SetItem(item);
        Inventory.Instance.InstantlyRemoveItemFromInventory(item);
        UpdateCraftButtonState();
    }

    public void OnCraftingSlotClicked(InventorySlot craftingSlot)
    {
        if (craftingSlot.IsFilled)
        {
            RemoveItemFromCrafting(craftingSlot.currentItem);

            craftingSlot.ClearSlot();
            resultInfoText.text = "";
            resultNameText.text = "";
        }
    }
    private void RemoveItemFromCrafting(ItemData item)
    {
        InventorySlot emptyInventorySlot = Inventory.Instance.InventoryUI.FindFirstEmptyInventorySlot();
        if (emptyInventorySlot != null)
        {
            emptyInventorySlot.SetItem(item);
            itemsToCraft.Remove(item);
            Inventory.Instance.InstantlyAddItemToInventory(item);
            UpdateCraftButtonState();
        }
    }
    private InventorySlot FindFirstEmptyCraftingSlot()
    {
        return craftingSlots.Find(slot => !slot.IsFilled);
    }

    private void UpdateCraftButtonState()
    {
        craftButton.interactable = (itemsToCraft.Count == craftingSlots.Count) && !resultSlot.IsFilled;
    }

    public void OnCraftButtonClicked()
    {
        if (itemsToCraft.Count == craftingSlots.Count)
        {
            ItemData resultItem = CraftingSystem.Instance.Craft(itemsToCraft);
            itemsToCraft.Clear();
            ClearCraftingPanelAfterCraft();
           // ClearCraftingPanel();


            foreach (var slot in craftingSlots)
            {
                slot.ClearSlot();
            }
            if (resultItem != null)
            {
                UpdateMainCraftSlot(resultItem);
            }
        }
        else
        {
            ClearCraftingPanelAfterCraft();
        //    ClearCraftingPanel();
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
     

        OnCraftingSlotClicked(resultSlot);

        foreach (var slot in craftingSlots)
        {
            OnCraftingSlotClicked(slot);
        }
        itemsToCraft.Clear();
        ClearCraftingSlots();
    }

    public void ClearCraftingPanelAfterCraft()
    {
        itemsToCraft.Clear();
        ClearCraftingSlots();

        OnCraftingSlotClicked(resultSlot);
    }

    private void ClearCraftingSlots()
    {
        foreach (var slot in craftingSlots)
        {
            slot.ClearSlot();
        }
    }

    public void UpdateMainCraftSlot(ItemData results)
    {
        resultSlot.SetItem(results);
        resultNameText.text = results.itemName;
        UpdateCraftButtonState();
    }
}