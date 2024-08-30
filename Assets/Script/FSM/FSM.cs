using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    public FSM(FSMStageBase InFSMState)
    {
        mCurrentState = InFSMState;
        if(mCurrentState == null)
        {
            mCurrentState.OnEnter();
        }
    }

    public void ChangeState(FSMStageBase InFSMState)
    {
        if (mCurrentState == InFSMState) 
        {
            return;
        }

        if(mCurrentState != null)
        {
            mCurrentState.OnExit();
        }

        mCurrentState = InFSMState;
        if (mCurrentState != null) 
        {
            mCurrentState.OnEnter();
        }

    }

    public void OnUpdateState(float InDeltaTime)
    {
        if (mCurrentState != null) 
        {
            mCurrentState.OnProgeress(InDeltaTime);
        }
    }


    public FSMStageBase mCurrentState { get; private set; }

}
