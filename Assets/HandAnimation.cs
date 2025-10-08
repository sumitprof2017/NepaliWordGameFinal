using UnityEngine;

public class HandAnimation : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveTime = 1f;           // Time to move between points
    public GameObject handObject;         // The hand GameObject

    [Header("Line Renderer")]
    public LineRenderer lineRenderer;     // Line renderer to draw path

    [Header("LevelManager Reference")]
    public LevelManager levelManagerObj;  // Reference to LevelManager to get points

    [HideInInspector] public Transform pointA;  // Assigned from LevelManager
    [HideInInspector] public Transform pointB;  // Assigned from LevelManager
    [HideInInspector] public Transform pointC;  // Assigned from LevelManager

    void Start()
    {
        // Make sure all points are assigned
        if (pointA == null || pointB == null || pointC == null)
        {
            Debug.LogError("HandAnimation: Points A, B, or C are not assigned!");
            return;
        }

        // Set initial position to pointA
        handObject.transform.position = pointA.position;

        // Setup LineRenderer
        lineRenderer.positionCount = 3;
        lineRenderer.enabled = true;

        // Initially hide all line segments
        lineRenderer.SetPosition(0, pointA.position);
        lineRenderer.SetPosition(1, pointA.position);
        lineRenderer.SetPosition(2, pointA.position);

        // Start the looping movement
        MoveSequence();
    }

    /// <summary>
    /// Loops the hand movement A → B → C → reset → repeat
    /// </summary>
    void MoveSequence()
    {
        // Move from pointA → pointB
        LeanTween.move(handObject, pointB.position, moveTime).setOnStart(() =>
        {
            // Enable first segment (A → B)
            lineRenderer.SetPosition(1, pointB.position);
        }).setOnComplete(() =>
        {
            // Move from pointB → pointC
            LeanTween.move(handObject, pointC.position, moveTime).setOnStart(() =>
            {
                // Enable second segment (B → C)
                lineRenderer.SetPosition(2, pointC.position);
            }).setOnComplete(() =>
            {
                // Reset to pointA
                LeanTween.delayedCall(0.3f, () =>
                {
                    lineRenderer.enabled = false;

                    LeanTween.delayedCall(0.2f, () =>
                    {
                        // Reset position and line
                        handObject.transform.position = pointA.position;
                        lineRenderer.enabled = true;

                        lineRenderer.SetPosition(0, pointA.position);
                        lineRenderer.SetPosition(1, pointA.position);
                        lineRenderer.SetPosition(2, pointA.position);

                        // Loop again
                        MoveSequence();
                    });
                });
            });
        });
    }

    /// <summary>
    /// Call this to stop the hand animation and all LeanTweens on it
    /// </summary>
    public void StopHandAnimation()
    {
        LeanTween.cancel(handObject);
        lineRenderer.enabled = false;
    }
}