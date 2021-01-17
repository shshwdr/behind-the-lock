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


    private Vector3 zAxis = new Vector3(0, 0, 1);

    // Start is called before the first frame update
    void Start()
    {

        Utils.finishLevel += LevelFinished;
    }

    void LevelFinished(string levelName)
    {
        string dialogueName = levelName+"Finished";
        if (PixelCrushers.DialogueSystem.DialogueManager.ConversationHasValidEntry(dialogueName))
        {
           // PixelCrushers.DialogueSystem.DialogueManager.StartConversation(dialogueName);
        }
        isRotating = true;
    }

    // Update is called once per frame
    void Update()
    {
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
                GameObject newLevel  = Instantiate(Resources.Load("Levels/Level2") as GameObject);
                Destroy(gameObject);
                string newLevelName = newLevel.GetComponent<LevelController>().levelName;
                //Utils.startLevel(newLevelName);
                string dialogueName = newLevelName+"Start";
                if (PixelCrushers.DialogueSystem.DialogueManager.ConversationHasValidEntry(dialogueName))
                {
                    PixelCrushers.DialogueSystem.DialogueManager.StartConversation(dialogueName);
                }
            }
        }
    }
}
