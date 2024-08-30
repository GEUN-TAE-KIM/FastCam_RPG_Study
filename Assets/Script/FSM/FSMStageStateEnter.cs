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
        // 스테이지 입장 시 적용된 스테이지 데이터를 가져옵니다.
        StageData lCurrentStageData = GameDataManager.aInstance.FindStageData(lCurrentStageId);
        if (lCurrentStageData != null)
        {
            // SpawnManager는 게임 중 소환되는 유닛의 관리를 담당합니다.
            // 매번 NPC의 소환시마다 로드하는 것을 막기 위해 데이터만 우선 불러와서 저장해두고 소환 될 때 이미 저장되어 있는 데이터를 활용합니다.
            // Spawn Manager를 통해서 FSMStageStateProgress 에서 소환할 데이터를 먼저 저장합니다.

            // Stage 데이터에 들어있는 해당 스테이지에 소환되어야 하는 유닛 데이터를 셋팅합니다.
            foreach (StageUnitData EachNpc in lCurrentStageData.Units)
            {
                SpawnManager.aInstance.AddUnitData(EachNpc.UnitId, EachNpc);
            }
            // Stage 데이터에 들어있는 해당 스테이지에 소환되어야 하는 보스 데이터를 셋팅합니다.
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
