using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIController : MonoBehaviour
{

    public GameObject backButton;

    // Start is called before the first frame update
    void Start()
    {
        //backButton.SetActive(false);
        DOTween.Init();
    }

    public void OnClickBack()
    {
        if (Utils.Pause)
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
