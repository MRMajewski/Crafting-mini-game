using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickupItemInteractable : MonoBehaviour, IInteractable

{   [SerializeField]
    private  ItemData itemData;
    public ItemData ItemData { get => itemData; }

    public Vector3 GetApproachPosition()
    {
        return transform.position;
    }

    public void Interact()
    {
        PlayerMainController.Instance.PlayerInventory.PickUpItem(this);
    }
}
