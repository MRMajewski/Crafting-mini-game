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

    public void UpdateInventoryUI()
    {
        
      //  itemsInInventory.Clear();
     //   itemsInInventory.TrimExcess();
       itemsInInventory = Inventory.Instance.GetInventoryItems();

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < itemsInInventory.Count)
            {
                slots[i].SetItem(itemsInInventory[i]);
            }
            else
            {
              //  slots[i].ClearSlot();
            }
        }
    } 

    public void SetSelectedItemInfo(InventorySlot slot)
    {
        if (slot.currentItem == null) return;
        selectedItemImage.sprite = slot.currentItem.itemIcon;
        selectedItemNameText.text = slot.currentItem.itemName;
        selectedItemNameDesc.text = slot.currentItem.itemDescription;
    }
}