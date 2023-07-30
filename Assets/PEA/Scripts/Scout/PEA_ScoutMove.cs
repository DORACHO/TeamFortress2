using System;   //Serializable을 사용하기 위해 적어줌
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// 2차원 배열은 인스펙터창에 보여지지 않기 때문에 배열을 담은 클래스를 이용해 이중으로 배열을 만들고 Serializable을 해서 인스펙터 창에서 볼 수 있도록 함
[Serializable]
public class PatrolPointByStep
{
    public Transform[] patrolPoints;
}

public class PEA_ScoutMove : MonoBehaviour
{
    private enum State 
    {
        Idle,
        Patrol,
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

    private State state = State.Idle;
    private WeaponState weaponState = WeaponState.PistolGun;

    // 변수들
    #region
    private int curPatrolPoint = -1;                                         // 현재 순찰가는 순찰포인트
    private int prevPatrolPoint = -1;                                        // 이전에 순찰갔던 순찰포인트
    private int jumpCount = 0;
    private int wagonPoint = 0;                                              // 수레가 지나간 체크포인트 번호
    private float speed = 0f;
    private float curTime = 0f;
    private float waitTime = 0f;
    private float distanceWithPlayer = 0f;
    private float distanceWithHiller = 0f;
    private float distanceWithTarget = 0f;
    //private float distanceWithPlayer = 0f;
   // private bool isJumpKeyDown = false;
    private bool isArrived = false;
    private bool isInSight = true;                                           // 플레이어가 시야 안에 있는지 확인
    private bool isRotate = false;
    private bool isHillerNear = false;
    private bool isPlayerNear = false;
    private RaycastHit hit;
    private Transform target = null;

    private readonly int maxJumpCount = 2;
    private readonly float jumpPower = 5f;
    private readonly float sightAngle = 60f;                                 // 시야 각도 / 2(앞방향을 기준으로 계산할거라 2로 나눔) 
    private readonly float attackRange = 10f;                                // 공격 반경
    private readonly float respawnTime = 3f; 
    private readonly float forwardSpeed = 7.62f;
    private readonly float backwardSpeed = 6.86f;
    private readonly float sensingDistance = 20f;

    // 순찰과 관련된 변수들
    private readonly float minWaitTime = 1f;                                 // 목적지에 도착 후 멈춰있는 최소시간
    private readonly float maxWaitTime = 5f;                                 // 목적지에 도착 후 멈춰있는 최대시간

    // 시야각과 관련된 변수들
    List<Collider> hitTargetList = new List<Collider>();                     // 시야각 안에 들어온 오브젝트들의 콜라이더를 담을 리스트

    private Vector3 dir = Vector3.zero;
    //private Vector3 target = Vector3.zero;

    private Rigidbody rig;
    private NavMeshAgent nav;
    private Transform player;
    private Transform hiller;
    private PEA_ScatterGun scatterGun;
    private PEA_PistolGun pistolGun;
    private PEA_ScoutSound scoutSound;
    private Animator anim;

    // 에디터에서 연결해줄 변수들
    public PatrolPointByStep[] patrolStep;                                     // 랜덤으로 순찰할 지점들
    public Transform respawnPoint;
    #endregion


    public int WagonPoint
    {
        get { return wagonPoint; }
    }

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        scoutSound = GetComponent<PEA_ScoutSound>();
        pistolGun = GetComponentInChildren<PEA_PistolGun>();
        scatterGun = GetComponentInChildren<PEA_ScatterGun>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        hiller = GameObject.FindGameObjectWithTag("Hiller").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //print(state);
        switch (state)
        {
            case State.Idle:
                Wait();
                CheackDistance();
                break;

            case State.Patrol:
                Patrol();
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
                Respawn();
                break;
        }
    }

    //Vector3 AngleToDir(float angle)
    //{
    //    float radian = angle * Mathf.Deg2Rad;
    //    return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //    Debug.DrawRay(transform.position, transform.forward + AngleToDir(30f) * attackRange, Color.blue);
    //    Debug.DrawRay(transform.position, transform.forward + AngleToDir(-30f) * attackRange, Color.blue);
    //    //Debug.DrawRay(transform.position, player.position - transform.position.normalized, Color.yellow);
    //    Debug.DrawRay(transform.position, transform.forward * attackRange, Color.cyan);

    //    hitTargetList.Clear();
    //    Collider[] Targets = Physics.OverlapSphere(transform.position, attackRange, LayerMask.NameToLayer("Player"));

    //    if (Targets.Length == 0) return;
    //    foreach (Collider EnemyColli in Targets)
    //    {
    //        Vector3 targetPos = EnemyColli.transform.position;
    //        Vector3 targetDir = (targetPos - transform.position).normalized;
    //        float targetAngle = Mathf.Acos(Vector3.Dot(transform.forward, targetDir)) * Mathf.Rad2Deg;
    //        if (targetAngle <= attackRange * 0.5f && !Physics.Raycast(transform.position, targetDir, attackRange, LayerMask.NameToLayer("Obstacle")))
    //        {
    //            hitTargetList.Add(EnemyColli);
    //            Debug.DrawLine(transform.position, targetPos, Color.red);
    //        }
    //    }
    //}

    private void Patrol()
    {
        if(isArrived)
        {
            // 목적지에 도착하면 잠시 멈추기
            SetRandomWaitTime();
            print("Patrol, Idle");
            state = State.Idle;
            anim.SetTrigger("Idle");
            scoutSound.StopFootSound();
            nav.isStopped = true;
        }
        else
        {
            CheckIsArrived();
        }
    }

    private void SetRandomWaitTime()
    {
        waitTime = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
        isArrived = false;
    }

    private void SetRandomPatrolPoint()
    {
        prevPatrolPoint = curPatrolPoint;
        curPatrolPoint = UnityEngine.Random.Range(0, patrolStep[wagonPoint].patrolPoints.Length);
        if(curPatrolPoint == prevPatrolPoint)
        {
            curPatrolPoint++ ;
            if(curPatrolPoint >= patrolStep[wagonPoint].patrolPoints.Length)
            {
                curPatrolPoint = 0;
            }
        }
        nav.SetDestination(patrolStep[wagonPoint].patrolPoints[curPatrolPoint].position);
        print(nav.destination);
    }

    private void CheckIsArrived()
    {
        float remainingDistance = Vector3.Distance( nav.destination, transform.position);
        if (remainingDistance <= 1f)
        {
            isArrived = true;
        }
    }

    private void CheackDistance()
    {
        distanceWithPlayer = Vector3.Distance(transform.position, player.position);
        distanceWithHiller = Vector3.Distance(transform.position, hiller.position);

        if(target != null)
        {
            distanceWithTarget = Vector3.Distance(transform.position, target.position);
        }

        // 힐러가 감지거리안에 들어왔는지 확인
        isHillerNear = distanceWithHiller <= sensingDistance ? true : false;

        //플레이어가 감지거리 안에 들어왔는지 확인
        isPlayerNear = distanceWithPlayer <= sensingDistance ? true : false;

        // 힐러가 가까이 있고 타겟이 아니라면 시야에 있는지 확인
        if(isHillerNear && target != hiller)
        {
            IsInSight();
        }

        switch (state)
        {
            case State.Idle:
            case State.Patrol:

                if(isHillerNear || isPlayerNear)
                {
                    // 힐러가 감지거리 안에 있으면 힐러를 타겟으로 함.(힐러 우선 처리)
                    //target = isHillerNear ? hiller : player;
                    IsInSight();
                }
                break;

            case State.Chase:

                // 힐러가 감지거리 안에 있고 타겟이 힐러가 아니라면 힐러가 시야안에 있는지 확인
                if(isHillerNear && target != hiller)
                {
                    IsInSight();
                }

                // 추적 중인 타겟이 공격 범위 안에 들어왔을 때
                if(distanceWithTarget <= attackRange)
                {
                    state = State.Attack;
                    anim.SetTrigger("Idle");
                    nav.speed = 0;
                    nav.isStopped = true;
                    nav.updateRotation = false;
                }

                // 추적 중인 타갯이 감지거리를 벗어났을 때
                else if( distanceWithTarget > sensingDistance)
                {
                    state = State.Idle;
                    anim.SetTrigger("Idle");
                    scoutSound.StopFootSound();
                    target = null;
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
        /*
        print("check insight");
        Vector3 playerPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        Vector3 dirToPlayer = playerPos - transform.position.normalized;

        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        //print(angle);

        // 시야각 안에 플레이어가 있을 때
        if(angle <= sightAngle)
        {
            Debug.DrawLine(transform.position, player.position);
            print("insight");
            isInSight = true;
            //nav.isStopped = true;
            //nav.updateRotation = false;
            print("IsPlayerInsight, Chase");
            state = State.Chase;
            anim.SetTrigger("Walk");
            nav.isStopped = false;
            nav.updateRotation = true;
        }
        else
        {
            isInSight = false;
        }
        */
    }

    private void Chase()
    {
        nav.SetDestination(target.position);
        print(target.gameObject.name);
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
            Fire();
            curTime = 0f;
            print("scoutMove attack fire");
        }
    }

    private void Wait()
    {
        curTime += Time.deltaTime;
        if (curTime >= waitTime)
        {
            curTime = 0f;
            //SetRandomTarget();
            SetRandomPatrolPoint();
            print("Wait, Move");
            state = State.Patrol;
            anim.SetTrigger("Walk");
            scoutSound.PlayFootSound();
            nav.speed = 3.5f;
            nav.isStopped = false;
            nav.updateRotation = true;
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
        //print("LookatPlayer");
        //Vector3 playerPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        //Vector3 dir = playerPos - transform.position;
        //dir.Normalize();

        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3( player.position.x, transform.position.y, player.position.z)), 10 * Time.deltaTime);
        //transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, dir, 10 * Time.deltaTime);

        //if(transform.localEulerAngles.y - dir.y <= 1f)
        //{
        //    transform.localEulerAngles = dir;
        //}

        Vector3 dir = target.position - transform.position;
        dir = new Vector3(dir.x, 0, dir.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10);
    }

    // 수레가 체크포인트를 지나갔을 때 호출
    // 순찰도는 포인트들이 달라짐
    public void PassByCheckPoint()
    {
        wagonPoint++;
    }

    private void Respawn()
    {
        curTime += Time.deltaTime;
        if(curTime <= respawnTime)
        {
            transform.position = respawnPoint.position;
            respawnPoint.eulerAngles = respawnPoint.eulerAngles;
            print("Respawn, Idle");
            state = State.Idle;
            anim.SetTrigger("Idle");
            scoutSound.StopFootSound();
            nav.speed = 0f;
            nav.isStopped = true;
            transform.position = respawnPoint.position;
            respawnPoint.rotation = respawnPoint.rotation;
        }
    }

    public void Die()
    {
        state = State.Die;
        anim.SetTrigger("Die");
        scoutSound.Die();
        GameManager.instance.RedKillBlue();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }
}



