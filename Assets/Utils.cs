using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{

    public delegate void FinishLevel(string levelName);
    public static FinishLevel finishLevel;

    public delegate void StartLevel(string levelName);
    public static StartLevel startLevel;

    public static int currentLevel; 
    public static bool Pause;
}
