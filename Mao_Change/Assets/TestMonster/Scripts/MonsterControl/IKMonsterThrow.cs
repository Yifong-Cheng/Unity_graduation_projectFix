using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKMonsterThrow : MonoBehaviour {
    #region Data
    [SerializeField]
    public Transform myRightHandMiddleFinger;
    Transform myRightHand;
    [SerializeField]
    public GameObject otherObj;

    [SerializeField]
    float StartTime = 1.0f;// orig .25 ||.05  new 1.0
    [SerializeField]
    float EndTime = .3f;// orig .55   new 0.3

    Animator animator;
    static readonly Vector3 offset = new Vector3(.02f, .04f, 0);
    Vector3 myRightMiddleHandFingerPosition, myRightHandPosition;
    Quaternion myRightMiddleFingerRotion;

    #endregion

    private float p = 2.0f;
    private float tp;

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

            percent *= tp;//0 -> 2
            if (percent > 1)
            {
                percent = tp - percent;
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
        if (otherObj != null)
        {
            Vector3 targetPosition = otherObj.transform.position + otherObj.transform.rotation * offset;
            //targetPosition += myRightMiddleHandFingerPosition + myRightMiddleFingerRotion * offset - myRightHandPosition;

            animator.SetIKPosition(AvatarIKGoal.RightHand, targetPosition);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, percentComplete);

            //Quaternion aeou = Quaternion.LookRotation(this.transform.forward, otherObj.transform.position);
            //animator.SetIKRotation(AvatarIKGoal.RightHand, aeou);
            //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, percentComplete);

            animator.SetLookAtPosition(targetPosition);
            animator.SetLookAtWeight(percentComplete);

            //animator.bodyRotation = Quaternion.Lerp(transform.rotation, delta * Quaternion.LookRotation(temp, Vector3.up), percentComplete);
        }


    }

    private void LateUpdate()
    {
        myRightMiddleHandFingerPosition = myRightHandMiddleFinger.position;
        myRightMiddleFingerRotion = myRightHandMiddleFinger.rotation;
        myRightHandPosition = myRightHand.position;
    }

    private float timeScale = 1.0f;
    public float maxvalue = 3;
    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 200, 150), "Player Machine");

        GUI.TextField(new Rect(20, 40, 180, 20), string.Format("State: {0}", percentComplete));
        timeScale = GUI.HorizontalSlider(new Rect(20, 70, 180, 20), timeScale, 0.0f, 1.0f);

        Time.timeScale = timeScale;

        GUI.TextField(new Rect(20, 80, 180, 20), string.Format("Percent: {0}", p));
        p = GUI.HorizontalSlider(new Rect(20, 110, 180, 20), p, 0.0f, 3f);
        tp = p;
    }
}
