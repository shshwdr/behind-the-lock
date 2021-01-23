using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValue : Singleton<GlobalValue>
{
    public DragRotation lastSelected;
    public List<Piece> pieces;

    public List<LevelController> levels;

    public bool AutoRotation;
    public bool IgnoreTarget;
    public void CleanPieces()
    {
        pieces.Clear();
        lastSelected = null;
    }
    public void UnselectCurrentPiece()
    {

        if (lastSelected)
        {
            lastSelected.isSelected = false;
            lastSelected.gameObject.SetActive(false);
        }
    }

    public int lockedCount()
    {
        int res = 0;
        foreach (Piece piece in pieces)
        {
            if (!piece.isLocked)
            {
                
            }
            else
            {
                res++;
            }
        }
        return res;
    }

    public void CheckWin()
    {
        bool isWin = true;
        List<bool> res = new List<bool>();
        foreach(Piece piece in pieces)
        {
            if (!piece.isLocked)
            {
                res.Add(false);
                isWin = false;
                //break;
                //return;
            }
            else
            {
                res.Add(true);
            }
        }
        Debug.Log("collider points = " + String.Join("",
            res
            .ConvertAll(i => i.ToString())
            .ToArray()));
        if (isWin)
        {

            Debug.Log("WIN");
            Utils.finishLevel();
        }
    }

    //static void LogList(List<T>)
    //{
    //    Debug.Log("collider points = " + String.Join("",
    //        new List<Vector2>(collider.points)
    //        .ConvertAll(i => i.ToString())
    //        .ToArray()));
    //}
    public void SelectPiece(DragRotation selected)
    {
        UnselectCurrentPiece();
        lastSelected = selected;
        selected.isSelected = true;
        lastSelected.gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        //GameObject newLevel = Utils.InitLevel("Level1");

        //newLevel.GetComponent<LevelController>().StartSolvingLevel();
        //TargetGroupController.Instance.UpdateTargets(new List<LevelController>() { newLevel.GetComponent<LevelController>() });

        //Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
        }
    }
}
