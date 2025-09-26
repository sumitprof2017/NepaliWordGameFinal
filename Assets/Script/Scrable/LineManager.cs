using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    [Header("LineRederer")]
    public List<GameObject> linesList;
    public LineRenderer lineCreator;
    Vector3 currentPosition;
    /*float fpsCounter;
    int animationStep;
    [SerializeField] private Texture[] tex;*/

    void Start()
    {
        lineCreator = GetComponent<LineRenderer>();
        
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPosition.z = 0f;
            lineCreator.SetPosition(lineCreator.positionCount - 1, currentPosition);
            /*LaserAnimation();*/
        }
    }
    /* private void LaserAnimation()
    {
        // animating laser

        fpsCounter += Time.deltaTime;
        if (fpsCounter >= 0.1f)
        {
            animationStep++;
            if (animationStep >= tex.Length)
                animationStep = 0;
            lineCreator.material.SetTexture("_MainTex", tex[animationStep]);
            lineCreator.material.SetTexture("_EmissionMap", tex[animationStep]);
            fpsCounter = 0f;
        }

        // -------x--------
    }*/
}


