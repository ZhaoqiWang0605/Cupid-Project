using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
    private GameObject moveObject;
    public List<Vector3> relativeMovePoints;
    private List<Vector3> absoluteMovePoints = new List<Vector3>();
    private Vector3 velocity = Vector2.zero;
    public float smoothTime;
    private int nextPoint = 0;
    private int progress = 0;
    private Vector2 centerPoint;

    // Use this for initialization
    void Start()
    {
        moveObject = gameObject;
        centerPoint = moveObject.transform.position;
        foreach (Vector2 point in relativeMovePoints)
        {
            Vector2 absolutePoint = centerPoint + point;
            absoluteMovePoints.Add(absolutePoint);
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveObject.transform.position = Vector3.SmoothDamp(moveObject.transform.position, absoluteMovePoints[nextPoint], ref velocity, smoothTime);
        UpdatePoint(Time.deltaTime);
    }

    void UpdatePoint(float deltaTime)
    {
        progress++;
        if (progress >= 100)
        {
            progress = 0;
            nextPoint++;
        }
        if (nextPoint >= absoluteMovePoints.Count)
        {
            nextPoint = 0;
        }
    }
}
