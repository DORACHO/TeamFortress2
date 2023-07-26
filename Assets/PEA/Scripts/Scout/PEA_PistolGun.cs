using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_PistolGun : MonoBehaviour
{
    private enum State 
    {
        Loading,
        Fire,
        Wait
    }

    private State state = State.Wait;


    private int randomCritical = 0;                                 
    private int loadBullets = 12;                                   // 장전되어 있는 총알 개수
    private int numberOfAmmunition = 36;                            // 소지하고 있는 총알 개수(장전되지 않은)
    private float x = 0;                                            // 반동 회복에 사용할 x축 회전값 변수
    private float damage = 0f;
    private float timeAfterFire = 0f;                               // 총알 발사 후 지난 시간
    private float curTime = 0f;                                     // 총알 장전시간, 반동 회복 시간에 사용할 시간 변수
    private float sumReboundAngle = 0f;                             // 총 반동 각도
    private bool isLoading = false;                                 // 현재 장전중인지 확인
    private bool isCritical = false;                                // 이번 공격이 치명타를 입히는 공격인지 확인
    private bool isRebounding = false;                              // 반동을 회복했는지 확인(true일 때 반동 회복중)

    //공격
    private readonly float attackInterval = 0.15f;                  // 공격 간격

    // 치명타 
    private readonly int criticalPercent = 2;                       // 치명타 발생률

    // 거리별 데미지값
    private readonly float baseDamage = 15f;                        // 기본 데미지 (중거리 데미지)
    private readonly float maximunDamage = 22f;                     // 근거리 데미지
    private readonly float minimumDamage = 8f;                      // 장거리 데미지
    private readonly float criticalDamage = 45f;                    // 치명타 데미지

    // 총알 장전 
    private readonly int maxLoadBulletCount = 12;                   // 한 번에 장전 가능한 총알 개수
    private readonly float loadingTime = 1.25f;                     // 장전 시간


    // 근거리 중거리 장거리 기준
    private readonly float closeRange = 3f;
    private readonly float middleRange = 5f;
    private readonly float longRange = 10f;

    // 반동
    private readonly float reboundAngle = -10f;                     // 반동으로 총올라갈 총의 각도
    private readonly float reboundRecoveryTime = 0.1f;              // 반동 회복 시간

    // 레이캐스트 관련 변수
    private RaycastHit hit;
    private readonly float rayDistance = 10f;                       // 공격이 닿을 수 있는 최대 거리

    // 소리
    private PEA_ScoutSound scoutSound;

    // 에디터에서 연결해줄 변수
    public Transform firePos;

    // Start is called before the first frame update
    void Start()
    {
        scoutSound = GetComponentInParent<PEA_ScoutSound>();
    }

    // Update is called once per frame
    void Update()
    {
        if (loadBullets == 0 && maxLoadBulletCount != 0)
        {
            state = State.Loading;
            scoutSound.PistolReload();
        }

        if(state == State.Loading)
        {
            ReLoad();
        }
    }

    // 거리별 데미지 측정(Fire에서 호출)
    public void SetDamage(float distance, bool isCritical)
    {
        if (isCritical)
        {
            damage += criticalDamage;
        }
        else
        {
            if (distance <= closeRange)
            {
                damage += maximunDamage;
            }
            else if (distance <= middleRange)
            {
                damage += baseDamage;
            }
            else
            {
                damage += minimumDamage;
            }
        }
    }

    // 재장전
    private void ReLoad()
    {
        print("Redloading");
        curTime += Time.deltaTime;

        if (curTime >= loadingTime)
        {
            numberOfAmmunition = numberOfAmmunition - (maxLoadBulletCount - loadBullets);
            loadBullets = maxLoadBulletCount;
            curTime = 0f;
            state = State.Wait;
            print("ReLoaded");
        }
    }

    // 데미지 출력(Fire에서 호출)
    private void ShowDamage()
    {
        print(damage);
    }

    // 총 쏘기
    public void Fire()
    {
        // 이번 공격으로 치명타를 입힐지 랜덤으로 정한다
        randomCritical = Random.Range(1, 100);
        if (randomCritical <= criticalPercent)
        {
            isCritical = true;
        }
        else
        {
            isCritical = false;
        }

        // 총구의 앞방향으로 레이를 쏘고, 거리에 따라 데미지 측정
        scoutSound.PistolShoot();
        Debug.DrawRay(firePos.position, firePos.forward * rayDistance, Color.red);
        if (Physics.Raycast(firePos.position, firePos.forward, out hit, rayDistance))
        {
            SetDamage(Vector3.Distance(transform.position, hit.point), isCritical);
            EJPSHP.instance.SetHP((int)damage, transform.position);
            //플레이어가 맞으면 플레이어한테 데미지 주기
        }

        // 반동주기, 측정된 데미지 출력, 장전된 총알 하나씩 빼기, 사용된 변수들 초기화해주기
        //Rebound();
        ShowDamage();
        loadBullets--;
        curTime = 0f;
        damage = 0;
        //timeAfterFire = 0f;

        print("load Bullet : " + loadBullets);
        print("number Of Ammunition : " + numberOfAmmunition);
    }

    private void Rebound()
    {
        transform.localEulerAngles += new Vector3(reboundAngle, 0, 0);
        sumReboundAngle = transform.eulerAngles.x - 360f;
        x = sumReboundAngle;
        isRebounding = true;
    }

    private void ReboundRecovery()
    {
        x -= sumReboundAngle * (Time.deltaTime / reboundRecoveryTime);
        transform.localEulerAngles = new Vector3(x, 0, 0);
        if (x >= 0)
        {
            transform.localEulerAngles = Vector3.zero;
            isRebounding = false;
        }
    }
}
