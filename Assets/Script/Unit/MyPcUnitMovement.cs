using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPcUnitMovement : UnitMovementBase
{
    // Start is called before the first frame update
    void Start()
    {
        GameControl.aInstance.aOnMoving += HandleMoving;
        GameControl.aInstance.aOnMoveStart += HandleMoveStart;
        GameControl.aInstance.aOnMoveEnd += HandleMoveEnd;
    }

    // Update is called once per frame
    void OnDestroy()
    {
        GameControl.aInstance.aOnMoving -= HandleMoving;
        GameControl.aInstance.aOnMoveStart -= HandleMoveStart;
        GameControl.aInstance.aOnMoveEnd -= HandleMoveEnd;
    }

    public void DoManualAttack(SkillType InSkillType, Vector3 InAttackPos)
    {
        StartCoroutine(_CoDoManualAttack(InSkillType, InAttackPos));
    }
    private IEnumerator _CoDoManualAttack(SkillType InSkillType, Vector3 InAttackPos)
    {
        Vector3 lAttackDirect = (InAttackPos - transform.position).normalized;
        SkillManager lSkillManager = GetComponent<SkillManager>();
        if (lSkillManager != null)
        {
            ActiveSkillData lActiveSkillData = lSkillManager.GetCurrentSkillData(InSkillType);
            if (lActiveSkillData != null)
            {
                mIsEnableMove = false;

                yield return null;

                if (mAnimator)
                {
                    mAnimator.CrossFade("Attack", 0.1f);
                }
                mRotationTransform.rotation = Quaternion.RotateTowards(mRotationTransform.rotation, Quaternion.LookRotation(lAttackDirect), 360);

                yield return new WaitForSeconds(lActiveSkillData.ActiveSkillLevelData.ActiveTime);

                mIsEnableMove = true;
            }
        }
    }

    private void HandleMoving(Vector3 pDirect)
    {
        if (FSMStageController.aInstance.IsPlayGame() == false)
        {
            return;
        }
        if (mIsEnableMove == false)
        {
            return;
        }

        // 이동
        transform.position += pDirect * mSpeed * Time.deltaTime;
        // 회전
        mRotationTransform.rotation = Quaternion.RotateTowards(mRotationTransform.rotation,
                                                                Quaternion.LookRotation(pDirect),
                                                                mRotationSpeed * Time.deltaTime);
    }

    private void HandleMoveStart()
    {
        if (FSMStageController.aInstance.IsPlayGame() == false)
        {
            return;
        }
        if (mAnimator != null)
        {
            mAnimator.CrossFade("Run", 0.1f);
        }
    }
    private void HandleMoveEnd()
    {
        if (mAnimator != null)
        {
            mAnimator.CrossFade("Idle", 0.1f);
        }
    }
}
