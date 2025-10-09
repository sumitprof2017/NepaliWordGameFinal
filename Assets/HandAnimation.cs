using TMPro;
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

    [Header("References")]
    public GameObject threePointer;      // Assign your "ThreePointer" parent
    public string devanagariWord = "कमल"; // Word to match (क, म, ल)


    private void Awake()
    {
        SetPoints();
    }

    void Start()
    {
        

        if (pointA == null || pointB == null || pointC == null)
        {
            Debug.LogError("HandAnimation: Points A, B, or C were not found!");
            return;
        }

        // Initial setup
        handObject.transform.position = pointA.position;
        lineRenderer.positionCount = 3;
        lineRenderer.enabled = true;

        // Reset line
        lineRenderer.SetPosition(0, pointA.position);
        lineRenderer.SetPosition(1, pointA.position);
        lineRenderer.SetPosition(2, pointA.position);

        // Start animation
        MoveSequence();
    }

    private void OnDisable()
    {
        LeanTween.cancel(handObject);
    }

    /// <summary>
    /// Automatically finds and assigns the correct points based on letters
    /// </summary>
    public void SetPoints()
    {
        if (threePointer == null)
        {
            Debug.LogError("ThreePointer reference not set!");
            return;
        }

        // Split the Devanagari word into individual letters
        char[] letters = devanagariWord.ToCharArray();

        // Get all letter components under ThreePointer
        TextMeshProUGUI[] allLetters = threePointer.GetComponentsInChildren<TextMeshProUGUI>(true);

        Transform[] foundPoints = new Transform[letters.Length];

        // Loop through each target letter
        for (int i = 0; i < letters.Length; i++)
        {
            string letterToFind = letters[i].ToString();

            foreach (var letterObj in allLetters)
            {
                if (letterObj.text == letterToFind)
                {
                    foundPoints[i] = letterObj.transform;
                    break;
                }
            }
        }

        // Assign to your points
        if (foundPoints.Length >= 3)
        {
            pointA = foundPoints[0];
            pointB = foundPoints[1];
            pointC = foundPoints[2];
        }

        Debug.Log($"Points set for word {devanagariWord}: " +
                  $"{pointA?.name}, {pointB?.name}, {pointC?.name}");
    }

    void MoveSequence()
    {
        LeanTween.move(handObject, pointB.position, moveTime).setOnStart(() =>
        {
            lineRenderer.SetPosition(1, pointB.position);
        }).setOnComplete(() =>
        {
            LeanTween.move(handObject, pointC.position, moveTime).setOnStart(() =>
            {
                lineRenderer.SetPosition(2, pointC.position);
            }).setOnComplete(() =>
            {
                LeanTween.delayedCall(0.3f, () =>
                {
                    lineRenderer.enabled = false;

                    LeanTween.delayedCall(0.2f, () =>
                    {
                        handObject.transform.position = pointA.position;
                        lineRenderer.enabled = true;
                        lineRenderer.SetPosition(0, pointA.position);
                        lineRenderer.SetPosition(1, pointA.position);
                        lineRenderer.SetPosition(2, pointA.position);
                        MoveSequence();
                    });
                });
            });
        });
    }

    public void StopHandAnimation()
    {
        LeanTween.cancel(handObject);
        lineRenderer.enabled = false;
    }
}