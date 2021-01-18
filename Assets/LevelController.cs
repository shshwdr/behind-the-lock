using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    private Vector3 zAxis = new Vector3(0, 0, 1);


    // Start is called before the first frame update
    void Start()
    {
        GlobalValue.Instance.levels.Add(this);
        Utils.finishLevel += LevelFinished;
        if (startLevel)
        {
            //StartSolvingLevel();
        }
    }

    public void Deselect()
    {
        //show selector and hide splitting pieces
        Selector.SetActive(true);
        splitObjects.SetActive(false);
    }

    public void Select()
    {
        Selector.SetActive(false);
        splitObjects.SetActive(true);
    }

    public void StartSolvingLevel()
    {
        Utils.currentSolvingLevel = gameObject;
        if (!solvingBefore)
        {
            string dialogueName = levelName + "Start";
            if (PixelCrushers.DialogueSystem.DialogueManager.ConversationHasValidEntry(dialogueName))
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

    void LevelFinished(string levelName)
    {
        string dialogueName = levelName+"Finished";
        if (PixelCrushers.DialogueSystem.DialogueManager.ConversationHasValidEntry(dialogueName))
        {
            PixelCrushers.DialogueSystem.DialogueManager.StartConversation(dialogueName);
        }
        isRotating = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Utils.currentSolvingLevel != this)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Utils.finishLevel(levelName);
        }
    }

    void FixedUpdate()
    {
        if (isRotating && targetObject.transform.rotation.z >= 0)
        {
            targetObject.transform.RotateAround(center.position, zAxis, speed);
            //Debug.Log("rotation "+targetObject.transform.rotation.z);

            //Debug.Log("euler " + targetObject.transform.eulerAngles.z);
            if (targetObject.transform.eulerAngles.z >179)
            {
                isRotating = false;
                //triger dialog if needed
                //or load another level
                //GameObject newLevel = Utils.InitLevel("Level2");

                //newLevel.GetComponent<LevelController>().StartSolvingLevel();
                Utils.ReplaceLevel(gameObject, nextLevel);
                //Destroy(gameObject); 
            }
        }
    }
}
