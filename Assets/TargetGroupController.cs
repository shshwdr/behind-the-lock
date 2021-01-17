using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGroupController : Singleton<TargetGroupController>
{
    Cinemachine.CinemachineTargetGroup targetGroup;
    private void Awake()
    {
        targetGroup = GetComponent<Cinemachine.CinemachineTargetGroup>();
    }
    public void clearTargets()
    {
        targetGroup.m_Targets = new Cinemachine.CinemachineTargetGroup.Target[0];
    }
    public void UpdateTargets(List<LevelController> levels)
    {
        clearTargets();
        foreach (LevelController level in levels)
        {
            targetGroup.AddMember(level.center, 1, 2);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
