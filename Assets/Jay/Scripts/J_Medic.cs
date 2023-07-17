using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class J_Medic : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject target;
    Animator anim;
    public enum State
    {
        Idle,
        Move,
        Attack,
        React,
        Die,
    }
    public State state;
    public float attackRange =5;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle: UpdateIdle();break;
            case State.Move: UpdateMove();break;
            case State.Attack: UpdateAttack();break;
        }
    }

    private void UpdateIdle()
    {
        target = GameObject.Find("Player");
        if(target != null)
        {
            state = State.Move;
            anim.SetTrigger("Move");
            agent.isStopped = false;
        }
    }

    private void UpdateMove()
    {
        agent.destination = target.transform.position;
        //목적지와 나의 거리를 재고싶다
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        if(distance < attackRange)
        {
            state = State.Attack;
            anim.SetTrigger("Attack");
            agent.isStopped = true;
        }
    }

    private void UpdateAttack()
    {
        transform.LookAt(target.transform);
    }

    // 애니메이션 이벤트함수를 통해 호출되는 함수들
    public void OnAttack_Hit()
    {
        anim.SetBool("bAttack", false);
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        if(distance < attackRange)
        {
            print("Enemy -> PlayerHit");
           
        }
    }
    public void OnAttack_Finished()
    {
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        if (distance > attackRange)
        {
            state = State.Move;
            anim.SetTrigger("Move");
            agent.isStopped = false;
        }
    }
    public void OnAttackWait_Finished()
    {
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        if (distance > attackRange) // 공격 범위 벗어남
        {
            state = State.Move;
            anim.SetTrigger("Move");
            agent.isStopped = false;
        }
        else // 공격 가능한 거리
        {
            anim.SetBool("bAttack", true);
        }
    }
    internal void OnReact_Finished()
    {
        // 리액션이 끝났으니 Move상태로 전이하고싶다.
        state = State.Move;
        anim.SetTrigger("Move");
        agent.isStopped = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            Destroy(collision.gameObject);
        }
        else
        {
           
        }
    }

}