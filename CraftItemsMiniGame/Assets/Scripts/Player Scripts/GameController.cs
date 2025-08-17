using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Managers")]
    [SerializeField] private PlayerMovementController playerMovement;
    [SerializeField] private PlayerInventoryController playerInventory;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private UIPanelController uIPanelController;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private CraftingController craftingController;
    [SerializeField] private QualityManager qualityManager;

    [Header("UI")]
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private Animator animator;

    public PlayerMovementController PlayerMovement => playerMovement;
    public PlayerInventoryController PlayerInventory => playerInventory;
    public Animator Animator => animator;
    public CameraController CameraController => cameraController;
    public UIPanelController UIPanelController => uIPanelController;
    public CraftingController CraftingController => craftingController;


    private bool isPaused;
    public bool IsPaused => isPaused;

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
        InitializeSystems();
        InitializeUI();
        InitializeGameplay();
    }

    private void InitializeSystems()
    {
#if UNITY_WEBGL
        qualityManager.SetQuality();
#endif
        musicManager.InitMusicManager();
        craftingController.InitCrafting();
    }

    private void InitializeUI()
    {
        uIPanelController.InitUI();
        InitMainMenu();
    }

    private void InitializeGameplay()
    {
        PauseGame(true);
    }
    private void InitMainMenu()
    {
        OpeningSequence();
        CameraController.SetMainMenuCameraPosition();
    }

    public void OpeningSequence()
    {
        mainMenu.OpeningSequence();
        PauseGame(true);
    }

    public void PauseGame(bool isPaused)
    {
        this.isPaused = isPaused;
    }

    public void StartGame()
    {
        void StartGameAfterAnimCompleted()
        {
            PauseGame(false);
            mainMenu.gameObject.SetActive(false);
            uIPanelController.OpenHUDPanelTween(true);
        }

        CameraController.SetCameraFollowingFromMainMenu(StartGameAfterAnimCompleted);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Debug.Log("ExitGame");
        Application.Quit();
    }
}
