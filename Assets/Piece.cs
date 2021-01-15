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
    public DragRotation dragRotation;

    float originRotation;
    void generateInnerPoints()
    {
        Vector2 certerPoint = Vector2.zero;
        foreach (Vector2 targetPoint in collider.points)
        {
            certerPoint += targetPoint;
        }
        //Debug.Log("total point count "+ collider.points.Length);
        certerPoint /= (float)collider.points.Length;

        //Debug.Log("center " + certerPoint);
        foreach (Vector2 targetPoint in collider.points)
        {
            Vector2 diff = certerPoint - targetPoint;
            Vector2 diffNormalize = diff * innerEpsilon;
            Vector2 innerPoint = targetPoint + diffNormalize;

            //Debug.Log("diff " + diff+" "+ diffNormalize);
            innerPoints.Add(innerPoint);
        }
        //Debug.Log("collider points = " + String.Join("",
        //    new List<Vector2>(collider.points)
        //    .ConvertAll(i => i.ToString())
        //    .ToArray()));
        //Debug.Log("inner points = " + String.Join("",
        //    innerPoints
        //    .ConvertAll(i => i.ToString())
        //    .ToArray()));
    }

    private void Start()
    {
        collider = GetComponent<PolygonCollider2D>();
        generateInnerPoints();
    }

    public void OnMouseDown()
    {
        isDragging = true;
        originRotation = transform.eulerAngles.z;
        if (dragRotation)
        {
            GlobalValue.Instance.UnselectCurrentPiece();
        }
    }

    public void OnMouseUp()
    {

        GlobalValue.Instance.SelectPiece(dragRotation);
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 closestPoint = Vector2.negativeInfinity;
            Vector2 snapToPoint = Vector2.zero;
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
                            snapToPoint = tempClosestPoint;
                            moveDis = tempClosestPoint - colliderPointPosition;
                        }
                    }
                }
            }

            if(distance< float.PositiveInfinity)
            {

                //check if all inner points is inside of target
                bool allIn = true;
                float selectedRotation = 0;
                float[] rotateDegrees = {0, 45, -45, 90, -90 };
                foreach (float rotateDegree in rotateDegrees)
                {
                    allIn = true;
                    foreach (Vector2 innerPoint in innerPoints)
                    {
                        Vector2 innerPointInTargetSpace = innerPoint + mousePosition + moveDis;// + (Vector2)targetCollider.transform.position;
                        Vector2 pivot = snapToPoint;

                        Vector2 dir = innerPointInTargetSpace - pivot;
                        dir = Quaternion.Euler(new Vector3(0,0,rotateDegree)) * dir;
                        Vector2 innerPointAfterRotation = dir+ pivot;
                        if (!targetCollider.OverlapPoint(innerPointAfterRotation))
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
                        selectedRotation = rotateDegree;
                        break;
                    }
                }
                if (allIn)
                {
                    Vector2 pivot = snapToPoint;
                    Vector2 dir = closestPoint - pivot;
                    dir = Quaternion.Euler(new Vector3(0, 0, selectedRotation)) * dir;
                    closestPoint = dir + pivot;
                    transform.position = closestPoint;
                    transform.eulerAngles = new Vector3(0,0,selectedRotation);
                    return;
                }
                else
                {
                }

            }
            transform.position = mousePosition;
            transform.eulerAngles = new Vector3(0, 0, originRotation);

            //Debug.Log(closestPoint+" "+ mousePosition+" "+ (closestPoint - mousePosition).SqrMagnitude());

        }
    }
}