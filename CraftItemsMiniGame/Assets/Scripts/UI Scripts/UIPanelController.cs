using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryMode
{
    Inventory,
    Crafting
}
public class UIPanelController : MonoBehaviour
{
    private InventoryMode currentMode=InventoryMode.Inventory;
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
    [SerializeField] 
    private CraftingController craftingController;
    [SerializeField]
    private CanvasGroup errorTextCanvasGroup;
    [SerializeField]
    private TextMeshProUGUI errorText;


    [Header("Button references")]
    [SerializeField]
    private Button inventoryButton;

    public float appearDuration = 0.2f;
    public float blinkDuration = 0.1f;
    public int blinkCount = 3;
    public float disappearDuration = 0.2f;

    private void Start()
    {
        SetupSlotListeners();
        CloseUIPanel();
        inventoryButton.onClick.AddListener(ToggleInventoryPanel);
    }

    private void SetupSlotListeners()
    {
        foreach (var slot in inventoryUI.InventorySlots)
        {
            slot.SetOnClickListener(() => OnSlotClicked(slot));
        }
    }
    public void SetMode(int modeIndex)
    {
       currentMode = (InventoryMode) modeIndex;
       UpdateUIForMode(currentMode);
    }

    public void UpdateUIForMode(InventoryMode mode)
    {
        inventoryUI.ClearSelectedSlot();

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

        craftingController.ClearCrafting();
    }
    public void OnSlotClicked(InventorySlot slot)
    {
        switch (currentMode)
        {
            case InventoryMode.Inventory:
                inventoryUI.SetSelectedItemInfo(slot);
                break;

            case InventoryMode.Crafting:
                craftingController.AddItemToCrafting(slot); 
                break;
        }

        MusicManager.Instance.PlaySound(SoundNames.Click);
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
        SetMode((int)InventoryMode.Inventory);
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
            PlayerMainController.Instance.PlayerMovement.UnblockMovement();
        }
        else
        {
            OpenUIPanel();
            UpdateUIForMode(currentMode);
            inventoryUI.UpdateInventoryUI();
            PlayerMainController.Instance.PlayerMovement.BlockMovement();
        }
    }

    public void DisplayErrorInfo(string message)
    {
        errorText.text = message;
        errorTextCanvasGroup.alpha = 0;
        errorTextCanvasGroup.DOKill();

        Sequence blinkSequence = DOTween.Sequence();

        blinkSequence.Append(errorTextCanvasGroup.DOFade(1, appearDuration));

        blinkSequence.Append(errorTextCanvasGroup.DOFade(0, blinkDuration)
            .SetLoops(blinkCount * 2, LoopType.Yoyo));

        blinkSequence.Append(errorTextCanvasGroup.DOFade(0, disappearDuration));
    }
}
