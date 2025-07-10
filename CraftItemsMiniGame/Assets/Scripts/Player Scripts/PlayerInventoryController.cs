using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField]
    private UIPanelController UIPanel;
    [SerializeField]
    private PickupItemInteractable nearbyItem;

    [SerializeField]
    private float detectionRadius = 1.5f; 

    public void PickUpItem(PickupItemInteractable item)
    {
        
        PlayerMainController.Instance.PlayerMovement.IsMoving = false;
        PlayerMainController.Instance.PlayerMovement.enabled = false;

        PlayerMainController.Instance.Animator.SetBool("isMoving", false);
        if (TrytoAddItem(item))
        {
            PlayerMainController.Instance.Animator.SetTrigger("PickUpTrigger");
            StartCoroutine(AddItemAfterAnimation(item));
        }
        else
        {
            UIPanel.DisplayErrorInfo("No free slots in inventory");
            PlayerMainController.Instance.Animator.SetTrigger("ShakeNoTrigger");
            StartCoroutine(EnablePlayerMovementAfterUnsuccesfullPickUp());
        }
    }
    private bool TrytoAddItem(PickupItemInteractable item)
    {
        bool wasAdded = Inventory.Instance.AddItem(item.ItemData.itemName);
        return wasAdded;
    }
    private IEnumerator AddItemAfterAnimation(PickupItemInteractable item)
    {
        float animationLength = PlayerMainController.Instance.Animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength + 0.5f); 
 
        Destroy(item.gameObject);
        PlayerMainController.Instance.PlayerMovement.enabled = true;

    }
    private IEnumerator EnablePlayerMovementAfterUnsuccesfullPickUp()
    {
        float animationLength = PlayerMainController.Instance.Animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength + 0.5f);

        PlayerMainController.Instance.PlayerMovement.enabled = true;
    }

}
