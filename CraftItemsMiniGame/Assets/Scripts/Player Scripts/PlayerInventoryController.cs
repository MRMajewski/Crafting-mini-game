using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField]
    private float pickUpItemDelay = 1f;


    private Sequence spawnSequence = null;

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

        AddToInventoryAnimation(item);

        PlayerMainController.Instance.PlayerMovement.UnblockMovement();
    }

    private void AddToInventoryAnimation(PickupItemInteractable item)
    {
        Transform itemTransform = item.transform;

        spawnSequence.Append(itemTransform.DOPunchPosition(Vector3.up * 0.2f, 0.3f, 1, 0.5f))
                     .Append(itemTransform.DOScale(Vector3.zero, 0.4f).SetEase(Ease.OutBack))
                     .OnComplete(() =>
                          {
                              Destroy(item.gameObject);
                              spawnSequence.Kill();
                          });
    }

    private IEnumerator EnablePlayerMovementAfterUnsuccesfullPickUp()
    {
        float animationLength = PlayerMainController.Instance.Animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength + pickUpItemDelay);

        PlayerMainController.Instance.PlayerMovement.UnblockMovement();
    }

}
