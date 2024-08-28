using UnityEngine;

public class PickupItem : MonoBehaviour
{   [SerializeField]
    private  ItemData itemData;
    public ItemData ItemData { get => itemData; }
    //[SerializeField]
    //private bool isPlayerInRange = false;
    //public bool IsPlayerInRange { get => isPlayerInRange; }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        isPlayerInRange = true;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        isPlayerInRange = false;
    //    }
    //}

}
