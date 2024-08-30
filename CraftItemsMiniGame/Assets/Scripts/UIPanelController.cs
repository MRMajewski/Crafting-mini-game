using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InventoryMode
{
    Inventory,
    Crafting
}
public class UIPanelController : MonoBehaviour
{
    public InventoryMode currentMode=InventoryMode.Inventory;

    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    private GameObject inventoryItemDataGameObject;
    [SerializeField]
    private GameObject craftingPanelGameObject;
    [SerializeField]
    private InventoryUI inventoryUI;
    [SerializeField]
    private CraftingUI craftingUI;

    private void Start()
    {
        CloseUIPanel();
    }
    public void SetMode(int modeIndex)
    {
        currentMode = (InventoryMode) modeIndex;
       UpdateUIForMode(currentMode);
    }
    //public void OpenInventoryPanel()
    //{
    //    OpenUIPanel();

    //    inventoryUI.UpdateInventoryUI();
    //}
    public void UpdateUIForMode(InventoryMode mode)
    {
        switch (mode)
        {
            case InventoryMode.Inventory:
                inventoryItemDataGameObject.SetActive(true);
                craftingPanelGameObject.SetActive(false);
                break;
            case InventoryMode.Crafting:
                inventoryItemDataGameObject.SetActive(false);
                craftingPanelGameObject.SetActive(true);
                break;
        }
    }

    public void OnSlotClicked(InventorySlot slot)
    {
        switch (currentMode)
        {
            case InventoryMode.Inventory:
                inventoryUI.SetSelectedItemInfo(slot);
                break;
            case InventoryMode.Crafting:
                craftingUI.OnCraftSlotClicked(slot);
                break;
        }
    }
 
    public void OpenCraftingPanel()
    {
        OpenUIPanel();
        craftingPanelGameObject.SetActive(true);
        inventoryItemDataGameObject.SetActive(false);
    }

    public void OpenUIPanel()
    {
        mainPanel.SetActive(true);
    }

    public void CloseUIPanel()
    {
        mainPanel.SetActive(false);
    }

    public void ToggleInventoryPanel()
    {
        if (mainPanel.activeSelf)
        {
            CloseUIPanel();
        }
        else
        {
            OpenUIPanel();
            inventoryUI.UpdateInventoryUI();
        }
    }
}
