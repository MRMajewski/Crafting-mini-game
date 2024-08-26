using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData;

    private bool isPlayerInRange = false;
    public bool IsPlayerInRange { get => isPlayerInRange; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Gracz w zasiêgu przedmiotu.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Gracz opuœci³ zasiêg przedmiotu.");
        }
    }

}
