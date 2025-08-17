using DG.Tweening;
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
    [SerializeField] private List<GameObject> panels;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject infoPanel;
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
    [SerializeField] private float inventoryBlinkScale = .2f;

    private Sequence activeSequence = null;

    public CanvasGroup FadePanel => fadePanel;

    private void Start()
    {
        InitUI();
    }

    public void InitUI()
    {
        if (panels.Count == 0)
        {
            AutoFillPanels();
        }

        SetupSlotListeners();
        CloseAllPanels();

        OpenHUDPanel(false);
        itemListUI.InitItemListUI();
    }

    [ContextMenu("AutoFill Panels")]
    private void AutoFillPanels()
    {
        panels = new List<GameObject> { inventoryPanel, pausePanel, infoPanel };
    }
    private void SetupSlotListeners()
    {
        foreach (var slot in inventoryUI.InventorySlots)
        {
            slot.SetOnClickListener(() => OnSlotClicked(slot));
        }
    }

    #region Panel Helpers

    public void CloseAllPanels()
    {
        foreach (var panel in panels)
        {
            panel.SetActive(false);
        }    
    }

    public void CloseAllPanelsExcept(GameObject panelToOpen)
    {
        foreach (var panel in panels)
        {
            if (panel == panelToOpen)
                panel.SetActive(true);
            else
                panel.SetActive(false);
        }
        ResumeGame(false);
    }

    private void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        ResumeGame(true);
    }

    #endregion

    #region Specific Panels



    private void ResumeGame(bool shouldBeResumed)
    {
        if (shouldBeResumed)
            GameController.Instance.PlayerMovement.UnblockMovement();
        else
            GameController.Instance.PlayerMovement.BlockMovement();

        GameController.Instance.PauseGame(!shouldBeResumed);
    }

    public void ToggleInventoryPanel()
    {
        TogglePanel(inventoryPanel);

        if (inventoryPanel.activeSelf)
            OpenInventoryPanel();


        void OpenInventoryPanel()
        {
            SetMode((int)InventoryMode.Inventory);
            inventoryUI.UpdateInventoryUI();
        }
    }

    public void TogglePausePanel()
    {
        TogglePanel(pausePanel);
    }

    public void ToggleInfoPanel()
    {
        TogglePanel(infoPanel);
    }

    private void TogglePanel(GameObject panel)
    {
        bool isActive = panel.activeSelf;

        if (isActive)
            ClosePanel(panel);
        else 
            CloseAllPanelsExcept(panel);
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

    #endregion

    #region HUD

    public void OpenHUDPanel(bool shouldActive)
    {
        topPanelHUDGameObject.SetActive(shouldActive);
    }

    public void OpenHUDPanelTween(bool shouldActive)
    {
        if (topPanelHUDCanvasGroup == null) return;

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

    #endregion

    #region Mode & Slots

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

    #endregion

    #region Tweens & Effects

    public void DisplayErrorInfo(string message)
    {
        errorText.text = message;
        errorTextCanvasGroup.alpha = 0;

        if (activeSequence != null && activeSequence.IsActive())
            activeSequence.Kill();

        activeSequence = DOTween.Sequence();

        activeSequence.Append(errorTextCanvasGroup.DOFade(1, appearDuration))
            .Append(errorTextCanvasGroup.DOFade(0, blinkDuration)
            .SetLoops(blinkCount * 2, LoopType.Yoyo))
            .Append(errorTextCanvasGroup.DOFade(0, disappearDuration));
    }

    public void BlinkInventoryButton()
    {
        inventoryButton.transform.DOPunchScale(
            Vector3.one * inventoryBlinkScale,
            inventoryBlinkDuration
            );
    }

    #endregion

    #region Game Flow

    public void ExitGameByButton()
    {
        GameController.Instance.ExitGame();
    }

    public void RestartGameByButton()
    {
        GameController.Instance.RestartGame();
    }
    #endregion
}
