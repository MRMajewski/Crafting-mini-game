using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSpawnerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private List<Transform> spawnLocations;
    [SerializeField] private Transform spawnerModelTransform;
    [SerializeField] private Transform pickablesParentTransform;

    private List<Transform> availableSpawnLocations;

    [Header("Tweens parameters")]
    private Sequence spawnSequence;
    private Sequence punchTween;
    private Vector3 baseScale;

    [SerializeField] private float tweenDuration = 0.5f;
    [SerializeField] private Vector3 punchScale;
    [SerializeField] private int punchVibrato = 1;
    [SerializeField, Range(0f, 1f)] private float punchElasticity = 0.5f;
    [SerializeField] private Vector2 delayRange = new Vector2(1f, 3f);

    private bool isBusy = false;

    private void Start()
    {
        availableSpawnLocations = new List<Transform>(spawnLocations);
        StartAnimationInLoop();
    }

    public void Interact()
    {
        if (isBusy) return;
        isBusy = true;
        SpawnItem();
    }

    private void SpawnItem()
    {
        var player = GameController.Instance;

        player.PlayerMovement.IsMoving = false;
        player.PlayerMovement.BlockMovement();
        player.Animator.SetBool("isMoving", false);

        if (objectToSpawn == null)
        {
            DisplayError("LOL, wild error appeared!");
            isBusy = false;
            return;
        }

        if (availableSpawnLocations.Count == 0)
        {
            DisplayError("No room for more items around");
            isBusy = false;
            return;
        }

        Transform spawnLocation = GetFreeSpawnLocation();
        MusicManager.Instance.PlaySound("kick");

        player.Animator.CrossFade(AnimatorStates.Interact, .1f); 

        StartCoroutine(SpawnItemAfterAnimation(spawnLocation));

    }

    private IEnumerator SpawnItemAfterAnimation(Transform spawnLocation)
    {
        yield return new WaitForSecondsRealtime(GameController.Instance.PlayerMovement.InteractionDelay * 4);

        SpawningNewItem(spawnLocation);

        availableSpawnLocations.Remove(spawnLocation);

        isBusy = false;
    }

    private void SpawningNewItem(Transform spawnLocation)
    {
        GameObject spawnedItem = Instantiate(objectToSpawn, spawnLocation.position, Quaternion.identity, pickablesParentTransform);
        spawnedItem.GetComponent<PickupItemSpawnedInteractable>().InitSpawnedPickupItem(this, spawnLocation);

        Transform itemTransform = spawnedItem.transform;
        Vector3 itemBaseScale = itemTransform.localScale;
        itemTransform.localScale = Vector3.zero;

        spawnSequence = DOTween.Sequence()
            .Append(itemTransform.DOScale(itemBaseScale, 0.4f).SetEase(Ease.OutBack))
            .Append(itemTransform.DOPunchPosition(Vector3.up * 0.2f, 0.3f, 1, 0.5f))
            .OnComplete(() => spawnSequence.Kill());
    }

    private void DisplayError(string message)
    {
        GameController.Instance.UIPanelController.DisplayErrorInfo(message);
     
        GameController.Instance.Animator.CrossFade(AnimatorStates.ShakeNo, 0.1f);

        MusicManager.Instance.PlaySound("error");

    }

    private Transform GetFreeSpawnLocation()
    {
        int randomIndex = Random.Range(0, availableSpawnLocations.Count);
        return availableSpawnLocations[randomIndex];
    }

    public void FreeSpawnLocation(Transform location)
    {
        if (!availableSpawnLocations.Contains(location))
        {
            availableSpawnLocations.Add(location);
        }
    }

    public Vector3 GetApproachPosition()
    {
        return transform.position;
    }

    public void StartAnimationInLoop()
    {
        if (spawnerModelTransform == null) return;

        baseScale = spawnerModelTransform.localScale;
        Vector3 scaledPunch = Vector3.Scale(baseScale, punchScale);

        punchTween = DOTween.Sequence()
            .Append(spawnerModelTransform.DOPunchScale(scaledPunch, tweenDuration, punchVibrato, punchElasticity))
            .AppendInterval(Random.Range(delayRange.x, delayRange.y))
            .SetLoops(-1, LoopType.Restart);
    }

    private void OnDestroy()
    {
        punchTween?.Kill();
        spawnSequence?.Kill();
    }

}
