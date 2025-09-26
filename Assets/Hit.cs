using UnityEngine;

public class ScaleWithCanvas : MonoBehaviour
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Camera canvasCamera;
    private Vector2 previousDimensions;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasCamera = canvas.worldCamera;

        if (rectTransform == null || canvas == null || canvasCamera == null)
        {
            Debug.LogError("RectTransform, Canvas, or Camera not found!");
            return;
        }

        // Initial scale setup
        UpdateScale();

        // Save initial dimensions
        previousDimensions = new Vector2(Screen.width, Screen.height);
    }

    void Update()
    {
        // Check if dimensions have changed
        Vector2 currentDimensions = new Vector2(Screen.width, Screen.height);

        if (currentDimensions != previousDimensions)
        {
            // Update scale if dimensions changed
            UpdateScale();

            // Save current dimensions
            previousDimensions = currentDimensions;
        }
    }

    private void UpdateScale()
    {
        // Convert screen dimensions to world space
        Vector3 screenSizeWorld = canvasCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, canvasCamera.nearClipPlane));

        // Calculate scale based on world space dimensions
        float scaleX = screenSizeWorld.x / rectTransform.sizeDelta.x;
        float scaleY = screenSizeWorld.y / rectTransform.sizeDelta.y;

        // Apply scale to the RectTransform
        rectTransform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
}
