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

    [SerializeField]
    private float rayDistance = 2f; 
    
    public float checkRadius = 0.5f; // Promieñ do OverlapSphere

    private LayerMask layerMask;
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

        layerMask = LayerMask.GetMask("Interactable");
    }
    //private void Update()
    //{
    //    DrawRay();
    //    if (!Input.anyKey) return;


    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        RaycastHit hit;

    //        Vector3 rayOrigin = PlayerMovement.PlayerModelTransform.transform.position;
    //        Vector3 rayDirection =PlayerMovement.PlayerModelTransform.transform.forward;


    //        if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance, layerMask))
    //        {
    //            IInteractable interactedItem = hit.collider.GetComponent<IInteractable>();
   
    //            if (interactedItem != null)
    //            {
    //                Debug.Log("Obiekt raycastowny: " + hit.collider.gameObject.name);
    //                interactedItem.Interact();
    //            }
    //        }
    //    }
    //}


    private void Update()
    {
        DrawRay();
        if (!Input.anyKey) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (RaycastInteraction())
            {
                return;
            }
            CheckInteractionAroundPlayer();
        }
    }

    private bool RaycastInteraction()
    {
        RaycastHit hit;
        Vector3 rayOrigin = PlayerMovement.PlayerModelTransform.position;
        Vector3 rayDirection = PlayerMovement.PlayerModelTransform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance, layerMask))
        {
            IInteractable interactedItem = hit.collider.GetComponent<IInteractable>();
            if (interactedItem != null)
            {
                interactedItem.Interact();
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
                return true; // Znaleziono interakcjê
            }
        }
        return false;
    }

    private void CheckInteractionAroundPlayer()
    {
        Vector3 playerPosition = PlayerMovement.PlayerModelTransform.position;

        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, checkRadius, layerMask);

        foreach (Collider collider in hitColliders)
        {
            IInteractable interactedItem = collider.GetComponent<IInteractable>();
            if (interactedItem != null)
            {
                interactedItem.Interact();
                return;
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
