using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMStageStateEnter : FSMStageBase
{
    public FSMStageStateEnter() : base(EFSMStageStateType.StageStart)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        mCountDown = 3;
        mDurationTime = 0;

        int lCurrentStageId = GameDataManager.aInstance.mStage;
        // �������� ���� �� ����� �������� �����͸� �����ɴϴ�.
        StageData lCurrentStageData = GameDataManager.aInstance.FindStageData(lCurrentStageId);
        if (lCurrentStageData != null)
        {
            // SpawnManager�� ���� �� ��ȯ�Ǵ� ������ ������ ����մϴ�.
            // �Ź� NPC�� ��ȯ�ø��� �ε��ϴ� ���� ���� ���� �����͸� �켱 �ҷ��ͼ� �����صΰ� ��ȯ �� �� �̹� ����Ǿ� �ִ� �����͸� Ȱ���մϴ�.
            // Spawn Manager�� ���ؼ� FSMStageStateProgress ���� ��ȯ�� �����͸� ���� �����մϴ�.

            // Stage �����Ϳ� ����ִ� �ش� ���������� ��ȯ�Ǿ�� �ϴ� ���� �����͸� �����մϴ�.
            foreach (StageUnitData EachNpc in lCurrentStageData.Units)
            {
                SpawnManager.aInstance.AddUnitData(EachNpc.UnitId, EachNpc);
            }
            // Stage �����Ϳ� ����ִ� �ش� ���������� ��ȯ�Ǿ�� �ϴ� ���� �����͸� �����մϴ�.
      //      SpawnManager.aInstance.SetBossUnitData(lCurrentStageData.BossUnit);
        }

        MyPcUnit MyPc = GameDataManager.aInstance.GetMyPcObject().GetComponent<MyPcUnit>();
        if (MyPc != null)
        {
            int ArmorValue = 100;
            int WeaponValue = 100;
            int AddSpeedValue = 0;
            /*StatData MyUnitAllAddStat = InventoryManager.aInstance.GetAllAddStat();
            if (MyUnitAllAddStat != null)
            {
                ArmorValue += MyUnitAllAddStat.Armor;
                WeaponValue += MyUnitAllAddStat.Attack;
                AddSpeedValue += MyUnitAllAddStat.Speed;
            }*/
            MyPc.InitUnit(0, 10000, WeaponValue, ArmorValue);
     //       MyPc.AddUnitSpeed(AddSpeedValue);

            SkillManager MySkillManager = MyPc.GetComponent<SkillManager>();
            MySkillManager.AddSkillData(SkillType.Missile);
            MySkillManager.AddSkillData(SkillType.ManualMissile);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("Stage Stage OnExit");
        mDurationTime = 0;
    }

    public override void OnProgeress(float InDeltaTime)
    {
        base.OnProgeress(InDeltaTime);
        mDurationTime += InDeltaTime;
        if (mDurationTime > 1.0f) 
        {
            if(mCountDown <= 0)
            {
                FSMStageController.aInstance.ChangeState(new FSMStageStateProgress());
            }
            else
            {
                mCountDown--;
                Debug.Log("Count Down - " + mCountDown);
            }
            mDurationTime = 0.0f;
        } 
      
    }

    private float mCountDown = 3;
    private float mDurationTime = 0;

}
