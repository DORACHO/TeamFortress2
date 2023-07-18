using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//map�� ������� �Ѵ� static - navigation���� 

public class EJEnemyForTest : MonoBehaviour
{
    GameObject target;
    NavMeshAgent agent;
    CharacterController cc;

    float speed = 5f;

    public float attackRange = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.Find("PlayerArm");
        print(target);
        UpdateMove();
        UpdateAttack();
    }

    private void UpdateMove()
    {
        //���� target�� ��ġ
        agent.destination = target.transform.position;
    }

    private void UpdateAttack()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);

        if (distance < attackRange )
        {
            EnemyFire();
        }
    }

    private void EnemyFire()
    {
        Ray ray = new Ray(transform.position,transform.forward);
        RaycastHit hitInfo;

        int layer = (1 << LayerMask.NameToLayer("Player"));

        Debug.DrawRay(transform.position,transform.forward, Color.red, 3);
        if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
        {
            //EJPSHP.instance.HP -= 50;
            EJPSHP.instance.SetHP(50, transform.position);
        }

    }
}
