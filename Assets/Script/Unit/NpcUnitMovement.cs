using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcUnitMovement : UnitMovementBase
{
    public bool mIsBoss = false;
    public virtual void Start()
    {
        mNpcUnit = GetComponent<NpcUnit>();
    }

    void OnDestroy()
    {
        mNpcUnit = null;
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (mIsBoss == false)
        {
            MoveToMyPc();
            RotationToMyPc();
        }
    }

    protected void RotationToMyPc()
    {
        Vector3 lTargetDirection = GameDataManager.aInstance.GetMyPcObject().transform.position - transform.position;
        Vector3 lDirect = lTargetDirection.normalized;

        if (lDirect != Vector3.zero)
        {
            mRotationTransform.rotation = Quaternion.RotateTowards(mRotationTransform.rotation,
                                                                    Quaternion.LookRotation(lDirect, Vector3.up),
                                                                    mRotationSpeed * Time.deltaTime);
        }
        mCurrentDirectVec = lDirect;
    }

    protected void MoveToMyPc()
    {
        if (FSMStageController.aInstance.IsPlayGame() == false)
        {
            return;
        }
        if (mNpcUnit.mIsMoveToTarget == false)
        {
            return;
        }
        Vector3 lTargetDirection = GameDataManager.aInstance.GetMyPcObject().transform.position - transform.position;
        Vector3 lDirect = lTargetDirection.normalized;
        transform.position += lDirect * mSpeed * Time.deltaTime;

        mCurrentDirectVec = lDirect;

    }

    public Vector3 GetCurrentDirectVector()
    {
        return mCurrentDirectVec;
    }

    private NpcUnit mNpcUnit = null;
    private Vector3 mCurrentDirectVec;
}

