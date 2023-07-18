using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_IKManager : MonoBehaviour
{
    Animator anim;
    public Transform leftHand;
    public Transform rightHand;

    //public Transform lookTarget;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        MySetIK(AvatarIKGoal.LeftHand, leftHand);
        MySetIK(AvatarIKGoal.RightHand, rightHand);
        //anim.SetLookAtWeight(1);
        //anim.SetLookAtPosition(lookTarget.position);    
    }
    void MySetIK(AvatarIKGoal goal, Transform target)
    {
        anim.SetIKPositionWeight(goal, 1);
        anim.SetIKRotationWeight(goal, 1);
        anim.SetIKPosition(goal,target.position);
        anim.SetIKRotation(goal,target.rotation);
    }
}
