using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�̺�Ʈ�Լ��� �����ϰ�ʹ�
//Hit, attackFinished�϶�
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
        medic.OnAttack_Hit();
    }
    void OnAttack_Finished()
    {
        //medic.OnAttack_Finished();
    }
    void OnAttackWait_Finished()
    {
        //medic.OnAttackWait_Finished();
    }
}
