using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFSMStageStateType
{
    None,
    StageStart,
    StageProgress,
    StageLevelup,
    StageBoss,
    StageEnd,
}

public class StageUnitData
{
    public string UnitId;
    public string UnitPath;
    public float UnitSpeed;
    public int Hp;
    public int Power;
    public int Armor;
}

public class StageData
{
    public int StageId;
    public int MaxSpawnCount;
    public string DropId;
    public List<StageUnitData> Units = new List<StageUnitData>();
    public StageUnitData BossUnit;
}
