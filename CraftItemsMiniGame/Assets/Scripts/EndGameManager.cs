using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    public static EndGameManager Instance { get; private set; }

    [Header(" References")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private CameraController cameraObject;
    [SerializeField] private PlayerInventoryController playerInventoryController;
    [SerializeField] private EndGamePointInteractable endGamePoint;

    [Header("Transforms & Positions")]
    [SerializeField] private GameObject scooterTransform;
    [SerializeField] private Transform playerEndGameTransform;
    [SerializeField] private Transform cameraEndGameTransform;
    [SerializeField] private Transform scooterEndGameTransform;

    [Header("UI Elements")]
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private CanvasGroup endGamePanel;
    [SerializeField] private TextMeshProUGUI endMessageText;

    [Header("Tween References")]
    [SerializeField] private float moveDuration = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetEndGamePointActive(bool setActive)
    {
        if (endGamePoint.EndGamePointPointer.gameObject.activeSelf == setActive) return;

        endGamePoint.EndGamePointPointer.gameObject.SetActive(setActive);

        endGamePoint.CanInteract = setActive;

        if(setActive )
        {
            endGamePoint.StartPointerRotating();
            MusicManager.Instance.PlaySound(SoundNames.Fanfair);
        }
        else
        {
            endGamePoint.StopPointerRotating();
        }            
    }

    public void TriggerEndGame()
    {
        StartCoroutine(EndGameSequence());
    }

    private IEnumerator EndGameSequence()
    {
        fadeCanvas = GameController.Instance.UIPanelController.FadePanel;

        endGamePanel.gameObject.SetActive(true);
        endGamePanel.alpha = 0;

        fadeCanvas.gameObject.SetActive(true);

        GameController.Instance.PlayerMovement.BlockMovement();

        GameController.Instance.CameraController.SetCameraFollowing(false);

        endMessageText.text = "After a while...";
        yield return Fade(1, 2.0f);
        yield return new WaitForSeconds(1.0f);
        GameController.Instance.UIPanelController.OpenHUDPanel(false);


        GameController.Instance.Animator.StopPlayback();
        GameController.Instance.Animator.transform.rotation = Quaternion.Euler(Vector3.zero);
    
        endGamePoint.gameObject.SetActive(false);
        scooterTransform.gameObject.SetActive(true);

        GameController.Instance.CameraController.SetEndGameCameraPosition();

        GameController.Instance.PlayerMovement.PlayerModelTransform.transform.position = playerEndGameTransform.transform.position;
        GameController.Instance.PlayerMovement.PlayerModelTransform.transform.rotation = playerEndGameTransform.transform.rotation;

        GameController.Instance.Animator.CrossFade(AnimatorStates.Surfing, .1f);

        yield return Fade(0,3.0f);
        MusicManager.Instance.PlaySound(SoundNames.Rideoff);
        endMessageText.text = "The End. <br> Thanks for playing!";

        MoveScooterToEnd();
        yield return new WaitForSeconds(2.0f);

        yield return Fade(1, 2.0f);
        yield return new WaitForSeconds(2.0f);
        endGamePanel.DOFade(1, 1).OnComplete(() => {
            endGamePanel.interactable = true;
            endGamePanel.blocksRaycasts = true;
        });

    }

    public void MoveScooterToEnd()
    {
        GameController.Instance.Animator.transform.parent = scooterTransform.transform;
        scooterTransform.transform.DOMove(scooterEndGameTransform.position, moveDuration).SetEase(Ease.InOutQuad);
    }

    private IEnumerator Fade(float alpha, float duration)
    {
        fadeCanvas.DOFade(alpha, duration);
        yield return new WaitForSeconds(duration);
    }

}
