using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public ItemData mItemData { get; set; } = new ItemData();
    public bool mStageFinishItem = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        UnitBase AcquiredUnit = other.GetComponent<UnitBase>();
        if (AcquiredUnit != null)
        {
            if (mTargetUnit == AcquiredUnit)
            {
                ItemManager.aInstance.DespawnItem(this);
                mTargetUnit.OnGetterItem(this);
                mTargetUnit = null;
            }
        }
    }
    public void GetItem(UnitBase InTargetUnit)
    {
        mTargetUnit = InTargetUnit;
        StartCoroutine(_OnGetItem());
    }
    private IEnumerator _OnGetItem()
    {
        if (mTargetUnit == null)
        {
            yield break;
        }
        while (true)
        {
            if (mTargetUnit == null)
            {
                yield break;
            }
            if (FSMStageController.aInstance.IsPlayGame())
            {
                Vector3 TargetPos = mTargetUnit.transform.position;
                Vector3 MoveDir = (TargetPos - transform.position).normalized;
                transform.position += MoveDir * Time.deltaTime * 10.0f;
            }
            yield return null;
        }
    }

    private UnitBase mTargetUnit;
}
