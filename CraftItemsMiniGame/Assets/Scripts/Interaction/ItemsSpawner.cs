using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSpawner : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject objectToSpawn;

    [SerializeField]
    private List<Transform> spawnLocations;

    private Transform lastSpawnLocation; 


    public void Interact()
    {
        SpawnItem();
    }


    private void SpawnItem()
    {
        if (objectToSpawn != null && spawnLocations != null)
        {

             PlayerMainController.Instance.PlayerMovement.enabled = false;   
            
            Transform spawnLocation = GetFreeSpawnLocation();
            if (spawnLocation != null)
            {
                PlayerMainController.Instance.Animator.SetTrigger("InteractTrigger");
                StartCoroutine(SpawnItemAfterAnimation(spawnLocation));
            }
            else
            {
                PlayerMainController.Instance.Animator.SetTrigger("ShakeNoTrigger");
                StartCoroutine(EnablePlayerMovementAfterUnsuccesfullSpawn());
                Debug.LogWarning("Brak wolnych miejsc do spawnowania!");
            }
               
        }
        else
        {
            PlayerMainController.Instance.Animator.SetTrigger("ShakeNoTrigger");
            StartCoroutine(EnablePlayerMovementAfterUnsuccesfullSpawn());
            Debug.LogWarning("No object to Spawn!");
        }
    }

    private IEnumerator SpawnItemAfterAnimation(Transform spawnLocation)
    {
        yield return new WaitForSecondsRealtime(PlayerMainController.Instance.Animator.GetCurrentAnimatorStateInfo(0).length + 0.1f);
        Instantiate(objectToSpawn, spawnLocation.position, Quaternion.identity);
        lastSpawnLocation = spawnLocation;
        Debug.Log("Zespawnowano nowy obiekt: " + objectToSpawn.name);
        PlayerMainController.Instance.PlayerMovement.enabled = true;

    }
    private IEnumerator EnablePlayerMovementAfterUnsuccesfullSpawn()
    {
        yield return new WaitForSecondsRealtime(PlayerMainController.Instance.Animator.GetCurrentAnimatorStateInfo(0).length + 0.1f);
        PlayerMainController.Instance.PlayerMovement.enabled = true;
    }


    private Transform GetFreeSpawnLocation()
    {
        List<Transform> availableLocations = new List<Transform>();

        foreach (var location in spawnLocations)
        {
            // Sprawdzenie, czy miejsce jest wolne (brak kolizji z innymi obiektami)
            if (IsSpawnLocationFree(location) && location != lastSpawnLocation)
            {
                availableLocations.Add(location);
            }
        }

        // Wybieramy losowe wolne miejsce
        if (availableLocations.Count > 0)
        {
            int randomIndex = Random.Range(0, availableLocations.Count);
            return availableLocations[randomIndex];
        }

        return null; // Brak wolnych miejsc
    }

    private bool IsSpawnLocationFree(Transform spawnLocation)
    {
        Collider[] hitColliders = Physics.OverlapSphere(spawnLocation.position, 0.3f); // Promieñ detekcji kolizji
        foreach (var hitCollider in hitColliders)
        {
            // Sprawdzamy, czy collider nie jest triggerem (nie chcemy kolizji z triggerami)
            if (!hitCollider.isTrigger)
            {
                return false; // Miejsce jest zajête
            }
        }
        return true; // Miejsce jest wolne
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (var location in spawnLocations)
        {
            if (location != null)
            {
                Gizmos.DrawWireSphere(location.position, 0.3f); // Rysowanie sfer w miejscach spawnowania
            }
        }
    }
}
