using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
[ExecuteInEditMode]
public class TargetPiece : MonoBehaviour
{
    private bool isDragging;
    public float snapDistance = 1;
    PolygonCollider2D collider;
    [HideInInspector]
    public List<Vector2> innerPoints;
    float innerEpsilon = 0.1f;
    public DragRotation dragRotation;
    public bool isLocked = false;
    int originOrder;
    SpriteRenderer spriteRenderer;
    float originRotation;
    List<TargetPiece> otherTargetPieces;
    void generateInnerPoints()
    {
        innerPoints = new List<Vector2>();
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        originOrder = spriteRenderer.sortingOrder;

        GetOtherPieces();


    }

    public void GetOtherPieces()
    {
        otherTargetPieces = new List<TargetPiece>();
        foreach (TargetPiece tp in transform.parent.GetComponentsInChildren<TargetPiece>())
        {
            if (tp != this)
            {
                otherTargetPieces.Add(tp);
            }
        }
    }




    Vector2 rotateAroundVector(Vector2 originPosition, Vector2 pivot, float degree)
    {
        Vector2 dir = originPosition - pivot;
        dir = Quaternion.Euler(new Vector3(0, 0, degree)) * dir;
        Vector2 res = dir + pivot;
        return res;
    }
    public PolygonCollider2D getCollider()
    {
        if (!collider)
        {
            collider = GetComponent<PolygonCollider2D>();
        }

        return collider;
    }
    public void Align()
    {
       // if (otherTargetPieces == null || otherTargetPieces.Count == 0 ||collider==null)
        {
            Start();
        }


        //rotate self
        float zRotation = transform.rotation.eulerAngles.z;
        zRotation = Mathf.Round(zRotation / 45f) * 45;
        transform.eulerAngles = new Vector3(0, 0, zRotation);


        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 closestPoint = Vector2.negativeInfinity;
        Vector2 snapToPoint = Vector2.zero;//the point on target that will snap to
        float distance = float.PositiveInfinity;//closest distance from any point on piece to target points
        Vector2 moveDis = Vector2.zero;
        //for each rotation, check for each collider point, if it is close to a target point
        float rotateDegree = 0;
        // foreach (float rotateDegree in rotateDegrees)
        {

            bool allIn = true;
            float testRotateDegree = rotateDegree + originRotation;
            foreach (Vector2 colliderPoint in collider.points)
            {
                //rotate collider point
                Vector2 colliderPointAfterRotation = rotateAroundVector(colliderPoint, Vector2.zero, transform.rotation.eulerAngles.z);
                Vector2 colliderPointPosition = colliderPointAfterRotation + (Vector2)transform.localPosition;

                List<Vector2> snapPoints = new List<Vector2>();

                foreach (TargetPiece otherPiece in otherTargetPieces)
                {
                    foreach (Vector2 otherPieceColliderPoint in otherPiece.getCollider().points)
                    {
                        Vector2 opPointBeforeRotation = otherPieceColliderPoint;
                        Vector2 opPointAFterRotation = rotateAroundVector(opPointBeforeRotation, Vector2.zero, otherPiece.transform.rotation.eulerAngles.z);
                        Vector2 opPointAFterMove = opPointAFterRotation + (Vector2)otherPiece.transform.localPosition;
                        snapPoints.Add(opPointAFterMove);
                    }
                }

                foreach (Vector2 targetPoint in snapPoints)
                {
                    Vector2 tempClosestPoint = targetPoint ;
                    float tempDistance = (tempClosestPoint - colliderPointPosition).SqrMagnitude();
                    if (tempDistance <= snapDistance)
                    {
                        if (tempDistance < distance)
                        {
                            distance = tempDistance;
                            closestPoint = tempClosestPoint - colliderPointAfterRotation;
                            snapToPoint = tempClosestPoint;
                            moveDis = tempClosestPoint - colliderPointPosition;
                        }
                    }
                }
            }

            if (distance < float.PositiveInfinity)
            {

                //check if all inner points is inside of target
                //foreach (float rotateDegree in rotateDegrees)
                //{
                allIn = true;
                foreach (Vector2 innerPoint in innerPoints)
                {
                    Vector2 innerPointAfterRotation = rotateAroundVector(innerPoint, Vector2.zero, testRotateDegree);
                    Vector2 innerPointInTargetSpace = innerPointAfterRotation + mousePosition + moveDis;// + (Vector2)targetCollider.transform.position;
                                                                                                        //Vector2 pivot = snapToPoint;

                    //Vector2 dir = innerPointInTargetSpace - pivot;
                    //dir = Quaternion.Euler(new Vector3(0, 0, newRotateDegree)) * dir;
                    //Vector2 innerPointAfterRotation = dir + pivot;
                    //if (!targetCollider.OverlapPoint(innerPointInTargetSpace))
                    //{

                    //    allIn = false;
                    //    break;
                    //}
                    //check all locked piece does not overlap
                    foreach (TargetPiece otherPiece in otherTargetPieces)
                    {
                        if (otherPiece != this && otherPiece.isLocked)
                        {
                            if (otherPiece.collider.OverlapPoint(innerPointInTargetSpace))
                            {
                                allIn = false;
                                break;
                            }
                        }
                    }
                }
                if (allIn)
                {
                    //selectedRotation = newRotateDegree;
                    //Vector2 pivot = snapToPoint;
                    //Vector2 dir = closestPoint - pivot;
                    //dir = Quaternion.Euler(new Vector3(0, 0, selectedRotation)) * dir;
                    //closestPoint = dir + pivot;
                    //transform.position = closestPoint;
                    //float moveTime = 0.4f;
                    //transform.DOMove(closestPoint, moveTime);

                    //transform.DORotate(new Vector3(0, 0, testRotateDegree), moveTime);
                    transform.localPosition = closestPoint;
                    //transform.eulerAngles = new Vector3(0, 0, testRotateDegree);
                    //isLocked = true;
                    return;
                }

            }
        }
    }

    void Update()
    {
        //if(otherTargetPieces == null || otherTargetPieces.Count == 0)
        //{
        //    Start();
        //}

        ////if (isDragging)
        //{


        //    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector2 closestPoint = Vector2.negativeInfinity;
        //    Vector2 snapToPoint = Vector2.zero;//the point on target that will snap to
        //    float distance = float.PositiveInfinity;//closest distance from any point on piece to target points
        //    Vector2 moveDis = Vector2.zero;
        //    //for each rotation, check for each collider point, if it is close to a target point

        //    float[] rotateDegrees = { 0, 45, -45, 90, -90, 135, -135, 180 };
        //    if (!GlobalValue.Instance.AutoRotation)
        //    {
        //        rotateDegrees = new float[] { 0 };
        //    }
        //    foreach (float rotateDegree in rotateDegrees)
        //    {

        //        bool allIn = true;
        //        float testRotateDegree = rotateDegree + originRotation;
        //        foreach (Vector2 colliderPoint in collider.points)
        //        {
        //            //rotate collider point
        //            Vector2 colliderPointAfterRotation = rotateAroundVector(colliderPoint, Vector2.zero, testRotateDegree);
        //            Vector2 colliderPointPosition = colliderPointAfterRotation + mousePosition;

        //            List<Vector2> snapPoints = new List<Vector2>();

        //            foreach (TargetPiece otherPiece in otherTargetPieces)
        //            {
        //                foreach (Vector2 otherPieceColliderPoint in otherPiece.collider.points)
        //                {
        //                    Vector2 opPointBeforeRotation = otherPieceColliderPoint;
        //                    Vector2 opPointAFterRotation = rotateAroundVector(opPointBeforeRotation, Vector2.zero, otherPiece.transform.rotation.eulerAngles.z);
        //                    Vector2 opPointAFterMove = opPointAFterRotation +(Vector2) otherPiece.transform.position;
        //                    snapPoints.Add(opPointAFterMove);
        //                }
        //            }

        //            foreach (Vector2 targetPoint in snapPoints)
        //            {
        //                Vector2 tempClosestPoint = targetPoint + (Vector2)targetCollider.transform.position;
        //                float tempDistance = (tempClosestPoint - colliderPointPosition).SqrMagnitude();
        //                if (tempDistance <= snapDistance)
        //                {
        //                    if (tempDistance < distance)
        //                    {
        //                        distance = tempDistance;
        //                        closestPoint = tempClosestPoint - colliderPointAfterRotation;
        //                        snapToPoint = tempClosestPoint;
        //                        moveDis = tempClosestPoint - colliderPointPosition;
        //                    }
        //                }
        //            }
        //        }

        //        if (distance < float.PositiveInfinity)
        //        {

        //            //check if all inner points is inside of target
        //            //foreach (float rotateDegree in rotateDegrees)
        //            //{
        //            allIn = true;
        //            foreach (Vector2 innerPoint in innerPoints)
        //            {
        //                Vector2 innerPointAfterRotation = rotateAroundVector(innerPoint, Vector2.zero, testRotateDegree);
        //                Vector2 innerPointInTargetSpace = innerPointAfterRotation + mousePosition + moveDis;// + (Vector2)targetCollider.transform.position;
        //                                                                                                    //Vector2 pivot = snapToPoint;

        //                //Vector2 dir = innerPointInTargetSpace - pivot;
        //                //dir = Quaternion.Euler(new Vector3(0, 0, newRotateDegree)) * dir;
        //                //Vector2 innerPointAfterRotation = dir + pivot;
        //                //if (!targetCollider.OverlapPoint(innerPointInTargetSpace))
        //                //{

        //                //    allIn = false;
        //                //    break;
        //                //}
        //                //check all locked piece does not overlap
        //                foreach (TargetPiece otherPiece in otherTargetPieces)
        //                {
        //                    if (otherPiece != this && otherPiece.isLocked)
        //                    {
        //                        if (otherPiece.collider.OverlapPoint(innerPointInTargetSpace))
        //                        {
        //                            allIn = false;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            if (allIn)
        //            {
        //                //selectedRotation = newRotateDegree;
        //                //Vector2 pivot = snapToPoint;
        //                //Vector2 dir = closestPoint - pivot;
        //                //dir = Quaternion.Euler(new Vector3(0, 0, selectedRotation)) * dir;
        //                //closestPoint = dir + pivot;
        //                //transform.position = closestPoint;
        //                float moveTime = 0.4f;
        //                //transform.DOMove(closestPoint, moveTime);

        //                //transform.DORotate(new Vector3(0, 0, testRotateDegree), moveTime);
        //                transform.position = closestPoint;
        //                transform.eulerAngles = new Vector3(0, 0, testRotateDegree);
        //                isLocked = true;
        //                return;
        //            }

        //        }
        //    }
        //    transform.position = mousePosition;
        //    transform.eulerAngles = new Vector3(0, 0, originRotation);
        //    isLocked = false;

        //    //Debug.Log(closestPoint+" "+ mousePosition+" "+ (closestPoint - mousePosition).SqrMagnitude());

        //}
    }
}