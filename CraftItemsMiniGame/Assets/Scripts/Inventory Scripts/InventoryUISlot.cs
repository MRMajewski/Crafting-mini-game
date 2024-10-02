using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image iconImage; 
    public Button itemButton;  
    public ItemData currentItem; 

    public bool IsFilled => currentItem != null;

    [SerializeField]
    private Sprite emptySprite;

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
