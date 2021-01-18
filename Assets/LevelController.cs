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

    [HideInInspector]
    public bool isSelected;


    private Vector3 zAxis = new Vector3(0, 0, 1);


    // Start is called before the first frame update
    void Start()
    {
        GlobalValue.Instance.levels.Add(this);
        Utils.finishLevel += LevelFinished;
        if (isAWall)
        {
            UpdateAsAWall();
        }
        if (startLevel)
        {
            //StartSolvingLevel();
            isSelected = true;
        }
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
        splitObjects.SetActive(true);
        Utils.currentSolvingLevel = gameObject;
        isSelected = true;
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

    void LevelFinished()
    {
        if (gameObject != Utils.currentSolvingLevel)
        {
            return;
        }
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
        if (isRotating && targetObject.transform.rotation.z >= 0)
        {
            targetObject.transform.RotateAround(center.position, zAxis, speed);
            //Debug.Log("rotation "+targetObject.transform.rotation.z);

            //Debug.Log("euler " + targetObject.transform.eulerAngles.z);
            if (targetObject.transform.eulerAngles.z >179)
            {
                isRotating = false;
                if (isAWall)
                {
                    isAWall = false;
                    UpdateAsNotAWall();
                }
                else
                {
                    Utils.ReplaceLevel(gameObject, nextLevel);
                }
            }
        }
    }
}
