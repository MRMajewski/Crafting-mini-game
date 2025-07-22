using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickupItemInteractable : MonoBehaviour, IInteractable

{   [SerializeField]
    private  ItemData itemData;
    public ItemData ItemData { get => itemData; }

    [Header("Tweens parameters")]
    [SerializeField]
    private float tweenDuration = .5f;
    [SerializeField] private Vector3 punchScale = Vector3.one * 0.05f;
    [SerializeField] private int punchVibrato = 1;
    [SerializeField, Range(0f, 1f)] private float punchElasticity = 0.5f;
    [SerializeField] private Vector2 delayRange = new Vector2(3f, 6f);
    [SerializeField] private Transform spawnerModelTransform;

    private Sequence punchTween;

    private void Start()
    {
        StartAnimationInLoop();
    }

    private void OnDestroy()
    {
        punchTween?.Kill();
    }
    public Vector3 GetApproachPosition()
    {
        return spawnerModelTransform.position;
    }

    public void Interact()
    {
        PlayerMainController.Instance.PlayerInventory.PickUpItem(this);
    }

    public void StartAnimationInLoop()
    {
        if (spawnerModelTransform==null) return;

        punchTween = DOTween.Sequence()
            .Append(spawnerModelTransform.DOPunchScale(
                punchScale,
                tweenDuration,
                punchVibrato,
                punchElasticity
            ))
            .AppendInterval(Random.Range(delayRange.x, delayRange.y))
            .SetLoops(-1, LoopType.Restart);

    }
}
