using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Win32.SafeHandles;

public class GameDataManager
{
    public int mStage { get; private set; }
    public int mLiveNpcUnitCount { get; set; } = 0;
    public static GameDataManager aInstance
    {
        get
        {
            if (sInstance == null)
            {
                sInstance = new GameDataManager();
            }
            return sInstance;
        }
    }
    public GameObject GetMyPcObject()
    {
        return mMyPc;
    }
    public Transform GetSpawnRootTransform()
    {
        return mSpawnRoot;
    }
    public Transform GetItemRootTransform()
    {
        return mItemRoot;
    }
    public Transform GetSkillRootTransform()
    {
        return mSkillRoot;
    }

    public void Init()
    {
        ClearGameTime();
    }
    public void Clear()
    {
        mStage = 0;
        mMyPc = null;
        mSpawnRoot = null;
        mItemRoot = null;
        mSkillRoot = null;

        StageDatas.Clear();
        StageDatas = null;

        SkillDatas.Clear();
        SkillDatas = null;

        SkillResources.Clear();
        SkillResources = null;

        StatDatas.Clear();
        StatDatas = null;

        ItemDatas.Clear();
        ItemDatas = null;

        ItemResources.Clear();
        ItemResources = null;

        DropDatas.Clear();
        DropDatas = null;

        ShopDatas.Clear();
        ShopDatas = null;

        mLiveNpcUnitCount = 0;

        ClearGameTime();
    }

    public void LoadAll()
    {
        if (mIsInit == true)
        {
            return;
        }
        LoadStageData();
        LoadSkillData();
        LoadStatData();
        LoadItemData();
        LoadDropData();
        LoadShopData();

   //     InventoryManager.aInstance.LoadInventoryData();

        mIsInit = true;
    }

    public Dictionary<string, ShopData> GetShopDatas()
    {
        return ShopDatas;
    }

    public ShopData GetShopData(string InId)
    {
        if (ShopDatas.ContainsKey(InId) == false)
        {
            return null;
        }
        return ShopDatas[InId];
    }

    protected void LoadShopData()
    {
        ShopDatas = new Dictionary<string, ShopData>();
        ShopDatas.Clear();
        TextAsset JsonTextAsset = Resources.Load<TextAsset>("Data/Shop");
        string lJson = JsonTextAsset.text;
        JObject lDataObject = JObject.Parse(lJson);
        JToken lToken = lDataObject["ShopDatas"];
        JArray lArray = lToken.Value<JArray>();
        foreach (JObject EachObject in lArray)
        {
            ShopData NewShopData = new ShopData();
            NewShopData.Id = EachObject.Value<string>("Id");
            NewShopData.ItemId = EachObject.Value<string>("Item");
            NewShopData.Price = EachObject.Value<int>("Price");
            ShopDatas.Add(NewShopData.Id, NewShopData);
        }
    }
    protected void LoadDropData()
    {
        DropDatas = new Dictionary<string, DropData>();
        DropDatas.Clear();

        TextAsset JsonTextAsset = Resources.Load<TextAsset>("Data/DropDatas");
        string lJson = JsonTextAsset.text;
        JObject lDataObject = JObject.Parse(lJson);
        JToken lToken = lDataObject["DropData"];
        JArray lArray = lToken.Value<JArray>();
        foreach (JObject EachObject in lArray)
        {
            DropData NewDropData = new DropData();
            string DropId = EachObject.Value<string>("DropId");
            NewDropData.DropList = new List<DropDataInfo>();
            JArray lDropArray = EachObject.Value<JArray>("DropInfos");
            foreach (JObject EachDrop in lDropArray)
            {
                DropDataInfo NewDropInfo = ConvertDropDataInfo(EachDrop);
                NewDropData.DropList.Add(NewDropInfo);
            }
            NewDropData.BossDropList = new List<DropDataInfo>();
            JArray lBossDropArray = EachObject.Value<JArray>("BossDropInfos");
            if (lBossDropArray != null)
            {
                foreach (JObject EachDrop in lBossDropArray)
                {
                    DropDataInfo NewBossDropInfo = ConvertDropDataInfo(EachDrop);
                    NewDropData.BossDropList.Add(NewBossDropInfo);
                }
            }
            else
            {
                Debug.LogError("Not Exist Boss Drop Info = " + DropId);
            }
            NewDropData.PostLoad();
            DropDatas.Add(DropId, NewDropData);
        }
    }

    private DropDataInfo ConvertDropDataInfo(JObject InDropObject)
    {
        DropDataInfo NewDropInfo = new DropDataInfo();
        NewDropInfo.ItemId = InDropObject.Value<string>("ItemId");
        NewDropInfo.DropRatio = InDropObject.Value<int>("Ratio");
        return NewDropInfo;
    }

    protected void LoadStatData()
    {
        StatDatas = new Dictionary<string, StatData>();
        TextAsset JsonTextAsset = Resources.Load<TextAsset>("Data/Stats");
        string lJson = JsonTextAsset.text;
        JObject lDataObject = JObject.Parse(lJson);
        JToken lToken = lDataObject["Stats"];
        JArray lArray = lToken.Value<JArray>();
        foreach (JObject EachObject in lArray)
        {
            StatData NewStat = new StatData();
            NewStat.Id = EachObject.Value<string>("Id");
            JArray lEnchantArray = EachObject.Value<JArray>("Enchant");
            NewStat.EnchantLevelData = new Dictionary<int, StatEnchantData>();
            foreach (JObject EachEnchant in lEnchantArray)
            {
                StatEnchantData EachEnchantData = new StatEnchantData();
                EachEnchantData.Level = EachEnchant.Value<int>("Level");
                EachEnchantData.Attack = EachEnchant.Value<int>("Attack");
                EachEnchantData.Armor = EachEnchant.Value<int>("Armor");
                EachEnchantData.Speed = EachEnchant.Value<int>("Speed");
                NewStat.EnchantLevelData.Add(EachEnchantData.Level, EachEnchantData);
            }
            StatDatas.Add(NewStat.Id, NewStat);
        }
    }

    protected void LoadItemData()
    {
        ItemDatas = new Dictionary<string, ItemData>();
        ItemResources = new Dictionary<string, ItemBase>();
        TextAsset JsonTextAsset = Resources.Load<TextAsset>("Data/Items");
        string lJson = JsonTextAsset.text;
        JObject lDataObject = JObject.Parse(lJson);
        JToken lToken = lDataObject["Items"];
        JArray lArray = lToken.Value<JArray>();
        foreach (JObject EachObject in lArray)
        {
            ItemData NewItemData = new ItemData();
            NewItemData.Id = EachObject.Value<string>("Id");
            NewItemData.Path = EachObject.Value<string>("Path");
            NewItemData.IconPath = EachObject.Value<string>("IconPath");
            NewItemData.IsStackable = EachObject.Value<bool>("IsStackable");
            string ItemTypeString = EachObject.Value<string>("Type");
            NewItemData.Type = Enum.Parse<EItemType>(ItemTypeString);
            NewItemData.SellPrice = EachObject.Value<int>("SellPrice");
            NewItemData.Value = EachObject.Value<int>("Value");
            string StatKey = EachObject.Value<string>("Stat");
            if (StatKey != null && StatDatas.ContainsKey(StatKey))
            {
                NewItemData.ItemStatData = StatDatas[StatKey];
            }
            ItemDatas.Add(NewItemData.Id, NewItemData);
            Debug.Log("New Item Add Complete : " + NewItemData.ShowItemDataLog());

            ItemBase ItemObject = Resources.Load<ItemBase>(NewItemData.Path);
            if (ItemObject == null)
            {
                Debug.LogError("Not Exist Path Prefabs : " + NewItemData.Path);
            }
            else
            {
                ItemResources.Add(NewItemData.Id, ItemObject);
            }
        }
    }

    protected void LoadStageData()
    {
        StageDatas = new Dictionary<int, StageData>();
        StageDatas.Clear();

        TextAsset StageJsonTextAsset = Resources.Load<TextAsset>("Data/StageDatas");
        string lStageJson = StageJsonTextAsset.text;
        JObject lStageDataObject = JObject.Parse(lStageJson);
        JToken lStageToken = lStageDataObject["Stages"];
        JArray lStageArray = lStageToken.Value<JArray>();

        foreach (JObject EachObject in lStageArray)
        {
            StageData NewStageData = new StageData();
            NewStageData.StageId = EachObject.Value<int>("StageId");
            NewStageData.MaxSpawnCount = EachObject.Value<int>("MaxSpawn");
            NewStageData.DropId = EachObject.Value<string>("DropId");
            JArray lNpcArray = EachObject.Value<JArray>("UnitPaths");
            foreach (JObject EachNpc in lNpcArray)
            {
                StageUnitData UnitData = ConvertStageUnitData(EachNpc);
                NewStageData.Units.Add(UnitData);
            }
            JObject BossObject = EachObject.Value<JObject>("Boss");
            if (BossObject != null)
            {
                NewStageData.BossUnit = ConvertStageUnitData(BossObject);
            }

            StageDatas.Add(NewStageData.StageId, NewStageData);
        }
    }

    public StageUnitData ConvertStageUnitData(JObject InUnitObj)
    {
        StageUnitData UnitData = new StageUnitData();

        UnitData.UnitId = InUnitObj.Value<string>("Id");
        UnitData.UnitPath = InUnitObj.Value<string>("Path");
        UnitData.UnitSpeed = InUnitObj.Value<float>("Speed");
        UnitData.Hp = InUnitObj.Value<int>("Hp");
        UnitData.Armor = InUnitObj.Value<int>("Armor");
        UnitData.Power = InUnitObj.Value<int>("Power");

        return UnitData;
    }

    public DropData FindDropData(string InDropId)
    {
        if (DropDatas.ContainsKey(InDropId) == false)
        {
            return null;
        }
        return DropDatas[InDropId];
    }

    public ItemData GetItemData(string InItemId)
    {
        if (ItemDatas.ContainsKey(InItemId) == false)
        {
            return null;
        }
        return ItemDatas[InItemId];
    }

    public ItemBase GetItemObject(string InItemId)
    {
        if (ItemResources.ContainsKey(InItemId) == false)
        {
            return null;
        }
        return ItemResources[InItemId];
    }

    public StageData FindStageData(int InStageId)
    {
        if (StageDatas.ContainsKey(InStageId) == false)
        {
            return null;
        }
        return StageDatas[InStageId];
    }

    protected void LoadSkillData()
    {
        SkillDatas = new Dictionary<SkillType, SkillData>();
        SkillResources = new Dictionary<string, SkillBase>();
        SkillDatas.Clear();
        SkillResources.Clear();

        TextAsset SkillJsonTextAsset = Resources.Load<TextAsset>("Data/SkillDatas");
        string lSkillJson = SkillJsonTextAsset.text;
        JObject lDataObject = JObject.Parse(lSkillJson);
        JToken lToken = lDataObject["Skills"];
        JArray lArray = lToken.Value<JArray>();

        foreach (JObject EachObject in lArray)
        {
            SkillData NewSkillData = new SkillData();
            NewSkillData.Type = Enum.Parse<SkillType>(EachObject.Value<string>("Type"));
            NewSkillData.ActiveType = Enum.Parse<SkillActiveType>(EachObject.Value<string>("ActiveType"));
            NewSkillData.SkillIconPath = EachObject.Value<string>("SkillIcon");
            NewSkillData.LevelDatas = new Dictionary<int, SkillLevelData>();
            JArray lLevelArray = EachObject.Value<JArray>("LevelDatas");
            foreach (JObject EachLevel in lLevelArray)
            {
                SkillLevelData NewSkillLevelData = new SkillLevelData();
                NewSkillLevelData.Type = NewSkillData.Type;
                NewSkillLevelData.Level = EachLevel.Value<int>("Level");
                NewSkillLevelData.Path = EachLevel.Value<string>("Path");
                NewSkillLevelData.Power = EachLevel.Value<int>("Power");
                NewSkillLevelData.Size = EachLevel.Value<int>("Size");
                NewSkillLevelData.Speed = EachLevel.Value<float>("Speed");
                NewSkillLevelData.ActiveTime = EachLevel.Value<float>("ActiveTime");
                NewSkillLevelData.CoolTime = EachLevel.Value<float>("CoolTime");

                NewSkillData.LevelDatas.Add(NewSkillLevelData.Level, NewSkillLevelData);

                SkillBase SkillObject = Resources.Load<SkillBase>(NewSkillLevelData.Path);
                string SkillId = GetSkillId(NewSkillLevelData);
                SkillResources.Add(SkillId, SkillObject);
            }
            SkillDatas.Add(NewSkillData.Type, NewSkillData);
        }
    }

    public string GetSkillId(SkillLevelData InSkillLevelData)
    {
        return GetSkillId(InSkillLevelData.Type, InSkillLevelData.Level);
    }

    public string GetSkillId(SkillType InSkillType, int InLevel)
    {
        return string.Format("{0}_{1}", InSkillType.ToString(), InLevel);
    }

    public SkillData FindSkillData(SkillType InSkillType)
    {
        if (SkillDatas.ContainsKey(InSkillType) == false)
        {
            return null;
        }
        return SkillDatas[InSkillType];
    }

    public SkillLevelData FindSkillLevelData(SkillType InSkillType, int InSkillLevel)
    {
        if (SkillDatas.ContainsKey(InSkillType) == false)
        {
            return null;
        }
        if (SkillDatas[InSkillType].LevelDatas.ContainsKey(InSkillLevel) == false)
        {
            return null;
        }
        return SkillDatas[InSkillType].LevelDatas[InSkillLevel];
    }

    public SkillBase GetSkillObjectPrefab(SkillLevelData InSkillLevelData)
    {
        return GetSkillObjectPrefab(GetSkillId(InSkillLevelData));
    }

    public SkillBase GetSkillObjectPrefab(SkillType InType, int InSkillLevel)
    {
        return GetSkillObjectPrefab(GetSkillId(InType, InSkillLevel));
    }
    public SkillBase GetSkillObjectPrefab(string InSkillId)
    {
        if (SkillResources.ContainsKey(InSkillId) == false)
        {
            return null;
        }
        return SkillResources[InSkillId];
    }

    public void SetStageData(GameObject InMyPc, Transform InSpawnRoot, Transform InSkillRoot, Transform InItemRoot)
    {
        mMyPc = InMyPc;
        mSpawnRoot = InSpawnRoot;
        mSkillRoot = InSkillRoot;
        mItemRoot = InItemRoot;
    }
    public void SetCurrentStage(int InStage)
    {
        mStage = InStage;
    }

    // 게임 시간과 관련된 구현사항 적용
    public void UpdateGameTime(float InDeltaTime)
    {
        mCurrentGameTime += InDeltaTime;
    }
    public float GetGameTime()
    {
        return mCurrentGameTime;
    }
    public void ClearGameTime()
    {
        mCurrentGameTime = 0.0f;
    }

    public Dictionary<SkillType, SkillData> GetAllSkillDatas()
    {
        return SkillDatas;
    }

    private static GameDataManager sInstance = null;

    private GameObject mMyPc;
    private Transform mSpawnRoot;
    private Transform mSkillRoot;
    private Transform mItemRoot;

    private Dictionary<SkillType, SkillData> SkillDatas = null;
    private Dictionary<string, SkillBase> SkillResources = null;

    private Dictionary<int, StageData> StageDatas = null;

    private Dictionary<string, StatData> StatDatas = null;

    private Dictionary<string, ItemData> ItemDatas = null;
    private Dictionary<string, ItemBase> ItemResources = null;

    private Dictionary<string, DropData> DropDatas = null;

    private Dictionary<string, ShopData> ShopDatas = null;

    private float mCurrentGameTime = 0.0f;

    private bool mIsInit = false;
}
