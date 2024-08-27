using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;
    private PickupItem nearbyItem;

    private void Update()
    {
        if (!Input.anyKey) return;

        if (Input.GetKeyDown(KeyCode.R) && nearbyItem != null && nearbyItem.IsPlayerInRange)
        {
            PickupNearbyItem();
        }
    }

    private void PickupNearbyItem()
    {
        bool added = inventory.AddItem(nearbyItem.itemData.itemID);
        if (added)
        {
            Debug.Log($"{nearbyItem.itemData.itemName} zosta³ podniesiony.");
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
