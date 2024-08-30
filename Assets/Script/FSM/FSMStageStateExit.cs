using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMStageStateExit : FSMStageBase
{
    public FSMStageStateExit() : base(EFSMStageStateType.StageEnd)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
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
