using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private Coroutine shakeRoutine;
    private Vector3 originalPos;

    private void Awake()
    {
        instance = this;
        originalPos = transform.localPosition;
    }

    public void ShakeCamera(float duration = 0.2f, float magnitude = 0.2f)
    {
        // Stop old shake
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Horizontal (X-axis) shake — stronger side-to-side
            float x = Mathf.PerlinNoise(Time.time * 25f, 0f) * 2f - 1f; // smooth noise
            x *= magnitude;

            // Small vertical movement (optional)
            float y = Random.Range(-0.2f, 0.2f) * magnitude * 0.5f;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            // Optional small rotational shake
            float rot = Random.Range(-1f, 1f) * magnitude * 15f;
            transform.localRotation = Quaternion.Euler(0, 0, rot);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        transform.localRotation = Quaternion.identity;
        shakeRoutine = null;
    }

    [ContextMenu("Test Shake")]
    void TestShake()
    {
        ShakeCamera(2f, 0.5f); // Try larger magnitude for stronger shake
    }
}
