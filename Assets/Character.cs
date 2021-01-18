using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
public class Character : MonoBehaviour
{
    public Vector2 moveStart;
    public Vector2 moveEnd;
    public float moveTime;

    public string characterName;

    public List<GameObject> levels;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        if (levels.Count == 0)
        {
            Debug.LogError(characterName + " does not have levels");
        }
    }
    public IEnumerator WaitFinishStart()
    {
        yield return StartCoroutine(Move(moveStart,moveEnd,moveTime));
    }
    public IEnumerator Move(Vector2 start,Vector2 target,float moveTime)
    {
        transform.position = start;
        Tween myTween = transform.DOMove(target, moveTime);
        yield return myTween.WaitForCompletion();
    }
    private void OnMouseDown()
    {
        Debug.Log(characterName + " mouse down");
        foreach (GameObject level in levels)
        {

            LevelController levelController = level.GetComponent<LevelController>();
            if (levelController.isSelected && !levelController.isAWall)
            {
                Debug.Log(characterName + " check selected");
                Collider2D targetCollider = levelController.targetObject.GetComponent<Collider2D>();
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //worldPosition = new Vector3(worldPosition.x, worldPosition.y, targetCollider.bounds.center.z);
                Debug.Log(worldPosition + " worldPosition");
                Debug.Log("boundds " + targetCollider.bounds);
                if (targetCollider.OverlapPoint(worldPosition))
                {
                    Debug.Log(characterName + " check target");
                    string dialogueName = characterName + "Click";
                    if (PixelCrushers.DialogueSystem.DialogueManager.ConversationHasValidEntry(dialogueName))
                    {
                        PixelCrushers.DialogueSystem.DialogueManager.StartConversation(dialogueName);

                        PixelCrushers.DialogueSystem.DialogueLua.SetVariable(dialogueName, true);
                    }
                    break;
                }
            }
        }

        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
