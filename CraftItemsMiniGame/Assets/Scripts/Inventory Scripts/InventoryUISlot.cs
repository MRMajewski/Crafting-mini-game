using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image iconImage; 
    public Button itemButton;  
    public ItemData currentItem; 

    public bool IsFilled => currentItem != null;

    public void SetItem(ItemData item)
    {
        currentItem = item;
        iconImage.sprite = item.itemIcon;
        itemButton.onClick.RemoveAllListeners();
    }

    public void ClearSlot()
    {
        currentItem = null;
        iconImage.sprite = null;
    }
}
