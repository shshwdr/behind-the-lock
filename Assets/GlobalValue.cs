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
