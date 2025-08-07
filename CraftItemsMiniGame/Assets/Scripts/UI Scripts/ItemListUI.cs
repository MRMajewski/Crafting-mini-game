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
    [SerializeField]
    private Inventory playerInventory; 

    private List<TextMeshProUGUI> itemTextList = new List<TextMeshProUGUI>(); 

    public List<InventoryItem> requiredItems;
 
    private Color baseColor;
    private Material textMaterial;

    private float baseOutlineValue;

    [SerializeField]
    private TextMeshProUGUI requiredItemTitleText;

    private Tween pulsingTween;

    [SerializeField]
    private RequiredItemsChecker requiredItemsChecker;

    [SerializeField]
    float targetOutlineWidth = 0.05f;

    public void InitItemListUI()
    {
        playerInventory.OnInventoryChange += UpdateItemUI;

        textMaterial = new Material(requiredItemTitleText.fontMaterial);
        baseColor = requiredItemTitleText.faceColor;   
        baseOutlineValue = textMaterial.GetFloat(ShaderUtilities.ID_OutlineWidth);
        CreateItemUI();
        UpdateItemUI();
    }

    private void CreateItemUI()
    {
        for (int i = 0; i < requiredItemsChecker.Count; i++)
        {
            var item = requiredItemsChecker.GetItemAt(i);
            GameObject newTextObj = Instantiate(textPrefab, listParent);
            TextMeshProUGUI newItemText = newTextObj.GetComponent<TextMeshProUGUI>();
            newItemText.text = $"{item.itemData.itemName} 0/{item.requiredAmount}";
            itemTextList.Add(newItemText);
        }

        textPrefab.gameObject.SetActive(false);
    }

    public void UpdateItemUI()
    {
        for (int i = 0; i < requiredItemsChecker.Count; i++)
        {
            var item = requiredItemsChecker.GetItemAt(i);
            int itemCount = playerInventory.GetItemCount(item.itemData);
            itemTextList[i].text = $"{item.itemData.itemName} {itemCount}/{item.requiredAmount}";

            itemTextList[i].color = itemCount >= item.requiredAmount ? Color.green : baseColor;
        }

        bool allSupplied = requiredItemsChecker.AreAllItemsSupplied();
        if (allSupplied)
            SetRequiredItemsTitleTweening();
        else
            SetRequiredItemsTitleBasic();

        EndGameManager.Instance.SetEndGamePointActive(allSupplied);
    }

    public void SetRequiredItemsTitleTweening()
    {
       
        requiredItemTitleText.color = Color.green;
        requiredItemTitleText.fontMaterial = textMaterial;
        PulseOutline();
    }

    private void PulseOutline()
    {
        pulsingTween=DOTween.To(() => textMaterial.GetFloat(ShaderUtilities.ID_OutlineWidth),
                  x => textMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, x),
                  targetOutlineWidth, 0.5f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo); 
    }
    public void SetRequiredItemsTitleBasic()
    {
        pulsingTween?.Kill();
        requiredItemTitleText.color = baseColor;
        textMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, baseOutlineValue);
    }

    private void OnDestroy()
    {
        pulsingTween?.Kill();
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
