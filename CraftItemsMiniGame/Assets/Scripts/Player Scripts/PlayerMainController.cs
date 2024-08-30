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
    // Update is called once per frame
    void Update()
    {
        
    }
}
