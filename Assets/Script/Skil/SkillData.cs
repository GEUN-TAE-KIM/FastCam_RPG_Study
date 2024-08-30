using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Missile,
    ManualMissile,
    MoveSpeedUp,
    MagicRain,
    MoneyUp,
}
public enum SkillActiveType
{
    Manual,
    Auto,
    Buff,
}

public class SkillLevelData
{
    public SkillType Type;
    public string Path;
    public int Level;
    public int Power;
    public int Size;
    public float Speed;
    public float ActiveTime;
    public float CoolTime;
}

public class SkillData
{
    public SkillType Type;
    public SkillActiveType ActiveType;
    public string SkillIconPath;
    public Dictionary<int, SkillLevelData> LevelDatas;
}

public class ActiveSkillData
{
    public SkillType Type;
    public SkillActiveType ActiveType;
    public string SkillIconPath;
    public float CurrentCoolTime;
    public Vector3 FirePosition;
    public SkillLevelData ActiveSkillLevelData;
}