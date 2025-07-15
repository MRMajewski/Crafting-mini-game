using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSpawnerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private UIPanelController uiPanel;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private List<Transform> spawnLocations;
    [SerializeField] private float afterAnimationDelay = 1f;

    private List<Transform> availableSpawnLocations;
    private Transform lastSpawnLocation;

    private void Start()
    {
        availableSpawnLocations = new List<Transform>(spawnLocations);
    }

    public void Interact()
    {
        SpawnItem();
    }

    private void SpawnItem()
    {
        //if (objectToSpawn == null)
        //{
        //    DisplayError("LOL, wild error appeared!");
        //    return;
        //}

        //if (availableSpawnLocations.Count == 0)
        //{
        //    DisplayError("No room for more items around");
        //    return;
        //}

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

        Instantiate(objectToSpawn, spawnLocation.position, Quaternion.identity);
        lastSpawnLocation = spawnLocation;
        availableSpawnLocations.Remove(spawnLocation);

        PlayerMainController.Instance.PlayerMovement.UnblockMovement();
    }

    private void DisplayError(string message)
    {
        // uiPanel.DisplayErrorInfo(message);
      //  PlayerMainController.Instance.PlayerMovement.BlockMovement();
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
}
