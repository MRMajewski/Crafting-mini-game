using DG.Tweening;
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

        StartAnimationInLoop();
    }

    private void OnDestroy()
    {
        spawner.FreeSpawnLocation(spawnLocationTransform);
        punchTween?.Kill();
    }

    public override void StartAnimationInLoop()
    {

        if (spawnerModelTransform == null) return;
        baseScale = spawnerModelTransform.localScale;

        Vector3 scaledPunch = Vector3.Scale(baseScale, punchScale);

        punchTween = DOTween.Sequence()
            .AppendInterval(delayRange.x)
            .Append(spawnerModelTransform.DOPunchScale(
                scaledPunch,
                tweenDuration,
                punchVibrato,
                punchElasticity
            ))
            .AppendInterval(Random.Range(delayRange.x, delayRange.y))
            .SetLoops(-1, LoopType.Restart);

    }
}
