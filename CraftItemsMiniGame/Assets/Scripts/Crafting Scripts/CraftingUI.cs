using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private List<InventorySlot> craftingSlots;
    [SerializeField] private InventorySlot resultSlot;
    [SerializeField] private Button craftButton;
    [SerializeField] private TextMeshProUGUI resultNameText;
    [SerializeField] private TextMeshProUGUI resultInfoText;

    public event Action<ItemData> OnInventorySlotClicked;
    public event Action<int> OnCraftingSlotClicked;
    public event Action OnCraftButtonPressed;


    public List<InventorySlot> CraftingSlots { get => craftingSlots; }
    public InventorySlot ResultSlot { get => resultSlot; }
    private void Awake()
    {
        craftButton.onClick.AddListener(() => OnCraftButtonPressed?.Invoke());

        for (int i = 0; i < craftingSlots.Count; i++)
        {
            int index = i;
            craftingSlots[i].SetOnClickListener(() =>
            {
                if (craftingSlots[index].IsFilled)
                {
                    OnCraftingSlotClicked?.Invoke(index);
                    MusicManager.Instance.PlaySound(SoundNames.Click);
                }             
            });
        }
    }

    public void ShowResult(ItemData result, string message)
    {
        resultInfoText.text = message;

        if (result != null)
        {
            resultSlot.SetItem(result);
            resultNameText.text = result.itemName;
        }
        else
        {
            resultSlot.ClearSlot();
            resultNameText.text = "";
        }
    }

    public void SetCraftingSlot(int index, ItemData item)
    {
        if (index >= 0 && index < craftingSlots.Count)
            craftingSlots[index].SetItem(item);
    }

    public void ClearCraftingSlot(int index)
    {
        if (index >= 0 && index < craftingSlots.Count)
            craftingSlots[index].ClearSlot();
    }

    public void ClearCraftingSlots()
    {
        foreach (var slot in craftingSlots)
            slot.ClearSlot();
    }

    public void ClearResultSlot()
    {
        resultSlot.ClearSlot();
        resultNameText.text = "";
        resultInfoText.text = "";
    }

    public void SetCraftButtonInteractable(bool value)
    {
        craftButton.interactable = value;
    }
}
