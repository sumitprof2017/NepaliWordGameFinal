using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private Coroutine shakeRoutine;
    private RectTransform rectTransform;
    private Vector2 originalPos;

    private void Awake()
    {
        instance = this;
        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.anchoredPosition; // Store the original anchored position
    }

    public void ShakeUI(float duration = 0.2f, float magnitude = 20f)
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Smooth horizontal shake
            float x = (Mathf.PerlinNoise(Time.time * 25f, 0f) * 2f - 1f) * magnitude;
            // Small vertical shake
            float y = Random.Range(-1f, 1f) * magnitude * 0.5f;

            rectTransform.anchoredPosition = originalPos + new Vector2(x, y);

            // Optional: small rotation around Z-axis for UI
            float rot = Random.Range(-1f, 1f) * magnitude * 0.5f;
            rectTransform.localRotation = Quaternion.Euler(0, 0, rot);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalPos;
        rectTransform.localRotation = Quaternion.identity;
        shakeRoutine = null;
    }

    [ContextMenu("Test Shake")]
    void TestShake()
    {
        ShakeUI(0.5f, 30f); // UI shake in pixels
    }
}