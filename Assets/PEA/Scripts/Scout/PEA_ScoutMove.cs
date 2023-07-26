using System;   //Serializable�� ����ϱ� ���� ������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// 2���� �迭�� �ν�����â�� �������� �ʱ� ������ �迭�� ���� Ŭ������ �̿��� �������� �迭�� ����� Serializable�� �ؼ� �ν����� â���� �� �� �ֵ��� ��
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
        Move,
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

    // ������
    #region
    private int curPatrolPoint = -1;                                         // ���� �������� ��������Ʈ
    private int prevPatrolPoint = -1;                                        // ������ �������� ��������Ʈ
    private int jumpCount = 0;
    private int wagonPoint = 0;                                              // ������ ������ üũ����Ʈ ��ȣ
    private float speed = 0f;
    private float curTime = 0f;
    private float waitTime = 0f;
    private float distanceWithPlayer = 0f;
    //private float distanceWithPlayer = 0f;
   // private bool isJumpKeyDown = false;
    private bool isArrived = false;
    private bool isInSight = true;                                           // �÷��̾ �þ� �ȿ� �ִ��� Ȯ��
    private bool isRotate = false;

    private readonly int maxJumpCount = 2;
    private readonly float jumpPower = 5f;
    private readonly float sightAngle = 60f;                                 // �þ� ���� / 2(�չ����� �������� ����ҰŶ� 2�� ����) 
    private readonly float attackRange = 10f;                                // ���� �ݰ�
    private readonly float respawnTime = 3f; 
    private readonly float forwardSpeed = 7.62f;
    private readonly float backwardSpeed = 6.86f;
    private readonly float sensingDistance = 20f;

    // ������ ���õ� ������
    private readonly float minWaitTime = 1f;                                 // �������� ���� �� �����ִ� �ּҽð�
    private readonly float maxWaitTime = 5f;                                 // �������� ���� �� �����ִ� �ִ�ð�

    // �þ߰��� ���õ� ������
    List<Collider> hitTargetList = new List<Collider>();                     // �þ߰� �ȿ� ���� ������Ʈ���� �ݶ��̴��� ���� ����Ʈ

    private Vector3 dir = Vector3.zero;
    private Vector3 target = Vector3.zero;

    private Rigidbody rig;
    private NavMeshAgent nav;
    private Transform player;
    private PEA_ScatterGun scatterGun;
    private PEA_PistolGun pistolGun;
    private Animator anim;

    // �����Ϳ��� �������� ������
    public PatrolPointByStep[] patrolStep;                                     // �������� ������ ������
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
            // �������� �����ϸ� ��� ���߱�
            SetRandomWaitTime();
            print("Patrol, Idle");
            state = State.Idle;
            anim.SetTrigger("Idle");
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


        switch (state)
        {
            case State.Idle:
            case State.Move:
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

                // �÷��̾ �����Ÿ��� ����� ��
                else if( distanceWithPlayer > sensingDistance)
                {
                    state = State.Idle;
                    anim.SetTrigger("Idle");
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
        /*
        print("check insight");
        Vector3 playerPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        Vector3 dirToPlayer = playerPos - transform.position.normalized;

        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        //print(angle);

        // �þ߰� �ȿ� �÷��̾ ���� ��
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
        nav.SetDestination(player.position);
    }

    private void Attack()
    {
        //transform.LookAt(player);
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

    private void Wait()
    {
        curTime += Time.deltaTime;
        if (curTime >= waitTime)
        {
            curTime = 0f;
            //SetRandomTarget();
            SetRandomPatrolPoint();
            print("Wait, Move");
            state = State.Move;
            anim.SetTrigger("Walk");
            nav.speed = 3.5f;
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

        Vector3 dir = player.position - transform.position;
        dir = new Vector3(dir.x, 0, dir.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10);
    }

    // ������ üũ����Ʈ�� �������� �� ȣ��
    // �������� ����Ʈ���� �޶���
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }
}



