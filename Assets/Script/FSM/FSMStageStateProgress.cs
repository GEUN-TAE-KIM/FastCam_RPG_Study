using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMStageStateProgress : FSMStageBase
{
    public FSMStageStateProgress() : base(EFSMStageStateType.StageProgress)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Stage Stage Progeree");
    }

    public override void OnExit()
    {
        base.OnExit();  
    }

    public override void OnProgeress(float InDeltaTime)
    {
        base.OnProgeress(InDeltaTime);
    }

}
