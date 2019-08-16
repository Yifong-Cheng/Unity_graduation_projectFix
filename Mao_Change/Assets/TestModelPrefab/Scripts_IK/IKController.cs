using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IKController : MonoBehaviour {

    #region Data
    [SerializeField]
    Transform myRightHandMiddleFinger;
    Transform myRightHand;
    [SerializeField]
    IKController otherGuys;

    [SerializeField]
    float StartTime = .25f;
    [SerializeField]
    float EndTime = .55f;

    Animator animator;
    static readonly Vector3 offset = new Vector3(.02f, .04f, 0);
    Vector3 myRightMiddleHandFingerPosition, myRightHandPosition;
    Quaternion myRightMiddleFingerRotion;

    #endregion

    float percentComplete
    {
        get
        {
            AnimatorStateInfo currentAnimation = animator.GetCurrentAnimatorStateInfo(0);
            float percent = currentAnimation.normalizedTime % 1;

            percent -= StartTime;
            percent /= EndTime - StartTime;
            if (percent <= 0 || percent >= 1)
            {
                return 0;
            }

            percent *= 2;//0 -> 2
            if(percent > 1)
            {
                percent = 2 - percent;
            }
            return percent;
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        myRightHand = myRightHandMiddleFinger.parent;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        Vector3 targetPosition = otherGuys.myRightMiddleHandFingerPosition + otherGuys.myRightMiddleFingerRotion * offset;
        targetPosition += myRightMiddleHandFingerPosition + myRightMiddleFingerRotion * offset - myRightHandPosition;

        animator.SetIKPosition(AvatarIKGoal.RightHand, targetPosition);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, percentComplete);

        //animator.SetIKRotation(AvatarIKGoal.RightHand, aeou);
        //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, percentComplete);

        //animator.SetLookAtPosition(targetPosition);
        //animator.SetLookAtWeight(percentComplete);

        //animator.bodyRotation = Quaternion.Lerp(transform.rotation, delta * Quaternion.LookRotation(temp, Vector3.up), percentComplete);

    }

    private void LateUpdate()
    {
        myRightMiddleHandFingerPosition = myRightHandMiddleFinger.position;
        myRightMiddleFingerRotion = myRightHandMiddleFinger.rotation;
        myRightHandPosition = myRightHand.position;
    }

}
