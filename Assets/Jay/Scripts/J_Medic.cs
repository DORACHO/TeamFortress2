using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PatrolNChase;


public class J_Medic : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject target;
    Animator anim;
    public int targetdex;
    public enum State
    {
        Idle,
        Chase,
        Attack,
        React,
        Die,
        Patrol,
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
            case State.Chase: UpdateChase();break;
            case State.Attack: UpdateAttack();break;
            case State.Patrol: UpdatePatrol();break;
        }
    }

    int targetIndex;
    private void UpdatePatrol()
    {
        //�������� �˰�ʹ�
        Vector3 pos =J_PathManager.instance.points[targetIndex].position;

        //���� ���� � ��ġ�� �������� �˰�ʹ�

        //�װ����� �̵��ϰ�ʹ�
        agent.SetDestination(pos);

        //1���� �����ߴٸ� �����Ѱ����� �ϰ�ʹ�
        pos.y = transform.position.y;
        float dist = Vector3.Distance(transform.position, pos);
        //�����ߴٸ� targetIndex�� 1������Ű��ʹ�
        if (dist < agent.stoppingDistance)
        {
            targetdex = (targetIndex + 1) % J_PathManager.instance.points.Length;
        }
        //���� �÷��̾ �� �νİŸ��ȿ� ���Դٸ�
        float dist2 = Vector3.Distance(transform.position, target.transform.position);
        if(dist2 < attackDistance)
        {
            //�������·� �����ϰ�ʹ�
            state = State.Chase;
        }

    }
    float attackDistance = 5;

    private void UpdateIdle()
    {
        //agent.SetDestination(J_PathManager. instance.points[0].position);
        target = GameObject.Find("PlayerArm");
        //target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            state = State.Patrol;
            anim.SetTrigger("Move");
            agent.isStopped = false;
        }
    }

    private void UpdateChase()
    {
        //agent.SetDestination(J_PathManager.instance.points[0].position);
        agent.destination = target.transform.position;
        //�������� ���� �Ÿ��� ���ʹ�
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        if(distance < attackRange)
        {
            state = State.Attack;
            anim.SetTrigger("Attack");
            agent.isStopped = true;
        }
        else if( distance > farDistance)
        {
            state = State.Patrol;
        }

    }
    public float farDistance = 10;
    public Transform Players;
    private Vector3 targetPos;
    private void UpdateAttack()
    {
        //y���� �÷��̾�� �����ϰ��Ѵ�
        targetPos = new Vector3(Players.position.x, transform.position.y, Players.position.z);
        transform.LookAt(targetPos);
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
            state = State.Chase;
            anim.SetTrigger("Chase");
            agent.isStopped = false;
        }
    }
    public void OnAttackWait_Finished()
    {
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        if (distance > attackRange) // ���� ���� ���
        {
            state = State.Chase;
            anim.SetTrigger("Chase");
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
        state = State.Chase;
        anim.SetTrigger("Chase");
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