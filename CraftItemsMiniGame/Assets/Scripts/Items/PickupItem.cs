using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickupItem : MonoBehaviour, IInteractable

{   [SerializeField]
    private  ItemData itemData;
    public ItemData ItemData { get => itemData; }

    public void Interact()
    {
        throw new System.NotImplementedException();
    }
}
