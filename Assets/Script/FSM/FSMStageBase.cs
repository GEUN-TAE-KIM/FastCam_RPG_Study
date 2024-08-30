using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMStageBase
{
    public EFSMStageStateType mCurrentStageType { get; protected set; }

    public FSMStageBase(EFSMStageStateType InType)
    {
        mCurrentStageType = InType;
    }

    public virtual void OnEnter()
    {

    }

    public virtual void OnExit()
    {

    }

    public virtual void OnProgeress(float InDeltaTime)
    {

    }
}
