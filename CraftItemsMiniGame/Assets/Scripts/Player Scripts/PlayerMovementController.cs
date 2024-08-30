using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Transform PlayerModelTransform { get=>playerModelTransform; }
    private bool isMoving = false;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isCurrentlyMoving = Mathf.Abs(x) > 0.01f || Mathf.Abs(z) > 0.01f;

        if (isCurrentlyMoving != isMoving)
        {
            isMoving = isCurrentlyMoving;
            PlayerMainController.Instance.Animator.SetBool("isMoving", isMoving);
        }
        if (isMoving)
        {
            HandleMovement(x,z);
        }    
    }

    private void HandleMovement(float x,float z)
    {
        Vector3 modeDirection = transform.right * x + transform.forward * z;
        controller.Move(modeDirection * movementSpeed * Time.deltaTime);

        bool isCurrentlyMoving = Mathf.Abs(x) > 0.01f || Mathf.Abs(z) > 0.01f;

        if (isCurrentlyMoving != isMoving)
        {
            isMoving = isCurrentlyMoving;
            PlayerMainController.Instance.Animator.SetBool("isMoving", isMoving);
        }

        if (isMoving)
        {
            controller.Move(modeDirection * movementSpeed * Time.deltaTime);
            RotatePlayerModel(modeDirection);
        }
        if (modeDirection != Vector3.zero)
        {
            RotatePlayerModel(modeDirection);
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
