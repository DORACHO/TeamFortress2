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
    private float distanceWithHiller = 0f;
    private float distanceWithTarget;
    private bool isInSight = true;                                           // �÷��̾ �þ� �ȿ� �ִ��� Ȯ��
    private bool isHillerNear = false;
    private bool isPlayerNear = false;

    private readonly float attackRate = 1.5f;
    private readonly float sightAngle = 60f;                                 // �þ� ���� / 2(�չ����� �������� ����ҰŶ� 2�� ����) 
    private readonly float attackRange = 10f;                                // ���� �ݰ�
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

        // ������ �����Ÿ��ȿ� ���Դ��� Ȯ��
        isHillerNear = distanceWithHiller <= sensingDistance ? true : false;

        if (medicHp.IsDead)
        {
            isHillerNear = false;
        }

        //�÷��̾ �����Ÿ� �ȿ� ���Դ��� Ȯ��
        isPlayerNear = distanceWithPlayer <= sensingDistance ? true : false;

        // ������ ������ �ְ� Ÿ���� �ƴ϶�� �þ߿� �ִ��� Ȯ��
        if (isHillerNear && target != hiller)
        {
            IsInSight();
        }

        switch (state)
        {
            case State.Standby:

                if (isHillerNear || isPlayerNear)
                {
                    // ������ �����Ÿ� �ȿ� ������ ������ Ÿ������ ��.(���� �켱 ó��)
                    //target = isHillerNear ? hiller : player;
                    IsInSight();
                }
                break;

            case State.Chase:

                // ������ �����Ÿ� �ȿ� �ְ� Ÿ���� ������ �ƴ϶�� ������ �þ߾ȿ� �ִ��� Ȯ��
                if (isHillerNear && target != hiller)
                {
                    IsInSight();
                }

                // ���� ���� Ÿ���� ���� ���� �ȿ� ������ ��
                if (distanceWithTarget <= attackRange)
                {
                    print("chase - > attack");
                    state = State.Attack;
                    anim.SetTrigger("Idle");
                    nav.speed = 0;
                    nav.isStopped = true;
                    nav.updateRotation = false;
                }

                // ���� ���� Ÿ���� �����Ÿ��� ����� ��
                else if (distanceWithTarget > sensingDistance)
                {
                    state = State.Standby;
                    anim.SetTrigger("Idle");
                    target = null;
                    scoutSound.StopFootSound();
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



