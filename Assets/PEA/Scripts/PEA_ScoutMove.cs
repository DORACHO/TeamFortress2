using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PEA_ScoutMove : MonoBehaviour
{
    private enum State 
    {
        Idle,
        Move,
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
    private float x = 0f;
    private float z = 0f;
    private float speed = 0f;
    private float curTime = 0f;
    private float waitTime = 0f;
    private float distanceWithPlayer = 0f;
    //private float distanceWithPlayer = 0f;
    private bool isJumpKeyDown = false;
    private bool isArrived = false;
    private bool isInSight = true;                                           // 플레이어가 시야 안에 있는지 확인
    private bool isRotate = false;

    private readonly int maxJumpCount = 2;
    private readonly float jumpPower = 5f;
    private readonly float forwardSpeed = 7.62f;
    private readonly float backwardSpeed = 6.86f;
    private readonly float sightAngle = 30f;                                 // 시야 각도 / 2(앞방향을 기준으로 계산할거라 2로 나눔) 
    private readonly float attackRange = 10f;                                // 공격 반경
    private readonly float maxDistanceWithPlayer = 5f;                       // 플레이어와 유지할 최대 거리

    // 순찰과 관련된 변수들
    private readonly float minWaitTime = 1f;                                 // 목적지에 도착 후 멈춰있는 최소시간
    private readonly float maxWaitTime = 5f;                                 // 목적지에 도착 후 멈춰있는 최대시간

    // 시야각과 관련된 변수들
    List<Collider> hitTargetList = new List<Collider>();                     // 시야각 안에 들어온 오브젝트들의 콜라이더를 담을 리스트

    private Vector3 dir = Vector3.zero;
    private Vector3 target = Vector3.zero;

    private Rigidbody rig = null;
    private NavMeshAgent nav;
    private Transform player;
    private PEA_ScatterGun scatterGun;
    private PEA_PistolGun pistolGun;

    // 에디터에서 연결해줄 변수들
    public Transform[] patrolPoints;                                       // 랜덤으로 순찰할 지점들
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
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
            case State.Idle:
                Wait();
                CheackDistance();
                break;

            case State.Move:
                Patrol();
                CheackDistance();
                break;

            case State.Attack:
                Attack();
                if (isRotate)
                {
                    LookAtPlayer();
                }
                CheackDistance();
                break;

            case State.Damage:
                break;

            case State.Die:
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
            state = State.Idle;
        }
        else
        {
            CheckIsArrived();
        }
    }

    private void SetRandomWaitTime()
    {
        waitTime = Random.Range(minWaitTime, maxWaitTime);
        isArrived = false;
    }

    private void SetRandomPatrolPoint()
    {
        prevPatrolPoint = curPatrolPoint;
        curPatrolPoint = Random.Range(0, patrolPoints.Length);
        if(curPatrolPoint == prevPatrolPoint)
        {
            curPatrolPoint++ ;
            if(curPatrolPoint >= patrolPoints.Length)
            {
                curPatrolPoint = 0;
            }
        }
        nav.SetDestination(patrolPoints[curPatrolPoint].position);
    }

    private void CheckIsArrived()
    {
        if (nav.remainingDistance <= 0.3f)
        {
            isArrived = true;
        }
    }

    private void CheackDistance()
    {
        distanceWithPlayer = Vector3.Distance(transform.position, player.position);
        if(distanceWithPlayer <= attackRange)
        {
            IsPlayerInSight();
        }
        else if(state == State.Attack)
        {
            SetRandomWaitTime();
            state = State.Idle;
        }
    }

    private void IsPlayerInSight()
    {
        print("check insight");
        Vector3 dirToPlayer = player.position - transform.position.normalized;

        print(Vector3.Angle(transform.forward, dirToPlayer));
        if(Vector3.Angle(transform.forward, dirToPlayer) <= 30)
        {
            print("insight");
            isInSight = true;
            nav.isStopped = true;
            nav.updateRotation = false;
            state = State.Attack;
        }
    }

    private void Attack()
    {
        transform.LookAt(player);
        curTime += Time.deltaTime;
        if(curTime >= 3f)
        {
            Fire();
            curTime = 0f;
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
            state = State.Move;
            nav.isStopped = false;
            nav.updateRotation = true;
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

        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, dir, 10 * Time.deltaTime);

        if(transform.localEulerAngles.normalized.y - dir.y <= 1f)
        {
            transform.localEulerAngles = dir;
            isRotate = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }
}
