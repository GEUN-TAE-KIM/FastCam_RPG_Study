using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int mStageId = 1;
    public GameObject mMyPC;
    public Transform mNpcSpwnParent;
    public Transform mSkillObjectParent;
    public Transform mItemObjectParent;

    // Start is called before the first frame update
    void Start()
    {
        GameDataManager.aInstance.Init();
        GameDataManager.aInstance.SetStageData(mMyPC, mNpcSpwnParent, mSkillObjectParent, mItemObjectParent);
        GameDataManager.aInstance.SetCurrentStage(mStageId);
        GameDataManager.aInstance.LoadAll();

        GamePoolManager.aInstance.Init();
        GameControl.aInstance.Init();
        GameControl.aInstance.SetControlObject(mMyPC);
        SpawnManager.aInstance.Init();
        FSMStageController.aInstance.Init();

        FSMStageController.aInstance.EnterStage();
    }

    // Update is called once per frame
    void Update()
    {
        FSMStageController.aInstance.OnUpdate(Time.deltaTime);

        GameControl.aInstance.OnUpdate();
    }

    private void FixedUpdate()
    {
        
    }

    private void LateUpdate()
    {
        
    }

    void OnDestroy()
    {
        GameDataManager.aInstance.Clear();

        GamePoolManager.aInstance.Clear();
        GameControl.aInstance.Clear();
        SpawnManager.aInstance.Clear();
        FSMStageController.aInstance.Clear();
    }

}
