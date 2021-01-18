﻿using System.Collections;
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
        newLevel.GetComponent<LevelController>().StartSolvingLevel();

        Destroy(originLevel);
    }
    static public void FirstSolvingLevel(LevelController levelController)
    {
        levelController.StartSolvingLevel();
        //camera move

    }
}
