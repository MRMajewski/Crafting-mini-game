using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    [SerializeField]
    private PlayerMovementController playerMovement;
    [SerializeField]
    private PlayerInventoryController playerInventory;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private MainMenu mainMenu;

    public PlayerMovementController PlayerMovement { get=> playerMovement; }
    public PlayerInventoryController PlayerInventory { get=> playerInventory; }
    public Animator Animator { get => animator; }

    [SerializeField]
    private bool isPause = true;

    public bool IsPause { get=>isPause; set => isPause = value; }

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
    private void Start()
    {
        InitMainMenu();
    }
    private void InitMainMenu()
    {
        OpeningSequence();
    }


    public void OpeningSequence()
    {
        mainMenu.OpeningSequence();
        PauseGame(true);
    }

    public void PauseGame(bool isPaused)
    {
        isPause = isPaused;
    }

    public void StartGame()
    {
        mainMenu.gameObject.SetActive(false);
    }




}
