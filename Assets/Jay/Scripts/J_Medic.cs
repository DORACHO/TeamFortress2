using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PatrolNChase;
using Unity.VisualScripting;

public class J_Medic : MonoBehaviour
{
    J_MedicHP medicHP;
    NavMeshAgent agent;
    GameObject target;
    Animator anim;

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
        medicHP = GetComponent<J_MedicHP>();
        
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

    public int targetIndex;
    private void UpdatePatrol()
    {

        //�������� �˰�ʹ�
        Vector3 pos = J_PathManager.instance.points[targetIndex].position;

        //���� ���� � ��ġ�� �������� �˰�ʹ�

        //�װ����� �̵��ϰ�ʹ�
        agent.SetDestination(pos);
        print(pos);
        //1���� �����ߴٸ� �����Ѱ����� �ϰ�ʹ�
        
        pos.y = transform.position.y;
        float dist = Vector3.Distance(transform.position, pos);
        //�����ߴٸ� targetIndex�� 1������Ű��ʹ�
        if (dist <= agent.stoppingDistance)
        {
            targetIndex = (targetIndex + 1) % J_PathManager.instance.points.Length;
            //targetdex = (targetIndex + 1);
            
        }
        //���� �÷��̾ �� �νİŸ��ȿ� ���Դٸ�
        float dist2 = Vector3.Distance(transform.position, target.transform.position);
        if(dist2 < attackDistance)
        {
            //�������·� �����ϰ�ʹ�
            state = State.Chase;
        }
    }
    float attackDistance = 10;

    private void UpdateIdle()
    {
        //agent.SetDestination(J_PathManager. instance.points[0].position);
        target = GameObject.FindWithTag("Player");
        //target = GameObject.Find("PlayerArm");
        //target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            //�������·� �����ϰ�ʹ�
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
    #region �ִϸ��̼� �̺�Ʈ�Լ��� ���� ȣ��Ǵ� �Լ���...
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
            print("AttackFinished");
            state = State.Chase;
            anim.SetTrigger("Move");
            agent.isStopped = false;
        }
    }
    public void OnAttackWait_Finished()
    {
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        if (distance > attackRange) // ���� ���� ���
        {
            state = State.Chase;
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
        state = State.Chase;
        anim.SetTrigger("Move");
        agent.isStopped = false;
    }
    #endregion
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
    void DamageProcess(int damage = 1)
    {
        if (state == State.Die)
        {
            return;
        }
        agent.isStopped = true;
        medicHP.HP -= 1;
        if(medicHP.HP < 0)
        {

            state = State.Die;

            Destroy(gameObject,5);
            anim.SetTrigger("Die");

            Collider col = GetComponentInChildren<Collider>();
            if (col)
            {
                col.enabled = false;
            }
        }
        else
        {
            state = State.Chase;
            agent.isStopped = false;
            anim.SetTrigger("Move");
        }

    }



}