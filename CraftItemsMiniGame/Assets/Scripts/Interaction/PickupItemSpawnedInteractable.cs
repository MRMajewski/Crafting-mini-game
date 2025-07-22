using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItemSpawnedInteractable : PickupItemInteractable, IInteractable
{
    private ItemsSpawnerInteractable spawner;
    private Transform spawnLocationTransform;

    public void InitSpawnedPickupItem(ItemsSpawnerInteractable spawner, Transform spawnLocationTransform)
    {
        this.spawner = spawner;
        this.spawnLocationTransform = spawnLocationTransform;
    }

    private void OnDestroy()
    {
        spawner.FreeSpawnLocation(spawnLocationTransform);
    }
}
