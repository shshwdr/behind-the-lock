using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using TMPro;

public class UIController : MonoBehaviour
{

    public GameObject backButton;
    public GameObject theEnd;
    public CinemachineStoryboard storyBoard;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        backButton.SetActive(false);
    }

    public void TheEnd()
    {
        Utils.End = true;
        theEnd.SetActive(true);
        //theEnd.color = new Color(theEnd.color.r, theEnd.color.g, theEnd.color.b,0);
        //storyBoard.m_Alpha = 1;
        //DOTween.To(() => storyBoard.m_Alpha, x => storyBoard.m_Alpha = x, 1, 1);

       // DOTween.To(() => theEnd.color, x => theEnd.color = x, new Color(theEnd.color.r, theEnd.color.g, theEnd.color.b, 0), 255);
    }

    public void OnClickBack()
    {
        if (Utils.Pause || Utils.End)
        {
            return;
        }
        TargetGroupController.Instance.UpdateAllSeeableTargets();
    }

    public void ShowBackButton()
    {
        backButton.SetActive(true);
        Sequence seq = DOTween.Sequence();
        seq.Append(backButton.transform.DOScale(new Vector3(2,2,2), 1));

        seq.Append(backButton.transform.DOScale(new Vector3(1, 1, 1), 1));
        
    }

    public void ToggleAutoRotation()
    {
        if (Utils.Pause)
        {
            return;
        }
    }

    public void Hint()
    {
        if (Utils.Pause)
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
