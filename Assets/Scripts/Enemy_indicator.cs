using UnityEngine;
using UnityEngine.UI;

public class EnemyIndicator : MonoBehaviour
{
    public GameObject indicatorPrefab;
    public float indicatorOffset = 50f;

    private Camera mainCamera;
    private GameObject indicator;
    private RectTransform indicatorRect;
    private Canvas gameCanvas;

    void Start()
    {
        mainCamera = Camera.main;
        CreateOrFindCanvas();
        CreateIndicator();
    }

    void CreateOrFindCanvas()
    {
        gameCanvas = FindObjectOfType<Canvas>();

        if (gameCanvas == null)
        {
            GameObject canvasObj = new GameObject("UI Canvas");
            gameCanvas = canvasObj.AddComponent<Canvas>();
            gameCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
    }

    void CreateIndicator()
    {
        if (indicatorPrefab != null && gameCanvas != null)
        {
            indicator = Instantiate(indicatorPrefab, gameCanvas.transform);
            indicatorRect = indicator.GetComponent<RectTransform>();
            indicator.SetActive(false);
        }
        else
        {
            Debug.LogError("Falta indicatorPrefab o gameCanvas en EnemyIndicator");
        }
    }

    void Update()
    {
        if (indicator == null || mainCamera == null) return;

        bool isInView = IsVisibleToCamera();
        indicator.SetActive(!isInView);

        if (!isInView)
        {
            UpdateIndicatorPosition();
        }
    }

    bool IsVisibleToCamera()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);

        bool inView = screenPoint.z > 0 &&
                      screenPoint.x > 0 && screenPoint.x < 1 &&
                      screenPoint.y > 0 && screenPoint.y < 1;

        return inView;
    }

    void UpdateIndicatorPosition()
    {
        Vector3 screenPos = mainCamera.WorldToViewportPoint(transform.position);
        Vector2 canvasSize = new Vector2(gameCanvas.GetComponent<RectTransform>().rect.width,
                                         gameCanvas.GetComponent<RectTransform>().rect.height);
        Vector2 screenCenter = new Vector2(0.5f, 0.5f);
        Vector2 direction = new Vector2(screenPos.x - screenCenter.x, screenPos.y - screenCenter.y);

        direction.Normalize();

        float angleRad = Mathf.Atan2(direction.y, direction.x);
        float angleDeg = angleRad * Mathf.Rad2Deg;

        Vector2 position = screenCenter + direction * 0.5f;
        position.x = Mathf.Clamp(position.x, 0.05f, 0.95f);
        position.y = Mathf.Clamp(position.y, 0.05f, 0.95f);

        Vector2 positionInCanvas = new Vector2(
            (position.x * canvasSize.x) - (canvasSize.x * 0.5f),
            (position.y * canvasSize.y) - (canvasSize.y * 0.5f)
        );

        indicatorRect.anchoredPosition = positionInCanvas;
        indicatorRect.rotation = Quaternion.Euler(0, 0, angleDeg - 90);
    }

    void OnDestroy()
    {
        // Destruir el indicador cuando se destruye el enemigo
        if (indicator != null)
        {
            Destroy(indicator);
        }
    }
}