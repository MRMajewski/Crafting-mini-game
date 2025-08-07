using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    [SerializeField]
    private PlayerMovementController playerMovement;
    [SerializeField]
    private PlayerInventoryController playerInventory;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private UIPanelController uIPanelController;
    [SerializeField]
    private MusicManager musicManager;
    [SerializeField]
    private CraftingController craftingController;
    [SerializeField]
    private QualityManager qualityManager;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private MainMenu mainMenu;

    public PlayerMovementController PlayerMovement { get=> playerMovement; }
    public PlayerInventoryController PlayerInventory { get=> playerInventory; }
    public Animator Animator { get => animator; }
    public CameraController CameraController { get => cameraController; }
    public UIPanelController UIPanelController { get => uIPanelController; }

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
        uIPanelController.InitUI();
        musicManager.InitMusicManager();
        craftingController.InitCrafting();

        qualityManager.SetQuality();
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
        isPause = isPaused;
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
