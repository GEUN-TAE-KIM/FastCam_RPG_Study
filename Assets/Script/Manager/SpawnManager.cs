using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager
{
    public static SpawnManager aInstance
    {
        get
        {
            if (sInstance == null)
            {
                sInstance = new SpawnManager();
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

    public void AddUnitData(string InUnitStringId, StageUnitData InData)
    {
        if (Units.ContainsKey(InUnitStringId))
        {
            return;
        }
        NpcUnit NpcUnitObject = Resources.Load<NpcUnit>(InData.UnitPath);
        NpcUnitObject.mStageUnitData = new StageUnitData();
        NpcUnitObject.mStageUnitData = InData;
        NpcUnitObject.SetSpeed(InData.UnitSpeed);

        if (NpcUnitObject != null)
        {
            Units.Add(InUnitStringId, NpcUnitObject);
            UnitKeyByIndex.Add(Units.Count, InUnitStringId);
        }
    }


    private GameObject mSpawnPointObject;
    private Dictionary<string, NpcUnit> Units;
    private Dictionary<int, string> UnitKeyByIndex;
    private int NpcIndex = 0;
    private int UnitId = 0;

    private NpcUnit mBossUnitObject;

    private static SpawnManager sInstance = null;

    private List<NpcUnit> mSpawnedUnits;

}
