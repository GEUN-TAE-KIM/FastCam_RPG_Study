using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static GameControl;

public class SkillManager : MonoBehaviour
{
    void Awake()
    {
        LevelOfSkills = new Dictionary<SkillType, int>();
        CurrentActiveSkillDatas = new Dictionary<SkillType, ActiveSkillData>();
        CurrentManualSkillDatas = new List<ActiveSkillData>();
    }

    void OnDestroy()
    {
        LevelOfSkills.Clear();
        LevelOfSkills = null;

        CurrentActiveSkillDatas.Clear();
        CurrentActiveSkillDatas = null;

        CurrentManualSkillDatas.Clear();
        CurrentManualSkillDatas = null;
    }
    // Update is called once per frame
    void Update()
    {
        if (FSMStageController.aInstance.IsPlayGame() == false)
        {
            return;
        }
        foreach (var EachActiveSkill in CurrentActiveSkillDatas)
        {
            EachActiveSkill.Value.CurrentCoolTime += Time.deltaTime;
            if (EachActiveSkill.Value.CurrentCoolTime >= EachActiveSkill.Value.ActiveSkillLevelData.CoolTime)
            {
                if (EachActiveSkill.Value.ActiveType == SkillActiveType.Auto)
                {
                    FireSkill(GetComponent<UnitBase>(), EachActiveSkill.Value);
                }
            }
        }
        CurrentCooltime += Time.deltaTime;
    }

    public void UseInputSkill(int InSkillIndex)
    {
        int FireSkillIndex = InSkillIndex + 1;

        if (FSMStageController.aInstance.IsPlayGame() == false)
        {
            return;
        }
        if (CurrentManualSkillDatas.Count - 1 < FireSkillIndex)
        {
            return;
        }
        if (CurrentManualSkillDatas[FireSkillIndex].CurrentCoolTime < CurrentManualSkillDatas[FireSkillIndex].ActiveSkillLevelData.CoolTime)
        {
            return;
        }
        _FireSkillWithMousePos(CurrentManualSkillDatas[FireSkillIndex]);
    }

    public void UseSkillFromIndex(int InSkillIndex, Vector3 InFirePosition, bool InIsIgnoreCooltime)
    {
        if (FSMStageController.aInstance.IsPlayGame() == false)
        {
            return;
        }
        if (CurrentManualSkillDatas.Count - 1 < InSkillIndex)
        {
            return;
        }
        if (InIsIgnoreCooltime == false
            && CurrentManualSkillDatas[InSkillIndex].CurrentCoolTime < CurrentManualSkillDatas[InSkillIndex].ActiveSkillLevelData.CoolTime)
        {
            return;
        }
        FireSkillWithPos(CurrentManualSkillDatas[InSkillIndex], InFirePosition);
    }

    public void FireMouseInputSkill(int InIndex, Vector3 InMousePos)
    {
        if (FSMStageController.aInstance.IsPlayGame() == false)
        {
            return;
        }

        if (CurrentManualSkillDatas.Count - 1 < InIndex)
        {
            return;
        }
        if (CurrentManualSkillDatas[InIndex].CurrentCoolTime < CurrentManualSkillDatas[InIndex].ActiveSkillLevelData.CoolTime)
        {
            return;
        }
        _FireSkillWithMousePos(CurrentManualSkillDatas[InIndex]);
    }

    private void _FireSkillWithMousePos(ActiveSkillData InSkillData)
    {
        RaycastHit lHit;
        Ray lRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layermask = 1 << LayerMask.NameToLayer("Terrain");
        if (Physics.Raycast(lRay, out lHit, 1000, layermask))
        {
            MyPcUnitMovement UnitMovement = GetComponent<MyPcUnitMovement>();
            if (UnitMovement != null)
            {
                UnitMovement.DoManualAttack(InSkillData.Type, lHit.point);
            }
            InSkillData.FirePosition = lHit.point;
            FireSkill(GetComponent<UnitBase>(), InSkillData);
        }
    }

    public void FireSkillWithPos(ActiveSkillData InSkillData, Vector3 InFirePosition)
    {
        InSkillData.FirePosition = InFirePosition;
        FireSkill(GetComponent<UnitBase>(), InSkillData);
    }

    public void AddSkillData(SkillType InSkillType, bool IsInMySkill = true)
    {
        SkillData lSkillData = GameDataManager.aInstance.FindSkillData(InSkillType);
        if (lSkillData == null)
        {
            return;
        }
        if (LevelOfSkills.ContainsKey(InSkillType))
        {
            LevelOfSkills[InSkillType]++;
        }
        else
        {
            LevelOfSkills.Add(InSkillType, 1);
        }

        int lCurrentSkillLevel = LevelOfSkills[InSkillType];
        SkillLevelData lCurrentSkillLevelData = GameDataManager.aInstance.FindSkillLevelData(InSkillType, lCurrentSkillLevel);
        if (lCurrentSkillLevelData == null)
        {
            return;
        }

        if (CurrentActiveSkillDatas.ContainsKey(InSkillType) == false)
        {
            ActiveSkillData NewSkillData = new ActiveSkillData();
            NewSkillData.Type = InSkillType;
            NewSkillData.ActiveType = lSkillData.ActiveType;
            NewSkillData.SkillIconPath = lSkillData.SkillIconPath;
            NewSkillData.CurrentCoolTime = 0.0f;
            NewSkillData.ActiveSkillLevelData = lCurrentSkillLevelData;

            CurrentActiveSkillDatas.Add(InSkillType, NewSkillData);
        }
        else
        {
            CurrentActiveSkillDatas[InSkillType].CurrentCoolTime = 0.0f;
            CurrentActiveSkillDatas[InSkillType].ActiveSkillLevelData = lCurrentSkillLevelData;
        }

        switch (lSkillData.ActiveType)
        {
            case SkillActiveType.Manual:
                {
                    int lFindIndex = -1;
                    int lCurrentIndex = 0;
                    foreach (var EachSkill in CurrentManualSkillDatas)
                    {
                        if (EachSkill.Type == InSkillType)
                        {
                            lFindIndex = lCurrentIndex;
                        }
                        lCurrentIndex++;
                    }

                    if (lFindIndex >= 0)
                    {
                        CurrentManualSkillDatas[lFindIndex] = CurrentActiveSkillDatas[InSkillType];
                    }
                    else
                    {
                        CurrentManualSkillDatas.Add(CurrentActiveSkillDatas[InSkillType]);
                    }
                }
                if (IsInMySkill)
                {
                    UIManager.aInstance.SetManualSkills(CurrentManualSkillDatas);
                }
                break;
        }
    }

    public ActiveSkillData GetCurrentSkillData(SkillType InSkillType)
    {
        if (CurrentActiveSkillDatas.ContainsKey(InSkillType) == false)
        {
            return null;
        }
        return CurrentActiveSkillDatas[InSkillType];
    }

    public void FireSkill(UnitBase InSrcUnit, ActiveSkillData InSkillData)
    {
        switch (InSkillData.Type)
        {
            case SkillType.Missile:
                {
                    for (int fireAngle = 0; fireAngle < 360; fireAngle += 20)
                    {
                        Vector3 ShotDirection = new Vector3(Mathf.Cos(fireAngle * Mathf.Deg2Rad),
                                                            0,
                                                            Mathf.Sin(fireAngle * Mathf.Deg2Rad));
                        Vector3 StartPos = new Vector3(transform.position.x, 1, transform.position.z);

                        FireSkillObject(InSrcUnit, InSkillData, StartPos, ShotDirection);
                    }
                }
                break;
            case SkillType.ManualMissile:
                {
                    Vector3 ShotDirection = (InSkillData.FirePosition - transform.position).normalized;
                    Vector3 StartPos = new Vector3(transform.position.x, 1, transform.position.z);
                    FireSkillObject(InSrcUnit, InSkillData, StartPos, ShotDirection);
                }
                break;
        }
        InSkillData.CurrentCoolTime = 0;
    }

    public void FireSkillObject(UnitBase InSrcUnit, ActiveSkillData InSkillData, Vector3 InStartPos, Vector3 InSkillDir)
    {
        SkillBase SkillObject = GamePoolManager.aInstance.DequeueSkillPool(InSkillData.ActiveSkillLevelData);
        if (SkillObject == null)
        {
            SkillBase NewSkillObjectPrefab = GameDataManager.aInstance.GetSkillObjectPrefab(InSkillData.Type, InSkillData.ActiveSkillLevelData.Level);
            if (NewSkillObjectPrefab == null)
            {
                return;
            }
            SkillObject = GameObject.Instantiate(NewSkillObjectPrefab, GameDataManager.aInstance.GetSkillRootTransform());
            if (SkillObject == null)
            {
                return;
            }
        }
        SkillObject.gameObject.SetActive(true);
        SkillObject.mSkillType = InSkillData.Type;
        SkillObject.FireSkill(InSrcUnit, InSkillData, InStartPos, InSkillDir);
    }
    public List<SkillLevelData> FindLevelupSkillData()
    {
        List<SkillLevelData> lResult = new List<SkillLevelData>();
        Dictionary<SkillType, SkillData> AllSkillDatas = GameDataManager.aInstance.GetAllSkillDatas();
        List<SkillData> AllSkillDataList = AllSkillDatas.Values.ToList();
        int[] RandomIndexes;
        RandomNotOverlap(0, AllSkillDataList.Count, out RandomIndexes);

        for (int i = 0; i < 3; i++)
        {
            int SelectedIndex = RandomIndexes[i];
            SkillType SelectedSkillType = AllSkillDataList[SelectedIndex].Type;
            if (CurrentActiveSkillDatas.ContainsKey(SelectedSkillType))
            {
                ActiveSkillData lCurrentActiveSkillData = CurrentActiveSkillDatas[SelectedSkillType];
                SkillLevelData lNextLevelData = GameDataManager.aInstance.FindSkillLevelData(
                                                                SelectedSkillType,
                                                                lCurrentActiveSkillData.ActiveSkillLevelData.Level + 1);
                if (lNextLevelData != null)
                {
                    lResult.Add(lNextLevelData);
                }
            }
            else
            {
                SkillLevelData NewLevelData = GameDataManager.aInstance.FindSkillLevelData(SelectedSkillType, 1);
                if (NewLevelData != null)
                {
                    lResult.Add(NewLevelData);
                }
            }
        }
        if (lResult.Count < 3)
        {
            for (int i = lResult.Count - 1; i < 3; i++)
            {
                SkillLevelData MoneyUpData = new SkillLevelData();
                MoneyUpData.Type = SkillType.MoneyUp;
                lResult.Add(MoneyUpData);
            }
        }
        return lResult;
    }

    private void RandomNotOverlap(int min, int max, out int[] outRandomIndex)
    {
        int size = Mathf.Abs(max - min);
        outRandomIndex = new int[size];

        for (int i = 0; i < size; i++)
        {
            outRandomIndex[i] = i + min;
        }

        for (int i = 0; i < size; i++)
        {
            int randomindex = (int)UnityEngine.Random.Range(min, max);
            int tempValue = outRandomIndex[i];
            if (randomindex == i)
            {
                continue;
            }
            outRandomIndex[i] = outRandomIndex[randomindex];
            outRandomIndex[randomindex] = tempValue;
        }
    }


    public float CurrentCooltime = 0.0f;

    // 스킬 타입별 레벨 정보
    Dictionary<SkillType, int> LevelOfSkills;

    // 현재 사용중인 스킬 정보
    Dictionary<SkillType, ActiveSkillData> CurrentActiveSkillDatas;
    List<ActiveSkillData> CurrentManualSkillDatas;
}
