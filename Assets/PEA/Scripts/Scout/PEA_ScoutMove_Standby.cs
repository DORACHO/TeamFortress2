using System;   //Serializable�� ����ϱ� ���� ������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PEA_ScoutMove_Standby : MonoBehaviour
{
    private enum State 
    {
        Standby,
        Chase,
        Attack,
        Damage,
        Die
    }

    private enum WeaponState 
    {
        ScatterGun,
        PistolGun
    }

    private State state = State.Standby;
    private WeaponState weaponState = WeaponState.PistolGun;

    // ������
    #region
    private float curTime = 0f;
    private float distanceWithPlayer = 0f;
    private bool isInSight = true;                                           // �÷��̾ �þ� �ȿ� �ִ��� Ȯ��

    private readonly float sightAngle = 60f;                                 // �þ� ���� / 2(�չ����� �������� ����ҰŶ� 2�� ����) 
    private readonly float attackRange = 10f;                                // ���� �ݰ�
    private readonly float sensingDistance = 20f;

    private NavMeshAgent nav;
    private Transform player;
    private PEA_ScatterGun scatterGun;
    private PEA_PistolGun pistolGun;
    private Animator anim;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        scatterGun = GetComponentInChildren<PEA_ScatterGun>();
        pistolGun = GetComponentInChildren<PEA_PistolGun>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        print(state);
        switch (state)
        {
            case State.Standby:
                CheackDistance();
                break;

            case State.Chase:
                Chase();
                CheackDistance();
                break;

            case State.Attack:
                Attack();
                LookAtPlayer();
                CheackDistance();
                break;

            case State.Damage:
                break;

            case State.Die:
                break;
        }
    }

    private void CheackDistance()
    {
        distanceWithPlayer = Vector3.Distance(transform.position, player.position);

        switch (state)
        {
            case State.Standby:
                if(distanceWithPlayer <= sensingDistance)
                {
                    IsPlayerInSight();
                }
                break;

            case State.Chase:

                // �÷��̾ ���ݹ��� �ȿ� ������ ��
                if (distanceWithPlayer <= attackRange)
                {
                    print("CheckDistance, Attack");
                    state = State.Attack;
                    anim.SetTrigger("Idle");
                    nav.speed = 0;
                    nav.isStopped = true;
                    nav.updateRotation = false;
                }
                break;

            case State.Attack:

                // �÷��̾ ���ݹ����� ����� ��
                if(distanceWithPlayer > attackRange)
                {
                    state = State.Chase;
                    anim.SetTrigger("Move");
                    nav.speed = 3.5f;
                    nav.isStopped = false;
                    nav.updateRotation = true;
                }
                break;
        }
    }

    private void IsPlayerInSight()
    {
        Vector3 targetVector = player.position - transform.position;
        targetVector.Normalize();

        Vector3 forwardVector = transform.forward;

        float dot = Vector3.Dot(targetVector, forwardVector);

        if (dot >= Math.Cos(sightAngle * Mathf.Deg2Rad))
        {
            Debug.DrawLine(transform.position, player.position);
            isInSight = true;
            print("IsPlayerInsight, Chase");
            state = State.Chase;
            anim.SetTrigger("Walk");
            nav.speed = 3.5f;
            nav.isStopped = false;
            nav.updateRotation = true;
        }
    }

    private void Chase()
    {
        nav.SetDestination(player.position);
    }

    private void Attack()
    {
        if( weaponState == WeaponState.ScatterGun)
        {
            anim.SetBool("IsScatterGun", true);
        }
        else
        {
            anim.SetBool("IsScatterGun", false);
        }

        curTime += Time.deltaTime;
        if(curTime >= 3f)
        {
            anim.SetTrigger("Fire");
            Fire();
            curTime = 0f;
            print("scoutMove attack fire");
        }
    }

    private void Fire()
    {
        switch (weaponState )
        {
            case WeaponState.ScatterGun:
                scatterGun.Fire();
                break;

            case WeaponState.PistolGun:
                pistolGun.Fire();
                break;
        }
    }

    private void LookAtPlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir = new Vector3(dir.x, 0, dir.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10);
    }

    public void Die()
    {
        state = State.Die;
        anim.SetTrigger("Die");
    }
}



