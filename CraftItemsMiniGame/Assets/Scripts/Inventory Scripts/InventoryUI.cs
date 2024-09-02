using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private List<InventorySlot> slots = new List<InventorySlot>(); 
    public List<InventorySlot> InventorySlots { get => slots; set => slots = value; }
    [SerializeField]
    private List<ItemData> itemsInInventory=new List<ItemData>();

    [Header("Selected main Info")]
    [SerializeField]
    private Image selectedItemImage;
    [SerializeField]
    private TextMeshProUGUI selectedItemNameText;
    [SerializeField]
    private TextMeshProUGUI selectedItemNameDesc;
    [SerializeField]
    private InventorySlot currentlySelectedSlot;

    [Header("Selected default")]
    [SerializeField]
    private Sprite selectedItemImageDefault;
    [SerializeField]
    private string selectedItemNameTextDefault;
    [SerializeField]
    private string selectedItemNameDescDefault;

    public void UpdateInventoryUI()
    {
       itemsInInventory = Inventory.Instance.InventoryItems;

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < itemsInInventory.Count)
            {
                slots[i].SetItem(itemsInInventory[i]);
            }
            else if (i>=itemsInInventory.Count)
            {
                slots[i].ClearSlot();
            }
        }
    }

    public void SetSelectedItemInfo(InventorySlot slot)
    {
        currentlySelectedSlot = slot;

        if (slot.currentItem == null)
        {
            ClearSelectedSlot();
        }
        else
        {
            selectedItemImage.sprite = slot.currentItem.itemIcon;
            selectedItemNameText.text = slot.currentItem.itemName;
            selectedItemNameDesc.text = slot.currentItem.itemDescription;
        }            
    }

    public void ClearSlot(InventorySlot slot)
    {
        slot.ClearSlot();
    }

    public void ClearSelectedSlot()
    {
        selectedItemImage.sprite = selectedItemImageDefault;
        selectedItemNameText.text = selectedItemNameTextDefault;
        selectedItemNameDesc.text = selectedItemNameDescDefault;
    }

    public void DropSelectedItem()
    {
        if (currentlySelectedSlot == null || currentlySelectedSlot.currentItem==null) return;

        Inventory.Instance.DropItem(currentlySelectedSlot.currentItem.itemName);
        ClearSlot(currentlySelectedSlot);
        ClearSelectedSlot();
    }

    public InventorySlot FindFirstEmptyInventorySlot()
    {
        return slots.Find(slot => !slot.IsFilled);
    }
}