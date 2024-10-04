using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEditor.Progress;

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField]
    private UIPanelController UIPanel;
    [SerializeField]
    private PickupItem nearbyItem;

    [SerializeField]
    private float detectionRadius = 1.5f; 


    private void Update()
    {
        if (!Input.anyKey) return;
          
        if (Input.GetKeyDown(KeyCode.I))
        {
            UIPanel.ToggleInventoryPanel();
        }
    }

    public void PickUpItem(PickupItem item)
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
