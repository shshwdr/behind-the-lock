using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
public class Piece : MonoBehaviour
{
    public PolygonCollider2D targetCollider;
    private bool isDragging;
    public float snapDistance = 1;
    PolygonCollider2D collider;
    [HideInInspector]
    public List<Vector2> innerPoints;
    float innerEpsilon = 0.2f;
    public DragRotation dragRotation;
    public bool isLocked = false;
    int originOrder;
    SpriteRenderer spriteRenderer;
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
        DOTween.Init();
        collider = GetComponent<PolygonCollider2D>();
        generateInnerPoints();
        //GlobalValue.Instance.pieces.Add(this);
        spriteRenderer = GetComponent<SpriteRenderer>();
        originOrder = spriteRenderer.sortingOrder;
    }

    public void OnMouseDown()
    {
        if (Utils.Pause || Utils.End)
        {
            isDragging = false;
            return;
        }
        spriteRenderer.sortingOrder = originOrder + 1;
        isDragging = true;
        originRotation = transform.eulerAngles.z;
        if (dragRotation)
        {
            GlobalValue.Instance.UnselectCurrentPiece();
        }
    }

    public void OnMouseUp()
    {
        if (Utils.Pause || Utils.End)
        {
            isDragging = false;
            return;
        }
        spriteRenderer.sortingOrder = originOrder;
        GlobalValue.Instance.SelectPiece(dragRotation);
        isDragging = false;
        if (isLocked)
        {
            Debug.Log("lock!");
            GlobalValue.Instance.CheckWin();
           StartCoroutine( CheckLockEvent());
        }
    }

    IEnumerator CheckLockEvent()
    {
        string currentLevelName = Utils.currentSolvingLevel.GetComponent<LevelController>().levelName;
        int lockedCount = GlobalValue.Instance.lockedCount();
        string dialogueName = currentLevelName + "Put"+ lockedCount;

        //string dialogueHappened = currentLevelName + "Put" + lockedCount;
        bool dialogueHappened  = PixelCrushers.DialogueSystem.DialogueLua.GetVariable(dialogueName).AsBool;
        if (PixelCrushers.DialogueSystem.DialogueManager.HasConversation(dialogueName)&& !dialogueHappened)
        {

            GameObject resource = Resources.Load("Character/"+dialogueName) as GameObject;
            if (resource!=null)
            {
                GameObject resourceObject = Instantiate(resource);
                yield return StartCoroutine(resourceObject.GetComponent<Character>().WaitFinishStart());
            }

            PixelCrushers.DialogueSystem.DialogueLua.SetVariable(dialogueName,true);
            PixelCrushers.DialogueSystem.DialogueManager.StartConversation(dialogueName);
        }
        yield return null;
    }

    Vector2 rotateAroundVector(Vector2 originPosition, Vector2 pivot, float degree)
    {
        Vector2 dir = originPosition - pivot;
        dir = Quaternion.Euler(new Vector3(0, 0, degree)) * dir;
        Vector2 res = dir + pivot;
        return res;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 closestPoint = Vector2.negativeInfinity;
            Vector2 snapToPoint = Vector2.zero;//the point on target that will snap to
            float distance = float.PositiveInfinity;//closest distance from any point on piece to target points
            Vector2 moveDis = Vector2.zero;
            //for each rotation, check for each collider point, if it is close to a target point

            float[] rotateDegrees = { 0, 45, -45, 90, -90, 135, -135, 180 };
            if (!GlobalValue.Instance.AutoRotation)
            {
                rotateDegrees = new float[]{ 0};
            }
            foreach (float rotateDegree in rotateDegrees)
            {

                bool allIn = true;
                float testRotateDegree = rotateDegree + originRotation;
                foreach (Vector2 colliderPoint in collider.points)
                {
                    //rotate collider point
                    Vector2 colliderPointAfterRotation = rotateAroundVector(colliderPoint, Vector2.zero, testRotateDegree);
                    Vector2 colliderPointPosition = colliderPointAfterRotation + mousePosition;

                    List<Vector2> snapPoints = new List<Vector2>();

                    foreach (Vector2 targetPoint in targetCollider.points)
                    {
                        Vector2 tempClosestPoint = targetPoint + (Vector2)targetCollider.transform.position;
                        snapPoints.Add(tempClosestPoint);
                    }
                        foreach (Piece otherPiece in GlobalValue.Instance.pieces)
                    {
                        if (otherPiece != this && otherPiece.isLocked)
                        {
                            foreach (Vector2 otherPieceColliderPoint in otherPiece.collider.points)
                            {
                                Vector2 opPointBeforeRotation = otherPieceColliderPoint;
                                Vector2 opPointAFterRotation = rotateAroundVector(opPointBeforeRotation, Vector2.zero, otherPiece.transform.rotation.eulerAngles.z);
                                Vector2 opPointAFterMove = opPointAFterRotation + (Vector2)otherPiece.transform.position;
                                snapPoints.Add(opPointAFterMove);
                            }
                        }
                    }


                    foreach (Vector2 targetPoint in snapPoints)
                    {
                        Vector2 tempClosestPoint = targetPoint;
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
                        if (!targetCollider.OverlapPoint(innerPointInTargetSpace))
                        {

                            allIn = false;
                            break;
                        }
                        //check all locked piece does not overlap
                        foreach (Piece otherPiece in GlobalValue.Instance.pieces)
                        {
                            if (otherPiece != this && otherPiece.isLocked)
                            {
                                if(otherPiece.collider.OverlapPoint(innerPointInTargetSpace))
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
                        float moveTime = 0.2f;
                        transform.DOMove(closestPoint, moveTime);

                        transform.DORotate(new Vector3(0, 0, testRotateDegree), moveTime);
                        //transform.position = closestPoint;
                        //transform.eulerAngles = new Vector3(0, 0, testRotateDegree);
                        isLocked = true;
                        return;
                    }

                }
            }
            transform.position = mousePosition;
            transform.eulerAngles = new Vector3(0, 0, originRotation);
            isLocked = false;

            //Debug.Log(closestPoint+" "+ mousePosition+" "+ (closestPoint - mousePosition).SqrMagnitude());

        }
    }
}