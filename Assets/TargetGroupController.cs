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
    public void UpdateTarget(LevelController level)
    {
        foreach (LevelController l in GlobalValue.Instance.levels)
        {
            if (l!=null&&l.isSeeable)
            {
                l.HideSelector();
            }
        }
        UpdateTargets(new List<LevelController>() { level });
    }
    public void UpdateTargets(List<LevelController> levels)
    {
        clearTargets();
        foreach (LevelController level in levels)
        {
            targetGroup.AddMember(level.center, 1, 2.5f);
        }
    }

    public void UpdateAllSeeableTargets()
    {
        List<LevelController> levels = new List<LevelController>();
        foreach (LevelController level in GlobalValue.Instance.levels)
        {
            if (level!=null&&level.isSeeable)
            {
                levels.Add(level);
                level.Deselect();
            }
        }
        GlobalValue.Instance.UnselectCurrentPiece();
        UpdateTargets(levels);
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
