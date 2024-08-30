using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManualMissile : SkillBase
{
    public GameObject MissileObject;
    public List<Material> LevelMaterial;
    // Start is called before the first frame update
    void Start()
    {
        mMissileParticle = GetComponent<ParticleSystem>();
    }

    public override void FireSkill(UnitBase InSrcUnit, ActiveSkillData InSkillData, Vector3 InStartPos, Vector3 InStartDir)
    {
        base.FireSkill(InSrcUnit, InSkillData, InStartPos, InStartDir);

        if (InSkillData.ActiveSkillLevelData.Level != CurrentMySkillLevel)
        {
            CurrentMySkillLevel = InSkillData.ActiveSkillLevelData.Level;
            if (MissileObject != null)
            {
                MeshRenderer MissileRenderer = MissileObject.GetComponent<MeshRenderer>();
                if (MissileRenderer != null)
                {
                    List<Material> MissileMats = new List<Material>();
                    MissileMats.Add(LevelMaterial[CurrentMySkillLevel - 1]);
                    MissileRenderer.SetMaterials(MissileMats);

                }
            }
        }
        StartCoroutine(_OnMissileLiftTime());
    }

    public IEnumerator _OnMissileLiftTime()
    {
        float CurrentLifeTime = 0.0f;
        Vector3 MovePosition = Vector3.zero;
        while (true)
        {
            if (FSMStageController.aInstance.IsPlayGame())
            {
                Vector3 AddForceVector = mStartDir * mActiveSkillData.ActiveSkillLevelData.Speed * Time.deltaTime;
                MovePosition += new Vector3(AddForceVector.x, 0, AddForceVector.z);
                transform.position = mStartPos + MovePosition;
                CurrentLifeTime += Time.deltaTime;
                if (CurrentLifeTime > 2.0f)
                {
                    break;
                }
            }
            yield return null;
        }
        StopSkill();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
        {
            return;
        }
        UnitBase TargetUnit = other.GetComponent<UnitBase>();
        if (TargetUnit != null && mSrcUnit != TargetUnit)
        {
            int Damage = mActiveSkillData.ActiveSkillLevelData.Power + mUnitPower;
            TargetUnit.OnHit(Damage);
            StopSkill();
        }
    }
    public override void StopSkill()
    {
        base.StopSkill();
        StopAllCoroutines();
    }

    private int CurrentMySkillLevel = 0;
    private ParticleSystem mMissileParticle;
    private AudioSource mFireSoundSource;
}

