using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemDropper : MonoBehaviour
{
    [SerializeField] 
    private Inventory inventory;
    [SerializeField]
    private float dropDistance;
    [SerializeField]
    private float dropHeightOffset = 0f; 
    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Transform itemsParent;

    private void OnEnable()
    {
        inventory.OnItemDropped += DropItem;
    }

    private void OnDisable()
    {
        inventory.OnItemDropped -= DropItem;
    }

    public void DropItem(ItemData itemData)
    {
        if (itemData == null || itemData.prefab == null) return;

        Vector3 dropPosition = playerTransform.position + playerTransform.forward * dropDistance;
        dropPosition.y += dropHeightOffset; // dodajemy offset wysokoœci

        Instantiate(itemData.prefab, dropPosition, Quaternion.identity, itemsParent);
    }
}
