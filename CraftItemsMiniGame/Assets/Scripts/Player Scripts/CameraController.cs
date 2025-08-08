using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private Vector3 offset = new Vector3(0, 5, -10);

    [SerializeField]
    private float moveDuration = 0.3f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField]
    private Ease easing = Ease.InOutSine;

    private Tween moveTween;

    private bool isFollowingPlayer = false;

    [Header("Camera Positions references")]
    [SerializeField]
    private Transform cameraEndGameTransform;
    [SerializeField]
    private Transform cameraMainMenuTransform;
    [SerializeField]
    private Transform cameraOnPlayerZoomTransform;

    [SerializeField]
    private Transform cameraFollowTransform;

    private void LateUpdate()
    {
        if (!isFollowingPlayer || player == null) return;

        Vector3 targetPos = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, moveDuration);
    }

    public void SetMainMenuCameraPosition()
    {
        transform.position = cameraMainMenuTransform.position;
        transform.rotation = cameraMainMenuTransform.rotation;

        Camera.main.fieldOfView = 45f;
    }

    public void SetEndGameCameraPosition()
    {
       transform.position = cameraEndGameTransform.position;
       transform.rotation = cameraEndGameTransform.rotation;

        Camera.main.fieldOfView = 15f;
    }

    public void SetCameraFollowing(bool shouldFollow)
    {
        isFollowingPlayer = shouldFollow;
        if(shouldFollow)
        {
            Camera.main.fieldOfView = 40f;
            transform.rotation = cameraFollowTransform.rotation;
        }
    }
    public void SetCameraFollowingFromMainMenu(System.Action onComplete = null)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOMove(cameraOnPlayerZoomTransform.position, 0.4f).SetEase(Ease.InOutSine))
            .Append(transform.DORotateQuaternion(cameraFollowTransform.rotation, 0.4f).SetEase(Ease.InOutElastic))
            .Append(Camera.main.DOFieldOfView(40f, 0.75f).SetEase(Ease.InOutBounce))
            .OnComplete(() =>
            {
                isFollowingPlayer = true;
                onComplete?.Invoke();
            });
    }
}
