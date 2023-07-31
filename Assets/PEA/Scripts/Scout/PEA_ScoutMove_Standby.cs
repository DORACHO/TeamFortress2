using System;   //Serializable을 사용하기 위해 적어줌
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

    // 변수들
    #region
    private float curTime = 0f;
    private float distanceWithPlayer = 0f;
    private float distanceWithHiller = 0f;
    private float distanceWithTarget;
    private bool isInSight = true;                                           // 플레이어가 시야 안에 있는지 확인
    private bool isHillerNear = false;
    private bool isPlayerNear = false;

    private readonly float attackRate = 1.5f;
    private readonly float sightAngle = 60f;                                 // 시야 각도 / 2(앞방향을 기준으로 계산할거라 2로 나눔) 
    private readonly float attackRange = 10f;                                // 공격 반경
    private readonly float sensingDistance = 20f;

    private NavMeshAgent nav;
    private Transform player;
    private Transform hiller;
    private Transform target;
    private PEA_ScatterGun scatterGun;
    private PEA_PistolGun pistolGun;
    private PEA_ScoutSound scoutSound;
    private Animator anim;
    private RaycastHit hit;
    private J_MedicHP medicHp;
    #endregion

    public bool IsDead
    {
        get { return state == State.Die;  }
    }


    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        scatterGun = GetComponentInChildren<PEA_ScatterGun>();
        pistolGun = GetComponentInChildren<PEA_PistolGun>();
        scoutSound = GetComponent<PEA_ScoutSound>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        hiller = GameObject.FindGameObjectWithTag("Hiller").transform;
        medicHp = hiller.GetComponent<J_MedicHP>();
    }

    // Update is called once per frame
    void Update()
    {
        print(state);
        print(target);
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
                LookAtTarget();
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
        distanceWithHiller = Vector3.Distance(transform.position, hiller.position);

        if (target != null)
        {
            distanceWithTarget = Vector3.Distance(transform.position, target.position);
        }

        // 힐러가 감지거리안에 들어왔는지 확인
        isHillerNear = distanceWithHiller <= sensingDistance ? true : false;

        if (medicHp.IsDead)
        {
            isHillerNear = false;
        }

        //플레이어가 감지거리 안에 들어왔는지 확인
        isPlayerNear = distanceWithPlayer <= sensingDistance ? true : false;

        // 힐러가 가까이 있고 타겟이 아니라면 시야에 있는지 확인
        if (isHillerNear && target != hiller)
        {
            IsInSight();
        }

        switch (state)
        {
            case State.Standby:

                if (isHillerNear || isPlayerNear)
                {
                    // 힐러가 감지거리 안에 있으면 힐러를 타겟으로 함.(힐러 우선 처리)
                    //target = isHillerNear ? hiller : player;
                    IsInSight();
                }
                break;

            case State.Chase:

                // 힐러가 감지거리 안에 있고 타겟이 힐러가 아니라면 힐러가 시야안에 있는지 확인
                if (isHillerNear && target != hiller)
                {
                    IsInSight();
                }

                // 추적 중인 타겟이 공격 범위 안에 들어왔을 때
                if (distanceWithTarget <= attackRange)
                {
                    print("chase - > attack");
                    state = State.Attack;
                    anim.SetTrigger("Idle");
                    nav.speed = 0;
                    nav.isStopped = true;
                    nav.updateRotation = false;
                }

                // 추적 중인 타겟이 감지거리를 벗어났을 때
                else if (distanceWithTarget > sensingDistance)
                {
                    state = State.Standby;
                    anim.SetTrigger("Idle");
                    target = null;
                    scoutSound.StopFootSound();
                }
                break;

            case State.Attack:

                // 힐러가 감지거리 안에 있고 타겟이 힐러가 아니라면 힐러가 시야안에 있는지 확인
                if (isHillerNear && target != hiller)
                {
                    IsInSight();
                }

                // 타겟이 공격범위를 벗어낫을 때
                if (distanceWithTarget > attackRange)
                {
                    print("attack -> chase");
                    state = State.Chase;
                    anim.SetTrigger("Walk");
                    scoutSound.PlayFootSound();
                    nav.speed = 3.5f;
                    nav.isStopped = false;
                    nav.updateRotation = true;
                }
                break;
        }
    }

    private void IsInSight()
    {
        Vector3 playerDirVector = player.position - transform.position;
        Vector3 hillerDirVector = hiller.position - transform.position;
        playerDirVector.Normalize();
        hillerDirVector.Normalize();

        Vector3 forwardVector = transform.forward;

        float dotPlayer = Vector3.Dot(playerDirVector, forwardVector);
        float dotHiller = Vector3.Dot(hillerDirVector, forwardVector);

        // 힐러가 감지거리 안에 있고, 시야각 안에 있다면 타겟은 힐러
        if (isHillerNear && dotHiller >= Math.Cos(sightAngle * Mathf.Deg2Rad))
        {
            Vector3 rayDir = hiller.position - transform.position;
            rayDir.Normalize();

            // 시야각 안에 있고, 플레이어와 타겟 사이에 다른 오브젝트가 있지 않을 시에 추적 시작
            if (Physics.Raycast(transform.position, rayDir, out hit, sensingDistance) && hit.transform == hiller)
            {
                target = hiller;
                Debug.DrawLine(transform.position, target.position);
                isInSight = true;
                state = State.Chase;
                anim.SetTrigger("Walk");
                scoutSound.PlayFootSound();
                nav.speed = 3.5f;
                nav.isStopped = false;
                nav.updateRotation = true;
            }
        }

        // 힐러가 타겟이 되지 않았고, 플레이어가 감지거리 안에 있고, 시야각 안에 있다면 타겟은 플레이어
        if (target == null && isPlayerNear && dotPlayer >= Math.Cos(sightAngle * Mathf.Deg2Rad))
        {
            Vector3 rayDir = player.position - transform.position;
            rayDir.Normalize();

            // 시야각 안에 있고, 플레이어와 타겟 사이에 다른 오브젝트가 있지 않을 시에 추적 시작
            if (Physics.Raycast(transform.position, rayDir, out hit, sensingDistance) && hit.transform == player)
            {
                target = player;
                Debug.DrawLine(transform.position, target.position);
                isInSight = true;
                state = State.Chase;
                anim.SetTrigger("Walk");
                scoutSound.PlayFootSound();
                nav.speed = 3.5f;
                nav.isStopped = false;
                nav.updateRotation = true;
            }
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
        if(curTime >= attackRate)
        {
            Fire();
            curTime = 0f;
        }
    }

    private void Fire()
    {
        anim.SetTrigger("Fire");
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

    private void LookAtTarget()
    {
        Vector3 dir = target.position - transform.position;
        dir = new Vector3(dir.x, 0, dir.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10);
    }

    public void Die()
    {
        state = State.Die;
        anim.SetTrigger("Die");
        scoutSound.Die();
        GameManager.instance.RedKillBlue();
    }
}



