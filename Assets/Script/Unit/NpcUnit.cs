using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcUnit : UnitBase
{
    public Transform mHpBarTransform;
    public Transform mBillboardTransform;

    public StageUnitData mStageUnitData { get; set; }
    public bool mIsMoveToTarget { get; set; } = false;

    public AudioClip mHitAudioClip;

    public bool mIsBoss = false;

    public override void Start()
    {
        base.Start();
        mCamTransform = Camera.main.transform;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (mBillboardTransform != null)
        {
            mBillboardTransform.LookAt(mBillboardTransform.position + mCamTransform.rotation * Vector3.forward,
                                        mCamTransform.rotation * Vector3.up);
        }
    }

    public void Init(int InUnitId, StageUnitData InStageUnitData)
    {
        InitUnit(InUnitId, InStageUnitData.Hp, InStageUnitData.Power, InStageUnitData.Armor);
        mStageUnitData = InStageUnitData;
        mIsMoveToTarget = true;
        mIsNoneDamage = false;
     //   GameDataManager.aInstance.mLiveNpcUnitCount++;
        _UpdateHp();
    }

    private void _UpdateHp()
    {
        if (mHpBarTransform != null)
        {
            float lHpPercent = (float)mUnitData.Hp / mUnitData.TotalHp;
            mHpBarTransform.localScale = new Vector3(lHpPercent, 1.0f, 1.0f);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        MyPcUnit lMyPcUnit = other.GetComponent<MyPcUnit>();
        if (lMyPcUnit != null)
        {
            mIsMoveToTarget = false;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        MyPcUnit lMyPcUnit = other.GetComponent<MyPcUnit>();
        if (lMyPcUnit != null)
        {
            mIsMoveToTarget = true;
        }
    }

    public void SetSpeed(float InSpeed)
    {
        mStageUnitData.UnitSpeed = InSpeed;
        NpcUnitMovement Movement = GetComponent<NpcUnitMovement>();
        if (Movement != null)
        {
            Movement.mSpeed = InSpeed;
        }
    }

    public override void OnHit(int InDamage)
    {
        if (FSMStageController.aInstance.IsPlayGame() == false)
        {
            return;
        }
        if (mIsNoneDamage == true)
        {
            return;
        }
        if (mUnitAudioSource != null)
        {
            mUnitAudioSource.clip = mHitAudioClip;
            mUnitAudioSource.Play();
        }

        mIsNoneDamage = true;
        base.OnHit(InDamage);
        _UpdateHp();
        if (mIsAlive)
        {
            StartCoroutine(_OnHitting());
        }
    }
    private IEnumerator _OnHitting()
    {
        yield return new WaitForSeconds(1.0f);
        mIsNoneDamage = false;
    }

    public override void OnDie()
    {
        base.OnDie();
        mIsAlive = false;
        gameObject.SetActive(false);
   //     GamePoolManager.aInstance.EnqueueNpcPool(this);
   //     GameDataManager.aInstance.mLiveNpcUnitCount = Mathf.Max(0, --GameDataManager.aInstance.mLiveNpcUnitCount);
        StopAllCoroutines();

        ItemManager.aInstance.DropItem(transform.position, mIsBoss);
    }

    private bool mIsNoneDamage = false;
    private Transform mCamTransform = null;
}
