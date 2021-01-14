using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValue : Singleton<GlobalValue>
{
    public DragRotation lastSelected;
    public void SelectPiece(DragRotation selected)
    {
        if (lastSelected)
        {

            lastSelected.isSelected = false;
        }
        lastSelected = selected;
        selected.isSelected = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
