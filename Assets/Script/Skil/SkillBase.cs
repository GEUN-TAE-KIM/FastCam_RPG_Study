using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    public SkillType mSkillType { set; get; }
    public Vector3 mStartPos { get; private set; }
    public Vector3 mStartDir { get; private set; }
    public ActiveSkillData mActiveSkillData { get; private set; }
    public StatData mAddStatData { get; private set; }
    public UnitBase mSrcUnit { get; private set; }

    public AudioClip mFireAudioClip;
    public virtual void Awake()
    {
        mFireAudioSource = GetComponent<AudioSource>();
        if (mFireAudioSource != null)
        {
            mFireAudioSource.enabled = true;
            mFireAudioSource.playOnAwake = false;
        }

    }
    public virtual void FireSkill(UnitBase InSrcUnit, ActiveSkillData InSkillData, Vector3 InStartPos, Vector3 InStartDir)
    {
        mSrcUnit = InSrcUnit;
        if (mSrcUnit != null)
        {
            mUnitPower = mSrcUnit.mUnitData.Power;
        }
        mActiveSkillData = InSkillData;
        mStartPos = InStartPos;
        mStartDir = InStartDir;
        mSkillLevel = InSkillData.ActiveSkillLevelData.Level;

        transform.position = mStartPos;
        if (mFireAudioClip != null)
        {
            mFireAudioSource.clip = mFireAudioClip;
            mFireAudioSource.Play();
        }


    }

    public virtual void StopSkill()
    {
        gameObject.SetActive(false);
        if (mSkillLevel == mActiveSkillData.ActiveSkillLevelData.Level)
        {
            GamePoolManager.aInstance.EnqueueSkillPool(this, mSkillLevel);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public virtual void Update()
    {

    }
    public virtual void OnDestroy()
    {

    }
    private int mSkillLevel;
    protected int mUnitPower = 0;
    protected AudioSource mFireAudioSource;
}

