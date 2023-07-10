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

    // ������
    #region
    private int curPatrolPoint = -1;                                         // ���� �������� ��������Ʈ
    private int prevPatrolPoint = -1;                                        // ������ �������� ��������Ʈ
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
    private bool isInSight = true;                                           // �÷��̾ �þ� �ȿ� �ִ��� Ȯ��

    private readonly int maxJumpCount = 2;
    private readonly float jumpPower = 5f;
    private readonly float forwardSpeed = 7.62f;
    private readonly float backwardSpeed = 6.86f;
    private readonly float sightAngle = 30f;                                 // �þ� ���� / 2(�չ����� �������� ����ҰŶ� 2�� ����) 
    private readonly float attackRange = 10f;                                // ���� �ݰ�
    private readonly float maxDistanceWithPlayer = 5f;                       // �÷��̾�� ������ �ִ� �Ÿ�

    // ������ ���õ� ������
    private readonly float minWaitTime = 1f;                                 // �������� ���� �� �����ִ� �ּҽð�
    private readonly float maxWaitTime = 5f;                                 // �������� ���� �� �����ִ� �ִ�ð�

    // �þ߰��� ���õ� ������
    List<Collider> hitTargetList = new List<Collider>();                     // �þ߰� �ȿ� ���� ������Ʈ���� �ݶ��̴��� ���� ����Ʈ

    private Vector3 dir = Vector3.zero;
    private Vector3 target = Vector3.zero;

    private Rigidbody rig = null;
    private NavMeshAgent nav;
    private Transform player;
    private PEA_ScatterGun scatterGun;
    private PEA_PistolGun pistolGun;

    // �����Ϳ��� �������� ������
    public Transform[] patrolPoints;                                       // �������� ������ ������
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
                CheackDistance();
                break;

            case State.Damage:
                break;

            case State.Die:
                break;
        }
    }

    Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Debug.DrawRay(transform.position, AngleToDir(30f) * attackRange, Color.blue);
        Debug.DrawRay(transform.position, AngleToDir(-30f) * attackRange, Color.blue);
        //Debug.DrawRay(transform.position, player.position - transform.position.normalized, Color.yellow);
        Debug.DrawRay(transform.position, transform.forward * attackRange, Color.cyan);

        hitTargetList.Clear();
        Collider[] Targets = Physics.OverlapSphere(transform.position, attackRange, LayerMask.NameToLayer("Player"));

        if (Targets.Length == 0) return;
        foreach (Collider EnemyColli in Targets)
        {
            Vector3 targetPos = EnemyColli.transform.position;
            Vector3 targetDir = (targetPos - transform.position).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(transform.forward, targetDir)) * Mathf.Rad2Deg;
            if (targetAngle <= attackRange * 0.5f && !Physics.Raycast(transform.position, targetDir, attackRange, LayerMask.NameToLayer("Obstacle")))
            {
                hitTargetList.Add(EnemyColli);
                Debug.DrawLine(transform.position, targetPos, Color.red);
            }
        }
    }

    private void GetInputValues()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        dir = new Vector3(x, 0, z).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(jumpCount < maxJumpCount)
            {
                isJumpKeyDown = true;
            }
        }
    }

    private void Move()
    {
        if(z >= 0)
        {
            speed = forwardSpeed;
            if(z == 0 && x == 0)
            {
                speed = 0f;
            }
        }
        else if(z < 0)
        {
            speed = backwardSpeed;
        }

        transform.Translate(dir * speed * Time.deltaTime);

        if (isJumpKeyDown && jumpCount < maxJumpCount)
        {
            rig.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            jumpCount++;
            isJumpKeyDown = false;
        }
    }

    private void Patrol()
    {
        if(isArrived)
        {
            // �������� �����ϸ� ��� ���߱�
            SetRandomWaitTime();
            state = State.Idle;
        }
        else
        {
            CheckIsArrived();
        }
    }

    // �������� �̵��� ��ġ ���ϱ�
    private void SetRandomTarget()
    {
        // ���������� ���� �Ÿ��� 0.2f ������ �� ������ �缳��
        //if(nav.remainingDistance <= 0.5f)
        //{
            x = Random.Range(player.position.x - maxDistanceWithPlayer, player.position.x + maxDistanceWithPlayer);
            z = Random.Range(player.position.z - maxDistanceWithPlayer, player.position.z + maxDistanceWithPlayer);
            target = new Vector3(x, transform.position.y, z);
            nav.SetDestination(target);
        //}
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
            state = State.Idle;
            SetRandomWaitTime();
        }
    }

    private void IsPlayerInSight()
    {
        print("check insight");
        Vector3 dirToPlayer = player.position - transform.position.normalized;

        if(Vector3.Angle(transform.forward, dirToPlayer) <= 30)
        {
            print(Vector3.Angle(transform.forward, dirToPlayer));
            print("insight");
            isInSight = true;
            nav.isStopped = true;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }
}
