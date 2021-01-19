using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTargetEditorHelper : MonoBehaviour
{
    PolygonCollider2D collider;
    List<TargetPiece> otherTargetPieces;
    public float snapDistance = 1;

    // Start is called before the first frame update
    private void Start()
    {
        collider = GetComponent<PolygonCollider2D>();

        GetOtherPieces();


    }
    public void GetOtherPieces()
    {
        otherTargetPieces = new List<TargetPiece>();
        foreach (TargetPiece tp in GetComponentsInChildren<TargetPiece>())
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
    public void Align()
    {
        // if (otherTargetPieces == null || otherTargetPieces.Count == 0 ||collider==null)
        {
            Start();
        }
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 closestPoint = Vector2.negativeInfinity;
        Vector2 snapToPoint = Vector2.zero;//the point on target that will snap to
        Vector2 moveDis = Vector2.zero;
        //for each rotation, check for each collider point, if it is close to a target point
        float rotateDegree = 0;
        // foreach (float rotateDegree in rotateDegrees)


        bool allIn = true;
        Vector2[] newColliderPoints = collider.points;
        int i = 0;
        foreach (Vector2 colliderPoint in collider.points)
        {

            float distance = float.PositiveInfinity;//closest distance from any point on piece to target points
                                                    //rotate collider point
            Vector2 colliderPointAfterRotation = rotateAroundVector(colliderPoint, Vector2.zero, transform.rotation.eulerAngles.z);
            Vector2 colliderPointPosition = colliderPointAfterRotation + (Vector2)transform.position;

            List<Vector2> snapPoints = new List<Vector2>();

            foreach (TargetPiece otherPiece in otherTargetPieces)
            {
                foreach (Vector2 otherPieceColliderPoint in otherPiece.getCollider().points)
                {
                    Vector2 opPointBeforeRotation = otherPieceColliderPoint;
                    Vector2 opPointAFterRotation = rotateAroundVector(opPointBeforeRotation, Vector2.zero, otherPiece.transform.rotation.eulerAngles.z);
                    Vector2 opPointAFterMove = opPointAFterRotation + (Vector2)otherPiece.transform.position;
                    snapPoints.Add(opPointAFterMove);
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

            if (distance < float.PositiveInfinity)
            {

                newColliderPoints[i] = snapToPoint - (Vector2)transform.position;

            }
            i++;
        }


        collider.points = newColliderPoints;
    }
}
