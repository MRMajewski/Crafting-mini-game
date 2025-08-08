using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class EndGamePointInteractable : MonoBehaviour, IInteractable
{
   private bool canInteract = false;
    public bool CanInteract { set=> canInteract = value;  }

    [SerializeField]
    private Transform endGamePointPointer;
    public Transform EndGamePointPointer { get => endGamePointPointer; set => endGamePointPointer = value; }

    [Header("Tween references")]
    private Tween rotationTween;
    [SerializeField]
    private float rotationSpeed = 360f;
    [SerializeField]
    private float accelerationTime =5f;

    public void Interact()
    {
        if(canInteract)
            EndGameManager.Instance.TriggerEndGame();
    }

    public void StartPointerRotating()
    {
        KillRotationTween();
        rotationTween = EndGamePointPointer.DOBlendableLocalRotateBy(new Vector3(0,360,0), rotationSpeed, RotateMode.LocalAxisAdd).SetUpdate(true).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    public void StopPointerRotating()
    {
        KillRotationTween();
    }
    private void KillRotationTween()
    {
        if (rotationTween != null && rotationTween.IsActive())
        {
            rotationTween.Kill();
            rotationTween = null;
        }
    }
    public Vector3 GetApproachPosition()
    {
        return transform.position;
    }

    public void StartAnimationInLoop()
    {
       
    }
}
