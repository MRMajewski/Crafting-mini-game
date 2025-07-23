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
    protected float tweenDuration = .5f;
    [SerializeField] protected Vector3 punchScale = Vector3.one * 0.05f;
    [SerializeField] protected int punchVibrato = 1;
    [SerializeField, Range(0f, 1f)] protected float punchElasticity = 0.5f;
    [SerializeField] protected Vector2 delayRange = new Vector2(3f, 6f);
    [SerializeField] protected Transform spawnerModelTransform;

    protected Sequence punchTween;
    protected Vector3 baseScale;

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

    public virtual void StartAnimationInLoop()
    {

        if (spawnerModelTransform==null) return;
        baseScale = spawnerModelTransform.localScale;

        Vector3 scaledPunch = Vector3.Scale(baseScale, punchScale);

        punchTween = DOTween.Sequence()
            .Append(spawnerModelTransform.DOPunchScale(
                scaledPunch,
                tweenDuration,
                punchVibrato,
                punchElasticity
            ))
            .AppendInterval(Random.Range(delayRange.x, delayRange.y))
            .SetLoops(-1, LoopType.Restart);


    }
}
