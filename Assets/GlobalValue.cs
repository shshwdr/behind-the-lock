﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValue : Singleton<GlobalValue>
{
    public DragRotation lastSelected;
    public List<Piece> pieces;
    public void UnselectCurrentPiece()
    {

        if (lastSelected)
        {
            lastSelected.isSelected = false;
            lastSelected.gameObject.SetActive(false);
        }
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

                //return;
            }
            else
            {
                res.Add(true);
            }
        }
        //Debug.Log("collider points = " + String.Join("",
        //    res
        //    .ConvertAll(i => i.ToString())
        //    .ToArray()));
        if (isWin)
        {

            Debug.Log("WIN");
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

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
        }
    }
}