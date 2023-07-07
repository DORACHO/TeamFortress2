using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//map을 구워줘야 한다 static - navigation으로 

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
        target = GameObject.Find("Player");
        print(target);
        UpdateMove();
    }

    private void UpdateMove()
    {
        //따라갈 target의 위치
        agent.destination = target.transform.position;
    }

    private void UpdateAttack()
    {
        
    }
}
