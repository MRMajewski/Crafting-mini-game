using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    private float movementSpeed = 6f;
    [SerializeField]
    private float rotationSpeed = 180f;
    [SerializeField]
    private Transform playerModelTransform;
    public Transform PlayerModelTransform { get => playerModelTransform; }

    private bool isMoving = false;

    public bool IsMoving { get => isMoving; set => isMoving = value; }

    private const float moveRadius = 9f;
    private Vector3 centerPosition = new Vector3(0, 0, 0);

    private Vector3? targetPosition = null;
    private Vector3 lastPosition;
    private float stuckTimer = 0f;
    private Collider targetInteractionCollider = null;

    [SerializeField]
    private  float interactionDistance = 1.5f;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;


    void Update()
    {
        HandleInput();

        // Animator: isMoving
        bool isMoving = agent.velocity.magnitude > 0.1f;
        animator.SetBool("isMoving", isMoving);

        if (targetInteractionCollider != null)
        {
            float distance = Vector3.Distance(transform.position, targetInteractionCollider.transform.position);
            if (distance <= interactionDistance)
            {
                agent.ResetPath();
                RotatePlayerModel(targetInteractionCollider.transform.position);
              //  PlayerMainController.Instance.TryStartInteraction(targetInteractionCollider);
                targetInteractionCollider = null;
            }
        }
    }
    private void HandleInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            ProcessInput(Input.mousePosition);
        }
#elif UNITY_ANDROID || UNITY_IOS
    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    {
        ProcessInput(Input.GetTouch(0).position);
    }
#endif
    }

    private void MoveTowards(Vector3 direction)
    {
        Vector3 newPosition = controller.transform.position + direction * movementSpeed * Time.deltaTime;

        Vector3 directionFromCenter = newPosition - centerPosition;

        if (newPosition.magnitude <= moveRadius)
        {
            controller.Move(direction * movementSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 clampedPosition = centerPosition + directionFromCenter.normalized * moveRadius;
            controller.Move(clampedPosition - controller.transform.position);
        }
        RotatePlayerModel(direction);
    }
    private void StopMovement()
    {
        targetPosition = null;
        targetInteractionCollider = null;
        IsMoving = false;
        PlayerMainController.Instance.Animator.SetBool("isMoving", false);
    }

    private void StartInteraction(Collider interaction)
    {
        //  PlayerMainController.Instance.TryStartInteraction(interaction);
    }
    private void ProcessInput(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                targetInteractionCollider = hit.collider;

                Vector3 dir = (hit.collider.transform.position - transform.position).normalized;
                Vector3 target = hit.collider.transform.position - dir * interactionDistance;
                agent.SetDestination(target);
            }
            else if (hit.collider.CompareTag("Ground"))
            {
                targetInteractionCollider = null;
                agent.SetDestination(hit.point);
            }
        }
    }

    private void RotatePlayerModel(Vector3 moveDirection)
    {
        Vector3 direction = new Vector3(moveDirection.x, 0f, moveDirection.z);

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerModelTransform.rotation = Quaternion.RotateTowards(playerModelTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }


}
