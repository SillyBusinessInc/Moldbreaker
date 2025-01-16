using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlatformPathController : MonoBehaviour
{
    [Header("Path Settings")]
    [SerializeField] private Transform pathParent; 
    private List<Vector3> controlPoints = new List<Vector3>();
    [SerializeField] private float platformSpeed = 5f; 
    
    [Header("Pause Settings")]
    [SerializeField] private float tStartPause = 0f; 
    [SerializeField] private float tEndPause = 0f;
    [SerializeField] private bool pause = false;
    private bool pausedTime = false;
    private float pauseTimer = 0f;

    private float t = 0f; 
    private int currentSegment = 0;
    private bool isReversing = false; 

    private void Start()
    {
        foreach (Transform child in pathParent)
        {
            controlPoints.Add(child.position);
        }
    }
    
    private void FixedUpdate()
    {
        if (pause) return; 
        if (pausedTime) return;

        if (controlPoints.Count < 4) return; 

        if (pauseTimer > 0)
        {
            pauseTimer -= Time.fixedDeltaTime;
            return;
        }

        MovePlatform();
    }

    private void MovePlatform()
    {
        Vector3 p0, p1, p2, p3;
        int controlPointCount = controlPoints.Count - 1;

        if (isReversing) {
            // Set reverse route
            p3 = controlPoints[Mathf.Clamp(currentSegment, 0, controlPointCount)];
            p2 = controlPoints[Mathf.Clamp(currentSegment-1, 0, controlPointCount)];
            p1 = controlPoints[Mathf.Clamp(currentSegment - 2, 0, controlPointCount)];
            p0 = controlPoints[Mathf.Clamp(currentSegment - 3, 0, controlPointCount)];
        } else {
            // Set forward path
            p0 = controlPoints[Mathf.Clamp(currentSegment - 1, 0, controlPointCount)];
            p1 = controlPoints[currentSegment];
            p2 = controlPoints[Mathf.Clamp(currentSegment + 1, 0, controlPointCount)];
            p3 = controlPoints[Mathf.Clamp(currentSegment + 2, 0, controlPointCount)];
        }

        Vector3 positionOnCurve = GetCatmullRomPosition(t, p0, p1, p2, p3);
        transform.position = positionOnCurve;

        t += Time.deltaTime * platformSpeed * (isReversing ? -1f : 1f);

        if (t > 1f) {
            t = 0f;
            currentSegment++;

            if (currentSegment >= controlPoints.Count - 1) {
                currentSegment = controlPoints.Count;
                t = 1f;
                isReversing = true; 
                StartCoroutine(PauseAtPosition(tEndPause)); 
            }
        } else if (t < 0f) {
            t = 1f;
            currentSegment--;

            if (currentSegment <= 1) {
                currentSegment = 0; 
                t = 0f;
                isReversing = false; 
                StartCoroutine(PauseAtPosition(tStartPause));
            }
        }

        // Debug.Log($"Segment: {currentSegment}, Reversing: {isReversing}");
    }

    private Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3
        );
    }

    private IEnumerator PauseAtPosition(float duration)
    {
        pausedTime = true;
        yield return new WaitForSeconds(duration);
        pausedTime = false;
    }
}
