using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//손을 IK로 제어하고 싶다.

public class IKManager : MonoBehaviour
{
    Animator Anim;
    public Transform leftHand;
    public Transform righttHand;

    public Transform lookObject;
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        MySetIK(AvatarIKGoal.LeftHand,leftHand);
        MySetIK(AvatarIKGoal.RightHand,righttHand);

        Anim.SetLookAtWeight(1);
        Anim.SetLookAtPosition(lookObject.position);
    }

    void MySetIK(AvatarIKGoal goal, Transform target)
    {
        //Anim.SetIKPositionWeight > weight가 1이라면 ik해놓은 곳으로 가고, 0.5면 중간
        Anim.SetIKPositionWeight(goal, 1);
        Anim.SetIKRotationWeight(goal, 1);
        Anim.SetIKPosition(goal, target.position);
        Anim.SetIKRotation(goal, target.rotation);
    }
}
