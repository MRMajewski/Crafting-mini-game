using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class ItemListUI : MonoBehaviour
{
    [SerializeField] 
    private GameObject textPrefab; 
    [SerializeField] 
    private Transform listParent;  
    private Inventory playerInventory; 

    private List<TextMeshProUGUI> itemTextList = new List<TextMeshProUGUI>(); 

    public List<InventoryItem> requiredItems;
 
    private Color baseColor;
    private Material textMaterial;

    private float baseOutlineValue;

    [SerializeField]
    private TextMeshProUGUI requiredItemTitleText;

    private void Start()
    {   
        playerInventory =Inventory.Instance;
        playerInventory.OnInventoryChange += UpdateItemUI;

        baseColor= requiredItemTitleText.faceColor;
        textMaterial = requiredItemTitleText.fontSharedMaterial;
        baseOutlineValue = textMaterial.GetFloat(ShaderUtilities.ID_OutlineWidth);
        CreateItemUI();
        UpdateItemUI();
    }

    private void CreateItemUI()
    {
        foreach (InventoryItem item in requiredItems)
        {
            GameObject newTextObj = Instantiate(textPrefab, listParent);
            TextMeshProUGUI newItemText = newTextObj.GetComponent<TextMeshProUGUI>();
            newItemText.text = $"{item.itemData.itemName} 0/{item.requiredAmount}";

            itemTextList.Add(newItemText); 
        }

        textPrefab.gameObject.SetActive(false);
    }

    public void UpdateItemUI()
    {
        for (int i = 0; i < requiredItems.Count; i++)
        {
            InventoryItem item = requiredItems[i];
            int itemCount = playerInventory.GetItemCount(item.itemData);
            itemTextList[i].text = $"{item.itemData.itemName} {itemCount}/{item.requiredAmount}";

            if (itemCount >= item.requiredAmount)
            {
                itemTextList[i].color = Color.green;
                item.isSupplied = true;
            }
            else
            {
                itemTextList[i].color = baseColor;
                item.isSupplied = false;
            }
        }
        if (CheckIfAllItemsAreSupplied())
        {
            SetRequiredItemsTitleTweening();          
        }
        else
        {
            SetRequiredItemsTitleBasic();
        }
        EndGameController.Instance.SetEndGamePointActive(CheckIfAllItemsAreSupplied());
    }

    public bool CheckIfAllItemsAreSupplied()
    {
        foreach (InventoryItem item in requiredItems)
        {
            if (!item.isSupplied)
                return false;
        }
        return true;
    }

    public void SetRequiredItemsTitleTweening()
    {
        requiredItemTitleText.color = Color.green;
        PulseOutline();
    }

    private void PulseOutline()
    {
        float targetOutlineWidth = 0.05f; 

        DOTween.To(() => textMaterial.GetFloat(ShaderUtilities.ID_OutlineWidth),
                  x => textMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, x),
                  targetOutlineWidth, 0.5f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo); 
    }
    public void SetRequiredItemsTitleBasic()
    {  
        DOTween.KillAll(); 
        requiredItemTitleText.color = baseColor;
        textMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, baseOutlineValue);
    }

    private void OnDestroy()
    {
        playerInventory.OnInventoryChange -= UpdateItemUI;
    }
}



[System.Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int requiredAmount; 
    public bool isSupplied=false;
}
