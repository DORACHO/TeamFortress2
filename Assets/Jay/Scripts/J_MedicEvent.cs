using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//이벤트함수를 제작하고싶다
//Hit, attackFinished일때
public class J_MedicEvent : MonoBehaviour
{
    J_Medic medic;
    // Start is called before the first frame update
    void Awake()
    {
        medic = GetComponentInParent<J_Medic>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnAttack_Hit()
    {
        //print("AttackHit");
        medic.OnAttack_Hit();
    }
    void OnAttack_Finished()
    {
        //print("AttackFinished");
        medic.OnAttack_Finished();
    }
    void OnAttackWait_Finished()
    {
        //print("AttackWaitFinished");
        medic.OnAttackWait_Finished();
    }
    public void OnReact_Finished()
    {
        medic.OnReact_Finished();
    }
}
