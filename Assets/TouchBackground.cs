using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchBackground : MonoBehaviour
{
    public void OnMouseDown()
    {
        GlobalValue.Instance.UnselectCurrentPiece();
    }
}
