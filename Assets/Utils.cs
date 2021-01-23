using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{

    public delegate void FinishLevel();
    public static FinishLevel finishLevel;

    public delegate void StartLevel(string levelName);
    public static StartLevel startLevel;

    public static GameObject currentSolvingLevel; 
    public static bool Pause;

    static public void UpdateSolvingLevel(GameObject currentLevel)
    {
        currentSolvingLevel = currentLevel;
        GlobalValue.Instance.CleanPieces();
        foreach (Piece piece in currentSolvingLevel.GetComponent<LevelController>().splitObjects.GetComponentsInChildren<Piece>())
        {
            GlobalValue.Instance.pieces.Add(piece);
        }
    }


    static public GameObject InitLevel(string levelPath)
    {
        GameObject newLevel = Instantiate(Resources.Load("Levels/"+levelPath) as GameObject);
        string newLevelName = newLevel.GetComponent<LevelController>().levelName;
        return newLevel;
    }

    static public void ReplaceLevel(GameObject originLevel, GameObject newLevelPrefab)
    {
        GlobalValue.Instance.CleanPieces(); 
        GameObject newLevel = Instantiate(newLevelPrefab);
        LevelController newLevelController = newLevel.GetComponent<LevelController>();
        newLevelController.StartSolvingLevel();

        newLevelController.Select();
        TargetGroupController.Instance.UpdateTarget(newLevelController);
        Destroy(originLevel);
    }
    static public void FirstSolvingLevel(LevelController levelController)
    {
        levelController.StartSolvingLevel();
        //camera move

    }
}
