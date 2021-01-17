using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
public class Character : MonoBehaviour
{
    public Vector2 moveStart;
    public Vector2 moveEnd;
    public float moveTime;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
