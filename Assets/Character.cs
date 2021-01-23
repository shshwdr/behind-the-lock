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
    public bool noLevel = false;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        if (!noLevel && levels.Count == 0)
        {
            //Debug.LogError(characterName + " does not have levels");
        }
        if (characterName.Length == 0)
        {
            characterName = name;
        }

        foreach(SpriteMask mask in GetComponentsInChildren<SpriteMask>())
        {
            mask.enabled = false;
            mask.GetComponent<TargetPiece>().enabled = false;
        }
    }
    public IEnumerator WaitFinishStart()
    {
        yield return StartCoroutine(Move(moveStart,moveEnd,moveTime));
    }
    public IEnumerator Move(Vector2 start,Vector2 target,float moveTime)
    {

        SFXPlayer.Instance.playSound("run");
        transform.position = start;
        Tween myTween = transform.DOMove(target, moveTime);
        yield return myTween.WaitForCompletion();
        Destroy(gameObject);
    }

    public void LeftItem(GameObject leftItem)
    {
        GameObject leftItemInstance = Instantiate(leftItem, transform.position,Quaternion.identity, transform.parent);
        leftItemInstance.GetComponent<Character>().levels = levels;
        leftItemInstance.name = "Key";
    }

    public void MoveDiff()
    {

        SFXPlayer.Instance.playSound("run");
        Tween myTween = transform.DOMove((Vector2)transform.position+ moveEnd, moveTime);
        //yield return myTween.WaitForCompletion();
       // Destroy(gameObject);
    }
    private void OnMouseDown()
    {
        if(Utils.Pause || Utils.End)
        {
            return;
        }
        //Debug.Log(characterName + " mouse down");
        foreach (GameObject level in levels)
        {

            LevelController levelController = level.GetComponent<LevelController>();
            if (levelController.isSelected && !levelController.isAWall)
            {
                //Debug.Log(characterName + " check selected");
                Collider2D targetCollider = levelController.targetObject.GetComponent<Collider2D>();
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //worldPosition = new Vector3(worldPosition.x, worldPosition.y, targetCollider.bounds.center.z);
                //Debug.Log(worldPosition + " worldPosition");
                //Debug.Log("boundds " + targetCollider.bounds);
                if (targetCollider.OverlapPoint(worldPosition))
                {
                    //Debug.Log(characterName + " check target");
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
