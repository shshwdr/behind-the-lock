using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClickBack()
    {
        TargetGroupController.Instance.UpdateAllSeeableTargets();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
