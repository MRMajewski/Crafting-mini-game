using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainController : MonoBehaviour
{
    public static PlayerMainController Instance { get; private set; }
    [SerializeField]
    private PlayerMovementController playerMovement;
    [SerializeField]
    private PlayerInventoryController playerInventory;

    [SerializeField]
    private Animator animator;

    public PlayerMovementController PlayerMovement { get=> playerMovement; }
    public PlayerInventoryController PlayerInventory { get=> playerInventory; }
    public Animator Animator { get => animator; }

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
    private void Update()
    {
        DrawRay();
        if (!Input.anyKey) return;


        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            float rayDistance = 1f; 
            LayerMask layerMask = LayerMask.GetMask("Default");

            Vector3 rayOrigin = PlayerMovement.PlayerModelTransform.transform.position;
            Vector3 rayDirection =PlayerMovement.PlayerModelTransform.transform.forward;


            if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance, layerMask))
            {
                IInteractable interactedItem = hit.collider.GetComponent<IInteractable>();
   
                if (interactedItem != null)
                {
                    Debug.Log("Obiekt raycastowny: " + hit.collider.gameObject.name);
                    interactedItem.Interact();
                }

            }
        }
    }

    private void DrawRay()
    {
        Vector3 rayOrigin = PlayerMovement.PlayerModelTransform.gameObject.transform.position;  
        Vector3 rayDirection = PlayerMovement.PlayerModelTransform.gameObject.transform.forward; 

        Debug.DrawLine(rayOrigin, rayOrigin+ rayDirection, Color.green);
    }


}
