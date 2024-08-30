using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePoolManager
{
    public static GamePoolManager aInstance
    {
        get
        {
            if (sInstance == null)
            {
                sInstance = new GamePoolManager();
            }
            return sInstance;
        }
    }
    public void Init()
    {
        SkillPool = new Dictionary<string, Queue<SkillBase>>();
        NpcPool = new Dictionary<string, Queue<NpcUnit>>();
        ItemPool = new Dictionary<string, Queue<ItemBase>>();
    }

    public void Clear()
    {
        SkillPool.Clear();
        SkillPool = null;

        NpcPool.Clear();
        NpcPool = null;

        ItemPool.Clear();
        ItemPool = null;
    }

    public void EnqueueItemPool(ItemBase InItem)
    {
        if (ItemPool == null)
        {
            return;
        }
        string ItemId = InItem.mItemData.Id;
        if (ItemPool.ContainsKey(ItemId) == false)
        {
            ItemPool.Add(ItemId, new Queue<ItemBase>());
        }
        ItemPool[ItemId].Enqueue(InItem);
    }

    public ItemBase DequeueItemPool(string InItemId)
    {
        if (ItemPool == null)
        {
            return null;
        }
        if (ItemPool.ContainsKey(InItemId) == false)
        {
            return null;
        }
        if (ItemPool[InItemId].Count == 0)
        {
            return null;
        }

        return ItemPool[InItemId].Dequeue();
    }

    public void EnqueueSkillPool(SkillBase InSkill, int InSkillLevel)
    {
        if (SkillPool == null)
        {
            return;
        }
        string SkillId = GameDataManager.aInstance.GetSkillId(InSkill.mActiveSkillData.Type, InSkillLevel);
        if (SkillPool.ContainsKey(SkillId) == false)
        {
            SkillPool.Add(SkillId, new Queue<SkillBase>());
        }
        SkillPool[SkillId].Enqueue(InSkill);
    }

    public SkillBase DequeueSkillPool(SkillLevelData InSkillLevelData)
    {
        if (SkillPool == null)
        {
            return null;
        }
        string SkillId = GameDataManager.aInstance.GetSkillId(InSkillLevelData);

        if (SkillPool.ContainsKey(SkillId) == false)
        {
            return null;
        }
        if (SkillPool[SkillId].Count == 0)
        {
            return null;
        }
        return SkillPool[SkillId].Dequeue();
    }
    public void ClearSkillPoolObjects()
    {
        foreach (var EachPool in SkillPool)
        {
            foreach (var EachObject in EachPool.Value)
            {
                GameObject.Destroy(EachObject.gameObject);
            }
        }
        SkillPool.Clear();
    }


    public void EnqueueNpcPool(NpcUnit InNpcUnit)
    {
        string lUnitId = InNpcUnit.mStageUnitData.UnitId;
        if (NpcPool == null)
        {
            return;
        }
        if (NpcPool.ContainsKey(lUnitId) == false)
        {
            NpcPool.Add(lUnitId, new Queue<NpcUnit>());
        }
        NpcPool[lUnitId].Enqueue(InNpcUnit);
    }

    public NpcUnit DequeueNpcPool(string InUnitId)
    {
        if (NpcPool == null)
        {
            return null;
        }
        if (NpcPool.ContainsKey(InUnitId) == false)
        {
            return null;
        }
        if (NpcPool[InUnitId].Count == 0)
        {
            return null;
        }
        return NpcPool[InUnitId].Dequeue();
    }

    private static GamePoolManager sInstance = null;

    private Dictionary<string, Queue<SkillBase>> SkillPool = null;
    private Dictionary<string, Queue<NpcUnit>> NpcPool = null;
    private Dictionary<string, Queue<ItemBase>> ItemPool = null;
}
