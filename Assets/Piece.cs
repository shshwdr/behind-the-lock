using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public PolygonCollider2D targetCollider;
    private bool isDragging;
    public float snapDistance = 1;
    PolygonCollider2D collider;
    [HideInInspector]
    public List<Vector2> innerPoints;
    float innerEpsilon = 0.1f;
    void generateInnerPoints()
    {
        Vector2 certerPoint = Vector2.zero;
        foreach (Vector2 targetPoint in collider.points)
        {
            certerPoint += targetPoint;
        }
        Debug.Log("total point count "+ collider.points.Length);
        certerPoint /= (float)collider.points.Length;

        Debug.Log("center " + certerPoint);
        foreach (Vector2 targetPoint in collider.points)
        {
            Vector2 diff = certerPoint - targetPoint;
            Vector2 diffNormalize = diff * innerEpsilon;
            Vector2 innerPoint = targetPoint + diffNormalize;

            Debug.Log("diff " + diff+" "+ diffNormalize);
            innerPoints.Add(innerPoint);
        }
        Debug.Log("collider points = " + String.Join("",
            new List<Vector2>(collider.points)
            .ConvertAll(i => i.ToString())
            .ToArray()));
        Debug.Log("inner points = " + String.Join("",
            innerPoints
            .ConvertAll(i => i.ToString())
            .ToArray()));
    }

    private void Start()
    {
        collider = GetComponent<PolygonCollider2D>();
        generateInnerPoints();
    }

    public void OnMouseDown()
    {
        isDragging = true;
    }

    public void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 closestPoint = Vector2.negativeInfinity;
            float distance = float.PositiveInfinity;
            Vector2 moveDis = Vector2.zero;
            foreach (Vector2 colliderPoint in collider.points)
            {
                Vector2 colliderPointPosition = colliderPoint + mousePosition;
                foreach (Vector2 targetPoint in targetCollider.points)
                {
                    Vector2 tempClosestPoint = targetPoint + (Vector2)targetCollider.transform.position;
                    float tempDistance = (tempClosestPoint - colliderPointPosition).SqrMagnitude();
                    if (tempDistance <= snapDistance)
                    {
                        if (tempDistance < distance)
                        {
                            distance = tempDistance;
                            closestPoint = tempClosestPoint - colliderPoint;
                            moveDis = tempClosestPoint - colliderPointPosition;
                        }
                    }
                }
            }

            if(distance< float.PositiveInfinity)
            {

                //check if all inner points is inside of target
                bool allIn = true;
                foreach (Vector2 innerPoint in innerPoints)
                {
                    Vector2 innerPointInTargetSpace = innerPoint + mousePosition + moveDis;// + (Vector2)targetCollider.transform.position;
                    if (!targetCollider.OverlapPoint(innerPointInTargetSpace))
                    {
                        allIn = false;
                        break;
                    }
                    else
                    {

                    }
                }
                if (allIn)
                {

                    //if not, rotate
                    transform.position = closestPoint;
                    return;
                }

            }
            transform.position = mousePosition;

            //Debug.Log(closestPoint+" "+ mousePosition+" "+ (closestPoint - mousePosition).SqrMagnitude());
            
        }
    }
}