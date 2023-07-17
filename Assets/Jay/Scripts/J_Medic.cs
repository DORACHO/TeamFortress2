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
        //�������� ���� �Ÿ��� ���ʹ�
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

    // �ִϸ��̼� �̺�Ʈ�Լ��� ���� ȣ��Ǵ� �Լ���
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
        if (distance > attackRange) // ���� ���� ���
        {
            state = State.Move;
            anim.SetTrigger("Move");
            agent.isStopped = false;
        }
        else // ���� ������ �Ÿ�
        {
            anim.SetBool("bAttack", true);
        }
    }
    internal void OnReact_Finished()
    {
        // ���׼��� �������� Move���·� �����ϰ�ʹ�.
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