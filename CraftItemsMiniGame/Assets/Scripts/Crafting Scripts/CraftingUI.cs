using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [SerializeField]
    private List<InventorySlot> craftingSlots = new List<InventorySlot>(); // Sloty na przedmioty do craftingu
    [SerializeField]
    private Button craftButton;

    private List<ItemData> itemsToCraft = new List<ItemData>();

    //public void AddItemToCrafting(ItemData item)
    //{
    //    if (itemsToCraft.Count < 2) // Maksymalnie dwa przedmioty do craftingu
    //    {
    //        itemsToCraft.Add(item);
    //        UpdateCraftingSlots();
    //    }
    //}
    public void AddItemToCrafting(InventorySlot item)
    {
        if (itemsToCraft.Contains(item.currentItem))
        {
            itemsToCraft.Remove(item.currentItem);
        }
        else
        {
            if (itemsToCraft.Count < craftingSlots.Count)
            {
                itemsToCraft.Add(item.currentItem);
            }
        }

        UpdateCraftingSlots();
    }
    private void UpdateCraftingSlots()
    {
        for (int i = 0; i < craftingSlots.Count; i++)
        {
            if (i < itemsToCraft.Count)
            {
                craftingSlots[i].SetItem(itemsToCraft[i]);
                craftingSlots[i].itemButton.interactable = true;
            }
            else
            {
                craftingSlots[i].ClearSlot();
                craftingSlots[i].itemButton.interactable = false;
            }
        }

        // Aktywuj przycisk craft, gdy oba sloty s¹ zape³nione
        craftButton.interactable = (itemsToCraft.Count == craftingSlots.Count);
    }


    //public void UpdateCraftingSlots()
    //{
    //    //for (int i = 0; i < craftingSlots.Count; i++)
    //    //{
    //    //    if (i < itemsToCraft.Count)
    //    //    {
    //    //        craftingSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = itemsToCraft[i].itemName;
    //    //        craftingSlots[i].interactable = true;
    //    //    }
    //    //    else
    //    //    {
    //    //        craftingSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = "Empty";
    //    //        craftingSlots[i].interactable = false;
    //    //    }
    //    //}
    //}

    public void OnCraftButtonClicked()
    {
        CraftingSystem.Instance.Craft(itemsToCraft);
        itemsToCraft.Clear();
        UpdateCraftingSlots();
    }
}
