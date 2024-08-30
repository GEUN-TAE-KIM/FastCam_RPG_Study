using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl
{
    public delegate void OnMoving(Vector3 pDirect);
    public delegate void OnMoveStart();
    public delegate void OnMoveEnd();
    public delegate void OnMouseInput(int InIndex, Vector3 InMousePos);
    public delegate void OnInputSkill(int InSkillIndex);

    public OnMoving aOnMoving { get; set; }
    public OnMoveStart aOnMoveStart { get; set; }
    public OnMoveEnd aOnMoveEnd { get; set; }
    public OnMouseInput aOnMouseInput { get; set; }
    public OnInputSkill aOnInputSkill { get; set; }

    private KeyCode[] SkillKeyCodes =
    {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
    };

    public static GameControl aInstance
    {
        get
        {
            if (sInstance == null)
            {
                sInstance = new GameControl();
            }
            return sInstance;
        }
    }
    public void Init()
    {

    }
    public void SetControlObject(GameObject InGameObject)
    {
        mControlObject = InGameObject;
        mMovementBase = InGameObject.GetComponent<UnitMovementBase>();
    }
    public GameObject GetControlObject()
    {
        return mControlObject;
    }
    public void OnUpdate()
    {
        _UpdateKeyboard();
        _UpdateMouseInput();
    }
    public void Clear()
    {

    }
    private void _UpdateMouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (aOnMouseInput != null)
            {
                aOnMouseInput(0, Input.mousePosition);
            }
        }
        //if (Input.GetMouseButton(1))
        //{
        //	if (aOnMouseInput != null)
        //	{
        //		aOnMouseInput(1, Input.mousePosition);
        //	}
        //}
    }
    private void _UpdateKeyboard()
    {
        if (FSMStageController.aInstance.IsPlayGame() == false)
        {
            mIsMoving = false;
            return;
        }
        if (mMovementBase.mIsEnableMove == false)
        {
            mIsMoving = false;
            return;
        }
        Vector3 MoveVector = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            MoveVector += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            MoveVector += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            MoveVector += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            MoveVector += new Vector3(1, 0, 0);
        }

        for (int i = 0; i < SkillKeyCodes.Length; i++)
        {
            if (Input.GetKeyDown(SkillKeyCodes[i]))
            {
                if (aOnInputSkill != null)
                {
                    aOnInputSkill(i);
                }
            }
        }

        Vector3 MoveVectorNormal = MoveVector.normalized;
        if (MoveVectorNormal != Vector3.zero)
        {
            if (aOnMoving != null)
            {
                aOnMoving(MoveVectorNormal);
            }
            if (mIsMoving == false)
            {
                if (aOnMoveStart != null)
                {
                    aOnMoveStart();
                }
                mIsMoving = true;
            }
        }
        else
        {
            if (mIsMoving == true)
            {
                if (aOnMoving != null)
                {
                    aOnMoveEnd();
                }
                mIsMoving = false;
            }
        }
    }

    private static GameControl sInstance = null;

    private GameObject mControlObject = null;
    private UnitMovementBase mMovementBase = null;
    private bool mIsMoving = false;
}
