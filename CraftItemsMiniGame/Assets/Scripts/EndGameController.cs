using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameController : MonoBehaviour
{
    public static EndGameController Instance { get; private set; }
    public Animator playerAnimator; 
    public CanvasGroup fadeCanvas;  
    public TextMeshProUGUI endMessageText;

    [SerializeField]
    private EndGamePoint endGamePoint;


    [SerializeField]
    private PlayerInventoryController playerInventoryController;

    [SerializeField]
    private GameObject scooterTransform;

    [SerializeField]
    private Transform playerEndGameTransform;
    [SerializeField]
    private Transform cameraEndGameTransform;
    [SerializeField]
    private Transform scooterEndGameTransform;

    [SerializeField]
    private CameraFollow cameraObject;

    [SerializeField]
    private float moveDuration = 2f; 

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
        endGamePoint.EndGamePointPointer.gameObject.SetActive(setActive);

        endGamePoint.CanInteract = setActive;

        if(setActive )
        {
            endGamePoint.StartRotating(); 
        }
        else
        {
            endGamePoint.StopRotating();
        }            
    }

    public void TriggerEndGame()
    {
        StartCoroutine(EndGameSequence());
    }

    private IEnumerator EndGameSequence()
    {

        PlayerMainController.Instance.PlayerMovement.enabled = false;
        PlayerMainController.Instance.PlayerInventory.enabled = false;

        yield return FadeIn(2.0f);  

        endMessageText.text = "After a while...";


        PlayerMainController.Instance.Animator.StopPlayback();
        PlayerMainController.Instance.Animator.transform.rotation = Quaternion.Euler(Vector3.zero);
    
        endGamePoint.gameObject.SetActive(false);

        scooterTransform.gameObject.SetActive(true);
        cameraObject.enabled = false;
        cameraObject.transform.position = cameraEndGameTransform.transform.position;
        cameraObject.transform.rotation = cameraEndGameTransform.transform.rotation;
        Camera.main.fieldOfView = 15f; 
        

        PlayerMainController.Instance.PlayerMovement.transform.position = playerEndGameTransform.transform.position;
        PlayerMainController.Instance.PlayerMovement.transform.rotation = playerEndGameTransform.transform.rotation;

        PlayerMainController.Instance.Animator.SetBool("isSurfing", true); 

        yield return FadeOut(2.0f); 
        yield return new WaitForSeconds(2.0f); 



        endMessageText.text = "The End. Thanks for playing!";

        MoveScooterToEnd();
        yield return new WaitForSeconds(2.0f);

        yield return FadeIn(1.0f); 


        EndGameSequenceComplete();
    }
    public void MoveScooterToEnd()
    {
        PlayerMainController.Instance.Animator.transform.parent = scooterTransform.transform;
        scooterTransform.transform.DOMove(scooterEndGameTransform.position, moveDuration).SetEase(Ease.InOutQuad);
    }
    private IEnumerator FadeIn(float duration)
    {
        fadeCanvas.DOFade(1, duration);  
        yield return new WaitForSeconds(duration);
    }

    private IEnumerator FadeOut(float duration)
    {
        fadeCanvas.DOFade(0, duration);  
        yield return new WaitForSeconds(duration);
    }

    private void EndGameSequenceComplete()
    {

        Application.Quit();

    }
}
