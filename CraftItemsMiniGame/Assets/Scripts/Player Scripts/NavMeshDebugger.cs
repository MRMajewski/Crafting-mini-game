using UnityEngine;
using UnityEngine.AI;

public class NavMeshDebugger : MonoBehaviour
{
    [Header("Opcjonalny prefab markera (np. zielona kropka)")]
    public GameObject clickMarkerPrefab;

    public Transform character;


    [Header("Ustawienia debug")]
    public Color rayColor = Color.green;
    public Color failedColor = Color.red;
    public float rayLength = 100f;
    public float markerLifetime = 1.5f;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick(Input.mousePosition);
        }
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            HandleClick(Input.GetTouch(0).position);
        }
#endif
    }

    private void HandleClick(Vector2 screenPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, rayLength))
        {
            Debug.DrawLine(ray.origin, hit.point, rayColor, 1f);
            Debug.DrawLine(character.position, hit.point, rayColor, 1f);

            if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
            {
                Debug.Log($"✅ Klik na NavMesh: {navHit.position}");

                if (clickMarkerPrefab)
                {
                    GameObject marker = Instantiate(clickMarkerPrefab, navHit.position + Vector3.up * 0.1f, Quaternion.identity);
                    Destroy(marker, markerLifetime);
                }
            }
            else
            {
                Debug.DrawLine(ray.origin, hit.point, failedColor, 1.5f);
                Debug.LogWarning("❌ Klik poza NavMeshem!");
            }
        }
    }
}
