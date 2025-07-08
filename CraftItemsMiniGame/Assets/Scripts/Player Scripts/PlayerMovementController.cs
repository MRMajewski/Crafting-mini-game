using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovementController : MonoBehaviour
{

    [SerializeField]
    private float rotationSpeed = 180f;
    [SerializeField]
    private Transform playerModelTransform;
    public Transform PlayerModelTransform { get => playerModelTransform; }

    private bool isMoving = false;

    public bool IsMoving { get => isMoving; set => isMoving = value; }

    private Collider targetInteractionCollider = null;

    [SerializeField]
    private  float interactionDistance = .75f;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;


    void Update()
    {
        HandleInput();

        bool isMoving = agent.velocity.magnitude > 0.1f;
        animator.SetBool("isMoving", isMoving);

        if (targetInteractionCollider != null)
        {
            float distance = Vector3.Distance(transform.position, targetInteractionCollider.transform.position);
            if (distance <= interactionDistance)
            {
                agent.ResetPath();

                if (targetInteractionCollider.TryGetComponent<IInteractable>(out var interactable))
                {
                    RotatePlayerModel(targetInteractionCollider.transform.position);
                    interactable.Interact();
                }

                targetInteractionCollider = null;
            }
        }
        //if (targetInteractionCollider != null)
        //{
        //    float distance = Vector3.Distance(transform.position, targetInteractionCollider.transform.position);
        //    if (distance <= interactionDistance)
        //    {
        //        agent.ResetPath();
        //        RotatePlayerModel(targetInteractionCollider.transform.position);
        //      //  PlayerMainController.Instance.TryStartInteraction(targetInteractionCollider);
        //        targetInteractionCollider = null;
        //    }
        //}
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

    private void StartInteraction(Collider interaction)
    {
        //  PlayerMainController.Instance.TryStartInteraction(interaction);
    }
    private void ProcessInput(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                targetInteractionCollider = hit.collider;
                Vector3 approach = interactable.GetApproachPosition();
                if (NavMesh.SamplePosition(approach, out NavMeshHit navHit, interactionDistance, NavMesh.AllAreas))
                {
                    agent.SetDestination(navHit.position);
                }
                //Vector3 dir = (hit.collider.transform.position - transform.position).normalized;
                //Vector3 target = hit.collider.transform.position - dir * interactionDistance;
                //agent.SetDestination(target);
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
        Vector3 direction = new Vector3(moveDirection.x, 0f, moveDirection.z);

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerModelTransform.rotation = Quaternion.RotateTowards(playerModelTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }


}
