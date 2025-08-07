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

    public void StartRotating()
    {
        if (rotationTween != null && rotationTween.IsActive())
        {
            rotationTween.Kill();
        }

        rotationTween = EndGamePointPointer.DOBlendableLocalRotateBy(new Vector3(0,360,0), rotationSpeed, RotateMode.LocalAxisAdd).SetUpdate(true).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    public void StopRotating()
    {
        if (rotationTween != null && rotationTween.IsActive())
        {
            rotationTween.Kill();
        }
    }

    public Vector3 GetApproachPosition()
    {
        return transform.position;
    }

    public void StartAnimationInLoop()
    {
        throw new System.NotImplementedException();
    }
}
