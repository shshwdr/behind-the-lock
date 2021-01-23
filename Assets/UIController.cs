using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using TMPro;

public class UIController : Singleton<UIController>
{

    public GameObject backButton;
    public GameObject hintButton;
    public GameObject theEnd;
    public CinemachineStoryboard storyBoard;
    float pastTime = 0;
    float hintTime = 60;

    public void resetTime()
    {
        pastTime = 0;
        HideHintButton();
    }
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        backButton.SetActive(false);

        hintButton.SetActive(false);
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

    public void ShowHintButton()
    {
        hintButton.SetActive(true);
        Sequence seq = DOTween.Sequence();
        seq.Append(hintButton.transform.DOScale(new Vector3(2, 2, 2), 1));

        seq.Append(hintButton.transform.DOScale(new Vector3(1, 1, 1), 1));

    }
    public void HideHintButton()
    {
        hintButton.SetActive(false);
    }

    public void ToggleAutoRotation()
    {
        if (Utils.Pause || Utils.End)
        {
            return;
        }
        if(Utils.currentSolvingLevel && Utils.currentSolvingLevel.GetComponent<LevelController>())
        {

            Utils.currentSolvingLevel.GetComponent<LevelController>().GiveHint();
        }
    }

    public void Hint()
    {
        if (Utils.Pause || Utils.End)
        {
            return;
        }
        if (Utils.currentSolvingLevel && Utils.currentSolvingLevel.GetComponent<LevelController>())
        {

            Utils.currentSolvingLevel.GetComponent<LevelController>().GiveHint();
        }
    }

    // Update is called once per frame
    void Update()
    {
        pastTime += Time.deltaTime;
        if (pastTime > hintTime && hintButton.active == false&&Utils.currentSolvingLevel)
        {
            ShowHintButton();
        }
    }
}
