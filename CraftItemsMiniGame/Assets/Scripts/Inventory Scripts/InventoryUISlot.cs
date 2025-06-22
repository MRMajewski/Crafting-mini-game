using System;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private Button itemButton;
    [SerializeField]
    private ItemData currentItem;
    public ItemData CurrentItem { get => currentItem; }

    public bool IsFilled => currentItem != null;

    [SerializeField]
    private Sprite emptySprite;

    public void SetOnClickListener(UnityEngine.Events.UnityAction callback)
    {
        itemButton.onClick.RemoveAllListeners();
        itemButton.onClick.AddListener(callback);
    }

    public void SetItem(ItemData item)
    {
        currentItem = item;
        iconImage.sprite = item.itemIcon;     
    }

    public void ClearSlot()
    {
        currentItem = null;
        iconImage.sprite = emptySprite;
    }
}
