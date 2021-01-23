using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    LevelController levelController;
    // Start is called before the first frame update
    void Start()
    {
        levelController = transform.parent.GetComponent<LevelController>();
    }
    private void OnMouseDown()
    {
        if (Utils.Pause || Utils.End)
        {
            return;
        }
        levelController.Select();
        TargetGroupController.Instance.UpdateTarget(levelController);
        GlobalValue.Instance.CleanPieces();
        foreach(Piece piece in levelController.splitObjects.GetComponentsInChildren<Piece>())
        {
            GlobalValue.Instance.pieces.Add(piece);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
