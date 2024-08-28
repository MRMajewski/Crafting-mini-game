using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private InventoryUI inventoryUI;
    [SerializeField]
    private UIPanelController UIPanel;
    private PickupItem nearbyItem;

    private void Update()
    {
        if (!Input.anyKey) return;

        if (Input.GetKeyDown(KeyCode.E) && nearbyItem != null)
        {
            PickupNearbyItem();
        }           
        if (Input.GetKeyDown(KeyCode.I))
        {
            UIPanel.ToggleInventoryPanel();
           // inventoryUI.ToggleInventoryUI();
        }
    }

    private void PickupNearbyItem()
    {
        bool added = inventory.AddItem(nearbyItem.ItemData.itemID);
        if (added)
        {
            Debug.Log($"{nearbyItem.ItemData.itemName} zosta³ podniesiony.");
            Destroy(nearbyItem.gameObject);
        }
        else
        {
            Debug.Log("Ekwipunek jest pe³ny!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PickupItem item = other.GetComponent<PickupItem>();
        if (item != null)
        {
            nearbyItem = item;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickupItem item = other.GetComponent<PickupItem>();
        if (item != null && item == nearbyItem)
        {
            nearbyItem = null;
        }
    }
}
