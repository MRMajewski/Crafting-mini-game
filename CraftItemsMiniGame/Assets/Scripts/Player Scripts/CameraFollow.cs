using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private Vector3 offset = new Vector3(0, 5, -10);

    [SerializeField]
    private float moveDuration = 0.3f;

    [SerializeField]
    private Ease easing = Ease.InOutSine;

    private Tween moveTween;

    void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;

        // Zatrzymaj poprzedni tween (jeœli jeszcze trwa)
        if (moveTween != null && moveTween.IsActive())
            moveTween.Kill();

        // Stwórz nowy tween
        moveTween = transform.DOMove(targetPosition, moveDuration)
                             .SetEase(easing)
                             .SetUpdate(UpdateType.Late);
    }
}
