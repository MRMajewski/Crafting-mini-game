using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class EndGamePoint : MonoBehaviour, IInteractable
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
            EndGameController.Instance.TriggerEndGame();
    }

    // Metoda do rozpoczêcia rotacji
    public void StartRotating()
    {
        // Zatrzymujemy poprzedni¹ animacjê, jeœli taka istnieje
        if (rotationTween != null && rotationTween.IsActive())
        {
            rotationTween.Kill();
        }

        rotationTween = EndGamePointPointer.DOBlendableLocalRotateBy(new Vector3(0,360,0), rotationSpeed, RotateMode.LocalAxisAdd).SetUpdate(true).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    // Metoda do zatrzymania rotacji
    public void StopRotating()
    {
        if (rotationTween != null && rotationTween.IsActive())
        {
            rotationTween.Kill();  // Zatrzymuje animacjê
        }
    }

}
