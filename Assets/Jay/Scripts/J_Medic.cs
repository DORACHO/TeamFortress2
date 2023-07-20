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
        //길정보를 알고싶다
        Vector3 pos =J_PathManager.instance.points[targetIndex].position;

        //내가 길의 어떤 위치로 갈것인지 알고싶다

        //그곳으로 이동하고싶다
        agent.SetDestination(pos);

        //1까지 근접했다면 도착한것으로 하고싶다
        pos.y = transform.position.y;
        float dist = Vector3.Distance(transform.position, pos);
        //도착했다면 targetIndex를 1증가시키고싶다
        if (dist < agent.stoppingDistance)
        {
            targetdex = (targetIndex + 1) % J_PathManager.instance.points.Length;
        }
        //만약 플레이어가 내 인식거리안에 들어왔다면
        float dist2 = Vector3.Distance(transform.position, target.transform.position);
        if(dist2 < attackDistance)
        {
            //추적상태로 전이하고싶다
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
        //목적지와 나의 거리를 재고싶다
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
        //y값을 플레이어와 동일하게한다
        targetPos = new Vector3(Players.position.x, transform.position.y, Players.position.z);
        transform.LookAt(targetPos);
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
            state = State.Chase;
            anim.SetTrigger("Chase");
            agent.isStopped = false;
        }
    }
    public void OnAttackWait_Finished()
    {
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        if (distance > attackRange) // 공격 범위 벗어남
        {
            state = State.Chase;
            anim.SetTrigger("Chase");
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