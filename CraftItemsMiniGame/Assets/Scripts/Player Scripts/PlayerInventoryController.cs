using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField]
    private float pickUpItemDelay =1f;

    public void PickUpItem(PickupItemInteractable item)
    {   
        PlayerMainController.Instance.PlayerMovement.IsMoving = false;
        PlayerMainController.Instance.PlayerMovement.BlockMovement();

        PlayerMainController.Instance.Animator.SetBool("isMoving", false);
        if (TrytoAddItem(item))
        {
            PlayerMainController.Instance.Animator.SetTrigger("PickUpTrigger");
            StartCoroutine(AddItemAfterAnimation(item));
        }
        else
        {
           // UIPanel.DisplayErrorInfo("No free slots in inventory");
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
        yield return new WaitForSeconds(animationLength + pickUpItemDelay); 
 
        Destroy(item.gameObject);
        PlayerMainController.Instance.PlayerMovement.UnblockMovement();
    }
    private IEnumerator EnablePlayerMovementAfterUnsuccesfullPickUp()
    {
        float animationLength = PlayerMainController.Instance.Animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength + pickUpItemDelay);

        PlayerMainController.Instance.PlayerMovement.UnblockMovement();
    }

}
