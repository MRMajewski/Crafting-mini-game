using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemsSpawner : MonoBehaviour, IInteractable
{
    [SerializeField]
    private UIPanelController UIPanel;
    [SerializeField]
    private GameObject objectToSpawn;

    [SerializeField]
    private List<Transform> spawnLocations;

    private Transform lastSpawnLocation;
    private int layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Interactable");
    }
    public void Interact()
    {
        SpawnItem();
    }

    private void SpawnItem()
    {
        if (objectToSpawn != null && spawnLocations != null)
        {
            PlayerMainController.Instance.PlayerMovement.IsMoving = false;
            PlayerMainController.Instance.PlayerMovement.enabled = false;

            PlayerMainController.Instance.Animator.SetBool("isMoving", false);
        
          Transform spawnLocation = GetFreeSpawnLocation();
            if (spawnLocation != null)
            {
                PlayerMainController.Instance.Animator.SetTrigger("InteractTrigger");
                StartCoroutine(SpawnItemAfterAnimation(spawnLocation));
            }
            else
            {
                UIPanel.DisplayErrorInfo("No room for more items around");
                PlayerMainController.Instance.Animator.SetTrigger("ShakeNoTrigger");
                StartCoroutine(EnablePlayerMovementAfterUnsuccesfullSpawn());
                Debug.LogWarning("Brak wolnych miejsc do spawnowania!");
            }            
        }
        else
        {
            UIPanel.DisplayErrorInfo("LOL, wild error appeared!");
            PlayerMainController.Instance.Animator.SetTrigger("ShakeNoTrigger");
            StartCoroutine(EnablePlayerMovementAfterUnsuccesfullSpawn());
            Debug.LogWarning("No object to Spawn!");
        }
    }

    private IEnumerator SpawnItemAfterAnimation(Transform spawnLocation)
    {
        yield return new WaitUntil(() => PlayerMainController.Instance.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);

        float animationLength = PlayerMainController.Instance.Animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength + 0.2f); 

        Instantiate(objectToSpawn, spawnLocation.position, Quaternion.identity);
        lastSpawnLocation = spawnLocation;

        PlayerMainController.Instance.PlayerMovement.enabled = true;

    }
    private IEnumerator EnablePlayerMovementAfterUnsuccesfullSpawn()
    {
        yield return new WaitUntil(() => PlayerMainController.Instance.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);

        float animationLength = PlayerMainController.Instance.Animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength + 0.2f); 

        PlayerMainController.Instance.PlayerMovement.enabled = true;
    }

    private Transform GetFreeSpawnLocation()
    {
        List<Transform> availableLocations = new List<Transform>();

        foreach (var location in spawnLocations)
        {
            if (IsSpawnLocationFree(location) && location != lastSpawnLocation)
            {
                availableLocations.Add(location);
            }
        }
        if (availableLocations.Count > 0)
        {
            int randomIndex = Random.Range(0, availableLocations.Count);
            return availableLocations[randomIndex];
        }
        return null;
    }

    private bool IsSpawnLocationFree(Transform spawnLocation)
    {
        Collider[] hitColliders = Physics.OverlapSphere(spawnLocation.position, 0.3f, layerMask);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.isTrigger)
            {
                return false;
            }
        }
        return true; 
    }
}
