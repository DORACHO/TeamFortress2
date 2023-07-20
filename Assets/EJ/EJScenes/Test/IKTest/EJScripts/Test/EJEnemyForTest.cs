using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//map을 구워줘야 한다 static - navigation으로 

public class EJEnemyForTest : MonoBehaviour
{
    GameObject target;
    //NavMeshAgent agent;
    CharacterController cc;

    float speed = 5f;

    float attackRange = 15;
    float currentTime;
    float fireTime = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        target = GameObject.Find("PlayerArm");
        print(target);
        UpdateMove();
        UpdateAttack();
    }

    private void UpdateMove()
    {
        //따라갈 target의 위치
        //agent.destination = target.transform.position;
    }

    private void UpdateAttack()
    {
        
        float distance = Vector3.Distance(target.transform.position, transform.position);

        if (fireTime < currentTime)
        {
            if (distance < attackRange)
            {
                EnemyFire();
                currentTime = 0;
            }
        }
    }

    private void EnemyFire()
    {
        Ray ray = new Ray(transform.position,transform.forward);
        RaycastHit hitInfo;

        int layer = (1 << LayerMask.NameToLayer("Player"));

        Debug.DrawRay(transform.position,transform.forward, Color.red);

        if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
        {
            //EJPSHP.instance.HP -= 50;
            EJPSHP.instance.SetHP(50, transform.position);
        }
    }
}

