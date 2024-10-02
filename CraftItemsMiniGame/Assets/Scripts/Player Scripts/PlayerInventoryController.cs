using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField]
    private UIPanelController UIPanel;
    [SerializeField]
    private PickupItem nearbyItem;

    [SerializeField]
    private float detectionRadius = 1.5f; 

    private bool canPickUpItem = true; // Dodany bool sprawdzaj�cy, czy trwa animacja podnoszenia

    private void Update()
    {
        if (!Input.anyKey) return;
          
        if (Input.GetKeyDown(KeyCode.I))
        {
            UIPanel.ToggleInventoryPanel();
        }
    }
    //public void PickUpItem()
    //{
    //    if (nearbyItem != null)
    //    {
    //        canPickUpItem = false;
    //        RotateTowardsNearbyItem();
    //        PlayerMainController.Instance.PlayerMovement.enabled = false;
    //        PlayerMainController.Instance.Animator.SetTrigger("PickUpTrigger"); 
    //        StartCoroutine(AddItemAfterAnimation());
    //    }
    //}

    public void PickUpItem(PickupItem item)
    {
        PlayerMainController.Instance.PlayerMovement.enabled = false;

        if(TrytoAddItem(item))
        {
            PlayerMainController.Instance.Animator.SetTrigger("PickUpTrigger");
            StartCoroutine(AddItemAfterAnimation(item));
        }
        else
        {
            PlayerMainController.Instance.Animator.SetTrigger("ShakeNoTrigger");
            StartCoroutine(EnablePlayerMovementAfterUnsuccesfullPickUp());
        }
    }
    private bool TrytoAddItem(PickupItem item)
    {
        bool wasAdded = Inventory.Instance.AddItem(item.ItemData.itemName);
        return wasAdded;
    }
    private IEnumerator AddItemAfterAnimation(PickupItem item)
    {
        yield return new WaitForSecondsRealtime(PlayerMainController.Instance.Animator.GetCurrentAnimatorStateInfo(0).length+0.1f); 
        Destroy(item.gameObject);
        PlayerMainController.Instance.PlayerMovement.enabled = true;

    }
    private IEnumerator EnablePlayerMovementAfterUnsuccesfullPickUp()
    {
        yield return new WaitForSecondsRealtime(PlayerMainController.Instance.Animator.GetCurrentAnimatorStateInfo(0).length + 0.1f);
        PlayerMainController.Instance.PlayerMovement.enabled = true;
    }

    //private IEnumerator AddItemAfterAnimation()
    //{
    //    yield return new WaitForSecondsRealtime(PlayerMainController.Instance.Animator.GetCurrentAnimatorStateInfo(0).length + 0.1f);

    //    if (nearbyItem == null)
    //    {
    //        DetectNearbyItems();
    //        yield return null;
    //    }


    //    bool wasAdded = Inventory.Instance.AddItem(nearbyItem.ItemData.itemName);
    //    if (wasAdded)
    //    {
    //        Destroy(nearbyItem.gameObject);

    //        nearbyItem = null;
    //    }
    //    PlayerMainController.Instance.PlayerMovement.enabled = true;
    //    canPickUpItem = true;
    //    DetectNearbyItems();
    //}

    private void RotateTowardsNearbyItem()
    {
        Vector3 directionToItem = nearbyItem.transform.position - PlayerMainController.Instance.PlayerMovement.PlayerModelTransform.transform.position;
        directionToItem.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToItem);
        PlayerMainController.Instance.PlayerMovement.PlayerModelTransform.DORotate(targetRotation.eulerAngles, 0.3f);
    }

    private void DetectNearbyItems()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            PickupItem item = hitCollider.GetComponent<PickupItem>();
            if (item != null && item != nearbyItem)
            {
                nearbyItem = item;
                return;
            }
        }
        nearbyItem = null;
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
