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
    private GameObject inventoryPanel;
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject inventoryItemDataGameObject;
    [SerializeField]
    private GameObject craftingPanelGameObject;
    [SerializeField]
    private GameObject topPanelHUDGameObject;
    [SerializeField]
    private CanvasGroup topPanelHUDCanvasGroup;
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

    [SerializeField] private CanvasGroup fadePanel;
    public CanvasGroup FadePanel => fadePanel;
    [SerializeField] private CanvasGroup exitPanel;
    public CanvasGroup ExitPanel => exitPanel;

    [SerializeField] private UIButtonPermamentSelection modeButtonSelector;
    [SerializeField] private Button inventoryModeButton;
    [SerializeField] private Button craftingModeButton;


    [Header("Button references")]
    [SerializeField]
    private Button inventoryButton;
    [Header("Button references")]
    [SerializeField]
    private Button pauseButton;

    public float appearDuration = 0.2f;
    public float blinkDuration = 0.1f;
    public int blinkCount = 3;
    public float disappearDuration = 0.2f;

    [SerializeField] private float fadeDuration = 1f;

    private Sequence activeSequence = null;

    [SerializeField]
    private ItemListUI itemListUI;

    public void InitUI()
    {
        SetupSlotListeners();
        CloseUIPanel();
        inventoryButton.onClick.AddListener(ToggleInventoryPanel);
        pauseButton.onClick.AddListener(TogglePausePanel);
        OpenHUDPanel(false);
        itemListUI.InitItemListUI();
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
        currentMode = (InventoryMode)modeIndex;
        UpdateUIForMode(currentMode);

        switch (currentMode)
        {
            case InventoryMode.Inventory:
                modeButtonSelector.ChangeToSelected(inventoryModeButton);
                break;
            case InventoryMode.Crafting:
                modeButtonSelector.ChangeToSelected(craftingModeButton);
                break;
        }
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
        inventoryPanel.SetActive(true); 
        SetMode((int)InventoryMode.Inventory);    
    }

    public void CloseUIPanel()
    {
        inventoryPanel.SetActive(false);     
    }

    public void OpenHUDPanel(bool shouldActive)
    {
        topPanelHUDGameObject.SetActive(shouldActive);
    }
    public void OpenHUDPanelTween(bool shouldActive)
    {
       
        if (topPanelHUDCanvasGroup == null)
        {
            return;
        }

        if (shouldActive)
        {
            topPanelHUDGameObject.SetActive(true);
            topPanelHUDCanvasGroup.alpha = 0f;
            topPanelHUDCanvasGroup.DOFade(1f, 0.5f).SetEase(Ease.OutQuad);
        }
        else
        {
            topPanelHUDCanvasGroup.DOFade(0f, 0.5f)
                .SetEase(Ease.InQuad)
                .OnComplete(() => topPanelHUDGameObject.SetActive(false));
        }
    }

    public void ToggleInventoryPanel()
    {
        if (inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(false);
            GameController.Instance.PlayerMovement.UnblockMovement();
            GameController.Instance.PauseGame(false);
        }
        else
        {
            OpenUIPanel();
            UpdateUIForMode(currentMode);
            inventoryUI.UpdateInventoryUI();
            GameController.Instance.PlayerMovement.BlockMovement();
            GameController.Instance.PauseGame(true);
        }

        if (pausePanel.activeSelf)
        {
            pausePanel.gameObject.SetActive(false);
        }
    }

    public void TogglePausePanel()
    {
        if (pausePanel.activeSelf)
        {
            pausePanel.gameObject.SetActive(false);
            GameController.Instance.PlayerMovement.UnblockMovement();
            GameController.Instance.PauseGame(false);
        }
        else
        {
            pausePanel.gameObject.SetActive(true);
            GameController.Instance.PlayerMovement.BlockMovement();
            GameController.Instance.PauseGame(true);
        }

        if (inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(false);
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

    public void OpenExitPanel()
    {
        exitPanel.alpha = 0f;
        exitPanel.gameObject.SetActive(true);   
        exitPanel.DOFade(1f, fadeDuration / 2f);
    }

    public void ReturnFromExitPanel()
    {
        if (activeSequence != null && activeSequence.IsActive())
        {
            activeSequence.Kill();
        }

        activeSequence = DOTween.Sequence();
        activeSequence
            .Append(exitPanel.DOFade(0f, fadeDuration / 2f))
            .OnComplete(() =>
            {
                exitPanel.alpha = 0f;
                exitPanel.gameObject.SetActive(false);
            });
    }

    public void ExitGame()
    {
        Debug.Log("ExitGame");
        Application.Quit();
    }
}
