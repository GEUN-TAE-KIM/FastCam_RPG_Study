using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    public static ItemManager aInstance
    {
        get
        {
            if (sInstance == null)
            {
                sInstance = new ItemManager();
            }
            return sInstance;
        }
    }

    public void SpawnItem(string InItemId, Vector3 InSpawnPos, bool mIsBossDropItem = false)
    {
        ItemBase ItemObject = GamePoolManager.aInstance.DequeueItemPool(InItemId);
        if (ItemObject == null)
        {
            ItemBase NewItemObject = GameDataManager.aInstance.GetItemObject(InItemId);
            ItemObject = GameObject.Instantiate<ItemBase>(NewItemObject,
                                                        GameDataManager.aInstance.GetItemRootTransform());
        }

        if (ItemObject == null)
        {
            Debug.LogError("Not Exist ItemObject : " + InItemId);
            return;
        }
        ItemObject.transform.position = InSpawnPos;
        ItemObject.mItemData = GameDataManager.aInstance.GetItemData(InItemId);
        ItemObject.mStageFinishItem = mIsBossDropItem;
        ItemObject.gameObject.SetActive(true);
    }

    public void DespawnItem(ItemBase InItemBase)
    {
        InItemBase.gameObject.SetActive(false);
        GamePoolManager.aInstance.EnqueueItemPool(InItemBase);
    }

    public void DropItem(Vector3 InDropPos, bool InIsBoss)
    {
        int lStageId = GameDataManager.aInstance.mStage;
        StageData lStageData = GameDataManager.aInstance.FindStageData(lStageId);
        if (lStageData == null)
        {
            return;
        }
        DropData lCurrentDropData = GameDataManager.aInstance.FindDropData(lStageData.DropId);
        if (lCurrentDropData == null)
        {
            return;
        }
        DropDataInfo lDropDataInfo = lCurrentDropData.RandomPickDropInfo(InIsBoss);
        if (lDropDataInfo == null)
        {
            return;
        }
        SpawnItem(lDropDataInfo.ItemId, InDropPos, InIsBoss);

        Debug.Log("Spawn Item id : " + lDropDataInfo.ItemId + " / Drop Pos : " + InDropPos);
    }

    private static ItemManager sInstance = null;
}

