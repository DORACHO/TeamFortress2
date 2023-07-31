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
    private float distanceWithHiller = 0f;
    private float distanceWithTarget = 0f;
    //private float distanceWithPlayer = 0f;
   // private bool isJumpKeyDown = false;
    private bool isArrived = false;
    private bool isInSight = true;                                           // �÷��̾ �þ� �ȿ� �ִ��� Ȯ��
    private bool isRotate = false;
    private bool isHillerNear = false;
    private bool isPlayerNear = false;
    private RaycastHit hit;
    private Transform target = null;
    private J_MedicHP medicHp;

    private readonly int maxJumpCount = 2;
    private readonly float jumpPower = 5f;
    private readonly float attackRate = 1.5f;
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
    //private Vector3 target = Vector3.zero;

    private Rigidbody rig;
    private NavMeshAgent nav;
    private Transform player;
    private Transform hiller;
    private PEA_ScatterGun scatterGun;
    private PEA_PistolGun pistolGun;
    private PEA_ScoutSound scoutSound;
    private Animator anim;

    // �����Ϳ��� �������� ������
    public PatrolPointByStep[] patrolStep;                                     // �������� ������ ������
    public Transform respawnPoint;
    #endregion

    public bool IsDead
    {
        get { return state == State.Die; }
    }

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
        medicHp = hiller.GetComponent<J_MedicHP>();
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

        if (medicHp.IsDead && target == hiller)
        {
            target = null;
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

        // ������ �����Ÿ��ȿ� ���Դ��� Ȯ��
        isHillerNear = distanceWithHiller <= sensingDistance ? true : false;

        if (medicHp.IsDead)
        {
            isHillerNear = false;
        }

        //�÷��̾ �����Ÿ� �ȿ� ���Դ��� Ȯ��
        isPlayerNear = distanceWithPlayer <= sensingDistance ? true : false;

        // ������ ������ �ְ� Ÿ���� �ƴ϶�� �þ߿� �ִ��� Ȯ��
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
                    // ������ �����Ÿ� �ȿ� ������ ������ Ÿ������ ��.(���� �켱 ó��)
                    //target = isHillerNear ? hiller : player;
                    IsInSight();
                }
                break;

            case State.Chase:

                // ������ �����Ÿ� �ȿ� �ְ� Ÿ���� ������ �ƴ϶�� ������ �þ߾ȿ� �ִ��� Ȯ��
                if(isHillerNear && target != hiller)
                {
                    IsInSight();
                }

                // ���� ���� Ÿ���� ���� ���� �ȿ� ������ ��
                if(distanceWithTarget <= attackRange)
                {
                    state = State.Attack;
                    anim.SetTrigger("Idle");
                    nav.speed = 0;
                    nav.isStopped = true;
                    nav.updateRotation = false;
                }

                // ���� ���� Ÿ���� �����Ÿ��� ����� ��
                else if( distanceWithTarget > sensingDistance)
                {
                    state = State.Idle;
                    anim.SetTrigger("Idle");
                    scoutSound.StopFootSound();
                    target = null;
                }
                break;

            case State.Attack:

                // ������ �����Ÿ� �ȿ� �ְ� Ÿ���� ������ �ƴ϶�� ������ �þ߾ȿ� �ִ��� Ȯ��
                if (isHillerNear && target != hiller)
                {
                    IsInSight();
                }

                // Ÿ���� ���ݹ����� ����� ��
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

        // ������ �����Ÿ� �ȿ� �ְ�, �þ߰� �ȿ� �ִٸ� Ÿ���� ����
        if (isHillerNear && dotHiller >= Math.Cos(sightAngle * Mathf.Deg2Rad))
        {
            Vector3 rayDir = hiller.position - transform.position;
            rayDir.Normalize();

            // �þ߰� �ȿ� �ְ�, �÷��̾�� Ÿ�� ���̿� �ٸ� ������Ʈ�� ���� ���� �ÿ� ���� ����
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

        // ������ Ÿ���� ���� �ʾҰ�, �÷��̾ �����Ÿ� �ȿ� �ְ�, �þ߰� �ȿ� �ִٸ� Ÿ���� �÷��̾�
        if (target == null && isPlayerNear && dotPlayer >= Math.Cos(sightAngle * Mathf.Deg2Rad))
        {
            Vector3 rayDir = player.position - transform.position;
            rayDir.Normalize();

            // �þ߰� �ȿ� �ְ�, �÷��̾�� Ÿ�� ���̿� �ٸ� ������Ʈ�� ���� ���� �ÿ� ���� ����
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
        if(curTime >= attackRate)
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



