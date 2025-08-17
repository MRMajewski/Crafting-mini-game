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
    private InventoryMode currentMode = InventoryMode.Inventory;

    [Header("UI Panels")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject inventoryItemDataGameObject;
    [SerializeField] private GameObject craftingPanelGameObject;
    [SerializeField] private GameObject topPanelHUDGameObject;

    [Header("CanvasGroups")]
    [SerializeField] private CanvasGroup topPanelHUDCanvasGroup;
    [SerializeField] private CanvasGroup errorTextCanvasGroup;
    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private CanvasGroup exitPanel;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private CraftingUI craftingUI;
    [SerializeField] private ItemListUI itemListUI;

    [Header("Buttons")]
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private UIButtonPermamentSelection modeButtonSelector;
    [SerializeField] private Button inventoryModeButton;
    [SerializeField] private Button craftingModeButton;

    [Header("Tween References")]
    [SerializeField] private float appearDuration = 0.2f;
    [SerializeField] private float blinkDuration = 0.1f;
    [SerializeField] private int blinkCount = 3;
    [SerializeField] private float disappearDuration = 0.2f;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float inventoryBlinkDuration = .25f;
    [SerializeField] private float inventoryBlinkScale = 1.2f;
    [SerializeField] private Ease inventoryBlinkEase= Ease.InOutSine;


    private Sequence activeSequence = null;

    public CanvasGroup FadePanel => fadePanel;

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

        GameController.Instance.CraftingController.ClearCrafting();
    }
    public void OnSlotClicked(InventorySlot slot)
    {
        switch (currentMode)
        {
            case InventoryMode.Inventory:
                inventoryUI.SetSelectedItemInfo(slot);
                break;

            case InventoryMode.Crafting:
                GameController.Instance.CraftingController.AddItemToCrafting(slot);

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

    public void TogglePausePanel()
    {
        TogglePanel(pausePanel);
        if (inventoryPanel.activeSelf)
            inventoryPanel.SetActive(false);
    }

    public void ToggleInventoryPanel()
    {
        TogglePanel(inventoryPanel);
        UpdateUIForMode(currentMode);
        inventoryUI.UpdateInventoryUI();

        if (pausePanel.activeSelf)
            pausePanel.SetActive(false);
    }
    private void TogglePanel(GameObject panel)
    {
        bool isActive = panel.activeSelf;

        panel.SetActive(!isActive);

        if (isActive)
        {
            GameController.Instance.PlayerMovement.UnblockMovement();
            GameController.Instance.PauseGame(false);
        }
        else
        {
            GameController.Instance.PlayerMovement.BlockMovement();
            GameController.Instance.PauseGame(true);
        }
    }

    public void DisplayErrorInfo(string message)
    {
        errorText.text = message;
        errorTextCanvasGroup.alpha = 0;

        if (activeSequence != null && activeSequence.IsActive())
        {
            activeSequence.Kill();
        }

        activeSequence = DOTween.Sequence();

        activeSequence.Append(errorTextCanvasGroup.DOFade(1, appearDuration))
                .Append(errorTextCanvasGroup.DOFade(0, blinkDuration)
                .SetLoops(blinkCount * 2, LoopType.Yoyo))
                .Append(errorTextCanvasGroup.DOFade(0, disappearDuration));
    }

    public void BlinkInventoryButton()
    {
        inventoryButton.transform.DOScale(inventoryBlinkScale, inventoryBlinkDuration).SetLoops(2,LoopType.Yoyo).SetEase(inventoryBlinkEase);
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

    public void ExitGameByButton()
    {
        GameController.Instance.ExitGame();
    }
    public void RestartGameByButton()
    {
        GameController.Instance.RestartGame();
    }
}
