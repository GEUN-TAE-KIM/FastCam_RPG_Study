using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    None,
    Exp,
    Gold,
    Weapon,
    Armor,
}

public class StatEnchantData
{
    public int Level = 0;
    public int Attack = 0;
    public int Armor = 0;
    public int Speed = 0;
    public void Clear()
    {
        Level = 0;
        Attack = 0;
        Armor = 0;
        Speed = 0;
    }
}

public class StatData
{
    public string Id = "";
    public int Attack = 0;
    public int Armor = 0;
    public int Speed = 0;
    public Dictionary<int, StatEnchantData> EnchantLevelData;
    public void Clear()
    {
        if (EnchantLevelData != null)
        {
            EnchantLevelData.Clear();
            EnchantLevelData = null;
        }
    }
}

public class ItemData
{
    public string Id = "";
    public string Path = "";
    public string IconPath = "";
    public bool IsStackable = false;
    public EItemType Type = EItemType.None;
    public int Value = 0;
    public int SellPrice = 0;
    public StatData ItemStatData = null;

    public bool IsValid()
    {
        return Id != string.Empty;
    }
    public string ShowItemDataLog()
    {
        return ("ItemId : " + Id + " / Type : " + Type.ToString());
    }
}

public class InventoryItemData
{
    public int InventoryId;
    public ItemData ItemData;
    public bool IsEquippied = false;
    public int Count;
    public int EnchantLevel = 0;
    public StatEnchantData GetCurrentEnchantStat()
    {
        if (ItemData.ItemStatData.EnchantLevelData.ContainsKey(EnchantLevel) == false)
        {
            return null;
        }
        return ItemData.ItemStatData.EnchantLevelData[EnchantLevel];
    }
}

public class DropDataInfo
{
    public string ItemId;
    public int DropRatio;

    public int DropSelectMinValue;
    public int DropSelectMaxValue;
}

public class DropData
{
    public List<DropDataInfo> DropList;
    public List<DropDataInfo> BossDropList;
    public void PostLoad()
    {
        foreach (var EachDrop in DropList)
        {
            EachDrop.DropSelectMinValue = mDropMaxValue;
            mDropMaxValue += EachDrop.DropRatio;
            EachDrop.DropSelectMaxValue = mDropMaxValue - 1;
        }
        foreach (var EachBossDrop in BossDropList)
        {
            EachBossDrop.DropSelectMinValue = mBossDropMaxValue;
            mBossDropMaxValue += EachBossDrop.DropRatio;
            EachBossDrop.DropSelectMaxValue = mBossDropMaxValue - 1;
        }
    }
    public DropDataInfo RandomPickDropInfo(bool InIsBoss)
    {
        List<DropDataInfo> CurrentDropList = InIsBoss ? BossDropList : DropList;
        int CurrentDropMaxValue = InIsBoss ? mBossDropMaxValue : mDropMaxValue;

        int lRandomValue = Random.Range(0, CurrentDropMaxValue);
        foreach (var EachDrop in CurrentDropList)
        {
            if (lRandomValue >= EachDrop.DropSelectMinValue && lRandomValue <= EachDrop.DropSelectMaxValue)
            {
                return EachDrop;
            }
        }
        return null;
    }

    private int mDropMaxValue = 0;
    private int mBossDropMaxValue = 0;
}
