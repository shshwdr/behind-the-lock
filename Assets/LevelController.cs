﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelController : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject splitObjects;
    public Transform center;
    public string levelName;
    public float speed;
    bool isRotating;
    bool solvingBefore;
    public GameObject nextLevel;
    public bool isAWall;
    public bool isSeeable;
    public GameObject Selector;
    public bool startLevel;
    public bool isSolved;
    public List<GameObject> requiredLevelList;
    public Dictionary<GameObject,bool> requiredLevels;
    public int hintPiece = 2;

    [HideInInspector]
    public bool isSelected;


    private Vector3 zAxis = new Vector3(0, 0, 1);

    Vector3 originPosition;

    

    // Start is called before the first frame update
    void Start()
    {
        if (levelName == null ||levelName.Length == 0)
        {
            levelName = name;
        }
        if (requiredLevelList.Count > 0)
        {
            isSeeable = false;
            targetObject.SetActive(false);
            splitObjects.SetActive(false);
            Selector.SetActive(false);
            requiredLevels = new Dictionary<GameObject, bool>();
            foreach (GameObject go in requiredLevelList)
            {
                requiredLevels[go] = false;
            }
        }
        originPosition = targetObject.transform.position;
        foreach (Transform child in targetObject.transform)
        {
            child.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        }

        GlobalValue.Instance.levels.Add(this);
        Utils.finishLevel += LevelFinished;
        if (isAWall)
        {
            UpdateAsAWall();
        }
        else
        {
            UpdateAsNotAWall();
        }
        if (startLevel)
        {
            StartSolvingLevel();

            splitObjects.SetActive(true);
           // TargetGroupController.Instance.UpdateTarget(this);
            isSelected = true;
        }
        HideSelector();
    }

    public void Deselect()
    {
        //show selector and hide splitting pieces
        Selector.SetActive(true);
        splitObjects.SetActive(false);
        Utils.currentSolvingLevel = null;
        isSelected = false;
    }

    public void Select()
    {
        Selector.SetActive(false);
        if (!isSolved)
        {
            splitObjects.SetActive(true);
        }
        StartSolvingLevel();

        isSelected = true;
    }

    public void HideSelector()
    {
        Selector.SetActive(false);

    }
    public void ShowSelector()
    {
        Selector.SetActive(true);
    }
    public void GiveHint()
    {
        string hintString = "Piece" + hintPiece;
        foreach (Piece piece in splitObjects.GetComponentsInChildren<Piece>())
        {
            if (piece.gameObject. name == hintString)
            {
                foreach (Transform piece2 in targetObject.GetComponentsInChildren<Transform>())
                {
                    if (piece2.gameObject.name == hintString)
                    {
                        piece.transform.position = piece2.transform.position;
                        piece.transform.rotation = piece2.transform.rotation;

                    }
                }
            }
            else
            {
                piece.backOriginPosition();
            }
        }
    }
    public void StartSolvingLevel()
    {

        Utils.UpdateSolvingLevel(gameObject);
        if (!solvingBefore)
        {
            string dialogueName = levelName + "Start";
            if (PixelCrushers.DialogueSystem.DialogueManager.HasConversation(dialogueName))
            {
                PixelCrushers.DialogueSystem.DialogueManager.StartConversation(dialogueName);

                PixelCrushers.DialogueSystem.DialogueLua.SetVariable(dialogueName, true);
            }
            solvingBefore = true;
        }
    }
    public void EndSolvingLevel()
    {
        //splitObjects.SetActive(false);
        Destroy(gameObject);
    }
    IEnumerator LevelFinishedYield()
    {
        yield return new WaitForSeconds(0.2f);
        string dialogueName = levelName + "Finished";
        if (PixelCrushers.DialogueSystem.DialogueManager.HasConversation(dialogueName))
        {
            PixelCrushers.DialogueSystem.DialogueManager.StartConversation(dialogueName);
        }

        isRotating = true;
    }
    void LevelFinished()
    {
        if (gameObject != Utils.currentSolvingLevel)
        {
            if (!isSeeable)
            {
                bool allUnlocked = true;
                List<GameObject> test = requiredLevels.Keys.ToList();
                foreach (GameObject requiredLevel in test)
                {
                    if (requiredLevel == Utils.currentSolvingLevel)
                    {
                        requiredLevels[requiredLevel] = true;
                    }
                    else
                    {
                        if (!requiredLevels[requiredLevel])
                        {
                            allUnlocked = false;
                        }
                    }
                }
                if (allUnlocked)
                {
                    isSeeable = true;
                    targetObject.SetActive(true);
                    //splitObjects.SetActive(true);
                    Selector.SetActive(true);
                }

            }
            return;
        }
        splitObjects.transform.parent = targetObject.transform;
        Utils.finishLevel -= LevelFinished;
        SFXPlayer.Instance.playSound("lock");
        StartCoroutine(LevelFinishedYield());
    }

    // Update is called once per frame
    void Update()
    {
        if (Utils.currentSolvingLevel != gameObject)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Utils.finishLevel();
        }
    }

    void UpdateAsAWall()
    {
        foreach (Transform child in targetObject.transform)
        {
            child.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            child.gameObject.GetComponent<SpriteMask>().enabled = false;
        }
    }
    void UpdateAsNotAWall()
    {
        foreach (Transform child in targetObject.transform)
        {
            child.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            child.gameObject.GetComponent<SpriteMask>().enabled = true;
        }
    }
    void FixedUpdate()
    {
        if (isRotating)
        {
            targetObject.transform.RotateAround(center.position, zAxis, speed);
            //Debug.Log("rotation "+targetObject.transform.rotation.z);

            //Debug.Log("euler " + targetObject.transform.eulerAngles.z);
            if (targetObject.transform.eulerAngles.z >360-5)
            {
                isRotating = false;
                splitObjects.transform.parent = targetObject.transform.parent;
                targetObject.transform.eulerAngles = Vector3.zero;
                targetObject.transform.position = originPosition;
                if (isAWall)
                {
                    isAWall = false;
                    UpdateAsNotAWall();
                    splitObjects.SetActive(false);
                    isSolved = true;
                }
                else
                {
                    Utils.ReplaceLevel(gameObject, nextLevel);
                }
            }
        }
    }
}
