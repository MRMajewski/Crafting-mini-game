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
    [SerializeField] private Transform spawnerModelTransform;

    private List<Transform> availableSpawnLocations;
    private Transform lastSpawnLocation;

    [Header("Tweens parameters")]
    private Sequence spawnSequence;
    private Sequence punchTween;
    private Vector3 baseScale;

    [SerializeField] private float tweenDuration = 0.5f;
    [SerializeField] private Vector3 punchScale;
    [SerializeField] private int punchVibrato = 1;
    [SerializeField, Range(0f, 1f)] private float punchElasticity = 0.5f;
    [SerializeField] private Vector2 delayRange = new Vector2(1f, 3f);

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
        var player = PlayerMainController.Instance;

        player.PlayerMovement.IsMoving = false;
        player.PlayerMovement.BlockMovement();
        player.Animator.SetBool("isMoving", false);

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
        player.Animator.SetTrigger("InteractTrigger");
        StartCoroutine(SpawnItemAfterAnimation(spawnLocation));
    }

    private IEnumerator SpawnItemAfterAnimation(Transform spawnLocation)
    {
        var animator = PlayerMainController.Instance.Animator;

        // Czekaj a¿ animacja siê odpali (czyli nie jesteœmy w Idle)
        yield return new WaitUntil(() =>
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            return state.normalizedTime > 0f || animator.IsInTransition(0);
        });

        // Czekaj a¿ przestanie byæ w transition i animacja siê zakoñczy
        yield return new WaitUntil(() =>
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            return !animator.IsInTransition(0) && state.normalizedTime >= 1f;
        });

        yield return new WaitForSecondsRealtime(afterAnimationDelay);

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
        Vector3 itemBaseScale = itemTransform.localScale;
        itemTransform.localScale = Vector3.zero;

        spawnSequence = DOTween.Sequence()
            .Append(itemTransform.DOScale(itemBaseScale, 0.4f).SetEase(Ease.OutBack))
            .Append(itemTransform.DOPunchPosition(Vector3.up * 0.2f, 0.3f, 1, 0.5f))
            .OnComplete(() =>
            {
                spawnSequence.Kill();
            });
    }

    private void DisplayError(string message)
    {
        // uiPanel.DisplayErrorInfo(message);
        PlayerMainController.Instance.Animator.SetTrigger("ShakeNoTrigger");
        StartCoroutine(ReenableMovementWithDelay());
    }
    //private IEnumerator ReenableMovementWithDelay()
    //{
    //    var animator = PlayerMainController.Instance.Animator;
    //    var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

    //    // Pomiñ, jeœli to Idle (zmieñ nazwê na tak¹, jak¹ masz w Animatorze)
    //    if (stateInfo.IsName("Idle"))
    //    {
    //        PlayerMainController.Instance.PlayerMovement.UnblockMovement();
    //        yield break;
    //    }

    //    // Czekaj a¿ animacja siê skoñczy i nie bêdzie ju¿ w trakcie przejœcia
    //    yield return new WaitUntil(() =>
    //    {
    //        var state = animator.GetCurrentAnimatorStateInfo(0);
    //        return state.normalizedTime >= 1f && !animator.IsInTransition(0);
    //    });

    //    yield return new WaitForSeconds(afterAnimationDelay);
    //    PlayerMainController.Instance.PlayerMovement.UnblockMovement();
    //}
    //private IEnumerator ReenableMovementWithDelay()
    //{
    //    var animator = PlayerMainController.Instance.Animator;
    //    var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

    //    // Pomiñ, jeœli to Idle (zmieñ nazwê na tak¹, jak¹ masz w Animatorze)
    //    if (stateInfo.IsName("Idle"))
    //    {
    //        PlayerMainController.Instance.PlayerMovement.UnblockMovement();
    //        yield break;
    //    }
    //    // Poczekaj a¿ przejœcie siê zacznie
    //    yield return new WaitUntil(() => animator.IsInTransition(0));

    //    // Poczekaj a¿ siê zakoñczy
    //    yield return new WaitUntil(() => !animator.IsInTransition(0));
    //    if (stateInfo.IsName("Idle"))
    //    {
    //        yield return new WaitForSeconds(afterAnimationDelay * 2f);
    //        PlayerMainController.Instance.PlayerMovement.UnblockMovement();
    //    }
    //    // Poczekaj a¿ klip zakoñczy siê ca³kowicie
    //    yield return new WaitUntil(() =>
    //    {
    //        var state = animator.GetCurrentAnimatorStateInfo(0);
    //        return state.normalizedTime >= 1.5f;
    //    });

    //    yield return new WaitForSeconds(afterAnimationDelay*2f);
    //    PlayerMainController.Instance.PlayerMovement.UnblockMovement();
    //}

    private IEnumerator ReenableMovementWithDelay()
    {
        var animator = PlayerMainController.Instance.Animator;
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //yield return new WaitForSeconds(afterAnimationDelay);
        //yield return new WaitUntil(() => !animator.IsInTransition(0));
 
        //// Poczekaj a¿ klip zakoñczy siê ca³kowicie
        //yield return new WaitUntil(() =>
        //{
        //    var state = animator.GetCurrentAnimatorStateInfo(0);
        //    return state.normalizedTime >= 1.5f;
        //});

        yield return new WaitForSecondsRealtime(afterAnimationDelay*4f );
        PlayerMainController.Instance.PlayerMovement.UnblockMovement();
    }

    //private IEnumerator ReenableMovementWithDelay()
    //{
    //    Animator animator = PlayerMainController.Instance.Animator;
    //    int idleHash = Animator.StringToHash("Idle");

    //    // Sprawdzaj do skutku
    //    yield return new WaitUntil(() =>
    //    {
    //        var state = animator.GetCurrentAnimatorStateInfo(0);

    //        // Jeœli stan to Idle, natychmiast odblokuj
    //        if (state.shortNameHash == idleHash)
    //        {
    //            return true;
    //        }

    //        // Jeœli animacja dobieg³a koñca i nie jesteœmy w trakcie przejœcia
    //        return state.normalizedTime >= 1f && !animator.IsInTransition(0);
    //    });

    //    yield return new WaitForSeconds(afterAnimationDelay*2f);
    //    PlayerMainController.Instance.PlayerMovement.UnblockMovement();
    //}

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
