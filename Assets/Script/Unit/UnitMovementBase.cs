using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovementBase : MonoBehaviour
{
    public float mSpeed = 5.0f;
    public Transform mRotationTransform;
    public float mRotationSpeed = 400.0f;
    public Animator mAnimator;
    public bool mIsEnableMove = true;

    void Start()
    {

    }

    public void AddSpeed(int InAddSpeed)
    {
        mSpeed += InAddSpeed;
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        // Height °è»ê
        Vector3 lNowPosition = transform.position + new Vector3(0, 100, 0);
        Vector3 lDirection = new Vector3(0, -1, 0);
        RaycastHit lHit;
        int layermask = 1 << LayerMask.NameToLayer("Terrain");
        if (Physics.Raycast(lNowPosition, lDirection, out lHit, 200, layermask))
        {
            float lHeight = lHit.point.y;
            Vector3 lNewPos = transform.position;
            lNewPos.y = lHeight;
            transform.position = lNewPos;
        }
    }
}

