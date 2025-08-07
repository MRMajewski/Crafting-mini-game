using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Player references")]
    [SerializeField]
    private Transform playerModelTransform;
    public Transform PlayerModelTransform { get => playerModelTransform; }
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float rotationSpeed = 360f;

    [Header("Interaction references")]
    private Collider targetInteractionCollider = null;
    private bool isRotatingToInteract = false;
    private Vector3 interactionTargetDirection;
    private bool hasInteracted = false;

    [SerializeField]
    private float interactionDistance = 1f;


    [Header("Stan Ruchu")]
    private bool isMoving = false;
    public bool IsMoving { get => isMoving; set => isMoving = value; }

    [SerializeField]
    private bool isMovementBlocked = false;

    [SerializeField]
    private float interactionDelay = .1f;

    public float InteractionDelay { get => interactionDelay; }


    void Update()
    {
        if (isMovementBlocked || GameController.Instance.IsPaused)
            return;

        HandleInput();

        UpdateMovementAnimation();

        if (targetInteractionCollider == null)
        {
            RotateInMovementDirection();
        }
        else
        {
            HandleInteraction();
        }
    }

    private void HandleInput()
    {
    #if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButtonDown(0))
            {
                ProcessInput(Input.mousePosition);
            }
    #elif UNITY_WEBGL
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ProcessInput(Input.GetTouch(0).position);
        }
        else if (Input.GetMouseButtonDown(0))
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

    private void UpdateMovementAnimation()
    {
        isMoving = agent.velocity.magnitude > 0.1f;
        animator.SetBool("isMoving", isMoving);
    }

    private void RotateInMovementDirection()
    {
        if (agent.desiredVelocity.sqrMagnitude > 0.01f)
        {
            RotatePlayerModel(transform.position + agent.desiredVelocity);
        }
    }

    private void HandleInteraction()
    {
        if (agent.pathPending || agent.remainingDistance > interactionDistance)
            return;

        agent.ResetPath();

        if (!isRotatingToInteract)
        {
            interactionTargetDirection = targetInteractionCollider.transform.position - playerModelTransform.position;
            interactionTargetDirection.y = 0;
            interactionTargetDirection.Normalize();

            isRotatingToInteract = true;
            return;
        }

        RotatePlayerModel(playerModelTransform.position + interactionTargetDirection);

        float angle = Vector3.Angle(playerModelTransform.forward, interactionTargetDirection);
        if (angle < 5f && !hasInteracted)
        {
            if (targetInteractionCollider.TryGetComponent<IInteractable>(out var interactable))
            {
                Debug.Log("Interakcja!");
                interactable.Interact();
            }
            hasInteracted = true;
            StartCoroutine(ClearInteractionAfterFrame());
        }
    }

    private IEnumerator ClearInteractionAfterFrame()
    {
        yield return null;
        targetInteractionCollider = null;
        interactionTargetDirection = Vector3.zero;
        isRotatingToInteract = false;
        hasInteracted = false;
    }

    private void ProcessInput(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                Vector3 approach = interactable.GetApproachPosition();
                float distance = Vector3.Distance(transform.position, approach);

                targetInteractionCollider = hit.collider;

                if (distance <= interactionDistance)
                {
                    agent.ResetPath();
                }
                else
                {
                    if (NavMesh.SamplePosition(approach, out NavMeshHit navHit, interactionDistance, NavMesh.AllAreas))
                    {
                        agent.SetDestination(navHit.position);
                    }
                }
            }
            else if (hit.collider.CompareTag("Ground"))
            {
                targetInteractionCollider = null;

                if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, interactionDistance, NavMesh.AllAreas))
                {
                    agent.SetDestination(navHit.position);
                }
            }
        }
    }

    private void RotatePlayerModel(Vector3 moveDirection)
    {
        Vector3 direction = moveDirection - playerModelTransform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerModelTransform.rotation = Quaternion.RotateTowards(
                playerModelTransform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
    public void BlockMovement()
    {

        agent.ResetPath();
        animator.SetBool("isMoving", false);
        animator.CrossFade(AnimatorStates.Idle, 0f);
        isMovementBlocked = true;
    }

    public void UnblockMovement()
    {
        isMovementBlocked = false;
        Debug.Log("TEST");
    }

    public void UnblockMovementWithDelay()
    {
        Debug.Log("TEST delay");
        StartCoroutine(UnblockMovementAfterDelay());

        IEnumerator UnblockMovementAfterDelay()
        {
            yield return new WaitForSeconds(interactionDelay);

            UnblockMovement();
        }
    }
}
