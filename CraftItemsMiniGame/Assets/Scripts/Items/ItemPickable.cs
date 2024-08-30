using UnityEngine;

public class PickupItem : MonoBehaviour
{   [SerializeField]
    private  ItemData itemData;
    public ItemData ItemData { get => itemData; }
}
