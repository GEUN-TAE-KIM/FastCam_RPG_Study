using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData
{
    public int TotalHp = 0;
    public int Hp = 0;
    public int Power = 0;
    public int Armor = 0;
}

public class UnitBase : MonoBehaviour
{
    public bool mIsAlive { set; get; }
    public UnitData mUnitData { private set; get; }
    public int mUnitId { private set; get; }

    public virtual void Start()
    {
        mUnitAudioSource = GetComponent<AudioSource>();
        if (mUnitAudioSource != null)
        {
            mUnitAudioSource.enabled = true;
            mUnitAudioSource.playOnAwake = false;
        }
    }

    public virtual void InitUnit(int InUnitId, int InHp, int InPower, int InArmor)
    {
        mUnitId = InUnitId;
        mUnitData = new UnitData();
        mUnitData.TotalHp = mUnitData.Hp = InHp;
        mUnitData.Power = InPower;
        mUnitData.Armor = InArmor;
        mIsAlive = true;
    }

    public virtual void OnHit(int InDamage)
    {
        if (mUnitData == null)
        {
            return;
        }
        int HitDamage = Mathf.Max(0, InDamage - mUnitData.Armor);
        mUnitData.Hp -= HitDamage;
        if (mUnitData.Hp <= 0)
        {
            OnDie();
        }
    }
    public virtual void OnDie()
    {
        mIsAlive = false;
    }

    public virtual void OnGetterItem(ItemBase InItemBase)
    {
        Debug.Log("Getter Item : " + InItemBase.mItemData.Id + " / Acquired Unit : " + mUnitId);
    }

    protected AudioSource mUnitAudioSource;
}
