using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField]
    private float pickUpItemDelay = 1f;

    [SerializeField] private float interactionAnimParameterValue = 3f;

    private Sequence spawnSequence = null;

    public void PickUpItem(PickupItemInteractable item)
    {
        var player = GameController.Instance;
        player.PlayerMovement.IsMoving = false;
        player.PlayerMovement.BlockMovement();
        player.Animator.SetBool("isMoving", false);

        if (TryToAddItem(item))
        {
            player.Animator.CrossFade(AnimatorStates.PickUp, .1f);
            StartCoroutine(AddItemAfterAnimation(item));

            MusicManager.Instance.PlaySound(SoundNames.Pick);
        }
        else
        {
            GameController.Instance.UIPanelController.DisplayErrorInfo("You cannot carry more items!");
            player.Animator.CrossFade(AnimatorStates.ShakeNo, .1f);
            MusicManager.Instance.PlaySound(SoundNames.Error);
            item.SetBusyState(false);
        }
    }

    private bool TryToAddItem(PickupItemInteractable item)
    {
        return Inventory.Instance.AddItem(item.ItemData.itemName);
    }

    private IEnumerator AddItemAfterAnimation(PickupItemInteractable item)
    {
        yield return new WaitForSecondsRealtime(GameController.Instance.PlayerMovement.InteractionDelay * interactionAnimParameterValue);

        AddToInventoryAnimation(item);
    }

    private void AddToInventoryAnimation(PickupItemInteractable item)
    {
        Transform itemTransform = item.transform;

        spawnSequence = DOTween.Sequence()
            .Append(itemTransform.DOPunchPosition(Vector3.up * 0.2f, 0.3f, 1, 0.5f))
            .Append(itemTransform.DOScale(Vector3.zero, 0.4f).SetEase(Ease.OutBack))
            .OnComplete(() =>
            {
                GameController.Instance.PlayerMovement.UnblockMovement();
                item.SetBusyState(false); 
                spawnSequence.Kill();
                Destroy(item.gameObject);
            });
    }

    private void OnDisable()
    {
        spawnSequence?.Kill();
    }
}
