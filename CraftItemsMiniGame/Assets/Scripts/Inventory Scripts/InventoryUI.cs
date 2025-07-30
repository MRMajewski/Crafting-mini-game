using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;

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

    [SerializeField]
    private Button dropItemButton;


    private void OnEnable()
    {
      inventory.OnInventoryChange += UpdateInventoryUI;
      dropItemButton.onClick.AddListener(DropItemUI);
    }

    private void Start()
    {
        Init();
    }
    public void Init()
    {    
        UpdateInventoryUI();
        ClearSelectedSlot();
    }
    public void UpdateInventoryUI()
    {
       itemsInInventory = inventory.InventoryItems;

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

        if (slot.CurrentItem == null)
        {
            ClearSelectedSlot();
        }
        else
        {
            SetSelectedSlot(slot);
        }

        void SetSelectedSlot(InventorySlot slot)
        {
            selectedItemImage.sprite = slot.CurrentItem.itemIcon;
            selectedItemNameText.text = slot.CurrentItem.itemName;
            selectedItemNameDesc.text = slot.CurrentItem.itemDescription;
            dropItemButton.interactable = true;
        }
        }
    public void ClearSelectedSlot()
    {
        selectedItemImage.sprite = selectedItemImageDefault;
        selectedItemNameText.text = selectedItemNameTextDefault;
        selectedItemNameDesc.text = selectedItemNameDescDefault;
        dropItemButton.interactable = false;
    }

    private void OnDisable()
    {
        Inventory.Instance.OnInventoryChange -= UpdateInventoryUI;
        dropItemButton.onClick.RemoveAllListeners();
    }


    private void DropItemUI()
    {
        inventory.DropItem(selectedItemNameText.text);
        ClearSelectedSlot();
        MusicManager.Instance.PlaySound(SoundNames.Kick);
    }
}