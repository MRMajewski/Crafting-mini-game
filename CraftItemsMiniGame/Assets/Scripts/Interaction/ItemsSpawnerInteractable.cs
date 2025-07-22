using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSpawnerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private UIPanelController uiPanel;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private List<Transform> spawnLocations;
    [SerializeField] private float afterAnimationDelay = 1f;
    [SerializeField]
    private List<Transform> availableSpawnLocations;
    private Transform lastSpawnLocation;

    [Header("Tweens parameters")]
    private Sequence spawnSequence;
    private Sequence punchTween;


    [SerializeField]
    private float tweenDuration = .5f;
    [SerializeField] private Vector3 punchScale = Vector3.one * 0.05f;
    [SerializeField] private int punchVibrato = 1;
    [SerializeField, Range(0f, 1f)] private float punchElasticity = 0.5f;
    [SerializeField] private Vector2 delayRange = new Vector2(1f, 3f);
    [SerializeField] private Transform spawnerModelTransform;

    private void Start()
    {
        availableSpawnLocations = new List<Transform>(spawnLocations);

        StartAnimationInLoop();
    }

    public void Interact()
    {
        SpawnItem();
    }

    private void SpawnItem()
    {

        PlayerMainController.Instance.PlayerMovement.IsMoving = false;
        PlayerMainController.Instance.PlayerMovement.BlockMovement();
        PlayerMainController.Instance.Animator.SetBool("isMoving", false);


        if (objectToSpawn == null)
        {
            DisplayError("LOL, wild error appeared!");
            return;
        }

        if (availableSpawnLocations.Count == 0)
        {
            DisplayError("No room for more items around");
            return;
        }

        Transform spawnLocation = GetFreeSpawnLocation();
        PlayerMainController.Instance.Animator.SetTrigger("InteractTrigger");
        StartCoroutine(SpawnItemAfterAnimation(spawnLocation));
    }

    private IEnumerator SpawnItemAfterAnimation(Transform spawnLocation)
    {
        yield return new WaitUntil(() => PlayerMainController.Instance.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);
        float animationLength = PlayerMainController.Instance.Animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength + afterAnimationDelay);

        SpawningNewItem(spawnLocation);

        lastSpawnLocation = spawnLocation;
        availableSpawnLocations.Remove(spawnLocation);

        PlayerMainController.Instance.PlayerMovement.UnblockMovement();
    }

    private void SpawningNewItem(Transform spawnLocation)
    {
        GameObject spawnedItem = Instantiate(objectToSpawn, spawnLocation.position, Quaternion.identity);

        spawnedItem.GetComponent<PickupItemSpawnedInteractable>().InitSpawnedPickupItem(this, spawnLocation);

        Transform itemTransform = spawnedItem.transform;

        Vector3 baseScale = spawnedItem.transform.localScale;
        itemTransform.localScale = Vector3.zero;

        spawnSequence
                     .Append(itemTransform.DOScale(baseScale, 0.4f).SetEase(Ease.OutBack))
                     .Append(itemTransform.DOPunchPosition(Vector3.up * 0.2f, 0.3f, 1, 0.5f))
                     .OnComplete(() =>
                     {
                         spawnSequence.Kill();
                     })
                     ;
    }

    private void DisplayError(string message)
    {
        // uiPanel.DisplayErrorInfo(message);
        PlayerMainController.Instance.Animator.SetTrigger("ShakeNoTrigger");
        StartCoroutine(ReenableMovementWithDelay());
    }

    private IEnumerator ReenableMovementWithDelay()
    {
        yield return new WaitUntil(() => PlayerMainController.Instance.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);
        float animationLength = PlayerMainController.Instance.Animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength + afterAnimationDelay);

        PlayerMainController.Instance.PlayerMovement.UnblockMovement();
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

        punchTween = DOTween.Sequence()
            .Append(spawnerModelTransform.DOPunchScale(
                punchScale,
                tweenDuration,
                punchVibrato,
                punchElasticity
            ))
            .AppendInterval(Random.Range(delayRange.x, delayRange.y))
            .SetLoops(-1, LoopType.Restart);
    }

    private void OnDestroy()
    {
        punchTween?.Kill();
    }

}
