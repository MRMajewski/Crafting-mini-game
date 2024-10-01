using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSpawner : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject objectToSpawn;

    [SerializeField]
    private List<Transform> spawnLocations;

    private Transform lastSpawnLocation; // Zapamiêtuje ostatni¹ lokalizacjê, aby nie spawnowaæ w tym samym miejscu dwa razy pod rz¹d.

    // Implementacja interakcji - wywo³ywana, gdy gracz wejdzie w interakcjê
    public void Interact()
    {
        // Sprawdzamy, czy mamy prefab do zespawnowania i spawnLocations
        if (objectToSpawn != null && spawnLocations != null)
        {
            // Szukamy wolnego miejsca
            Transform spawnLocation = GetFreeSpawnLocation();
            if (spawnLocation != null)
            {
                // Tworzenie nowego obiektu w podanej lokalizacji
                Instantiate(objectToSpawn, spawnLocation.position, Quaternion.identity);
                lastSpawnLocation = spawnLocation;
                Debug.Log("Zespawnowano nowy obiekt: " + objectToSpawn.name);
            }
            else
            {
                Debug.LogWarning("Brak wolnych miejsc do spawnowania!");
            }
        }
        else
        {
            Debug.LogWarning("Brak prefabrykatów do spawnowania lub miejsc spawnowania!");
        }
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
