using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMStageController
{
    public static FSMStageController aInstance
    {
        get
        {
            if (sInstance == null)
            {
                sInstance = new FSMStageController();
            }
            return sInstance;
        }
    }

    public void Init()
    {

    }

    public void Clear()
    {

    }

    public void EnterStage() 
    {
        mStageFMS = new FSM(new FSMStageStateEnter());
    }

    public void ChangeState(FSMStageBase InFSMStage)
    {
        if (mStageFMS != null) 
        {
            mStageFMS.ChangeState(InFSMStage);
        }
    }


    public void OnUpdate(float InDeltaTime)
    {
        if (mStageFMS != null) 
        {
            mStageFMS.OnUpdateState(InDeltaTime);
        }
    }

    public bool IsPlayGame()
    {
        if(mStageFMS == null)
        {
            return false;
        }
        return mStageFMS.mCurrentState.mCurrentStageType == EFSMStageStateType.StageProgress 
            || mStageFMS.mCurrentState.mCurrentStageType == EFSMStageStateType.StageBoss;
    }

    private FSM mStageFMS = null;
    private static FSMStageController sInstance = null;

}
