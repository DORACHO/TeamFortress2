using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_ScatterGun : MonoBehaviour
{
    private enum State
    {
        Loading,
        Fire,
        Wait
    }

    private State state = State.Wait;

    private int randomCritical = 0;                                 // 크리티컬인지 아닌지 결정할 랜덤 변수
    private int loadBullets = 6;                                    // 장전되어 있는 총알 개수
    private int numberOfAmmunition = 32;                            // 소지하고 있는 총알 개수(장전되지 않은)
    private float damage = 0f;
    private float timeAfterFire = 0f;                               // 총알 발사 후 지난 시간
    private float curTime = 0f;                                     // 총알 장전에 사용할 시간 변수
    private bool isLoading = false;                                 // 현재 장전중인지 확인
    private bool isCritical = false;                                // 이번 공격이 치명타를 입히는 공격인지 확인

    // 공격
    private readonly int oneShotBullets = 10;                       // 한 번에 발사되는 탄환 수 
    private readonly float attackInterval = 0.625f;                 // 공격 간격

    // 치명타 
    private readonly int criticalPercent = 2;                       // 치명타 발생률

    // 거리별 데미지값
    private readonly float baseDamage = 6f;                         // 기본 데미지 (중거리 데미지)
    private readonly float maximunDamage = 10.5f;                   // 근거리 데미지
    private readonly float minimumDamage = 3f;                      // 장거리 데미지
    private readonly float criticalDamage = 18f;                    // 치명타 데미지

    // 총알 장전 
    private readonly int maxLoadBulletCount = 6;                    // 한 번에 장전 가능한 총알 개수
    private readonly float firstLoadTime = 0.7f;                    // 초기 장전 시간( 첫 발 장전 시간)
    private readonly float continuousLoadTime = 0.5f;               // 연속 장전 시간( 첫 발 이후 한 발당 장전 시간)
    private readonly float loadingTime = 3.2f;                      // 총 장전 시간

    // 근거리 중거리 장거리 기준
    private readonly float closeRange = 3f;
    private readonly float middleRange = 5f;
    private readonly float longRange = 10f;

    // 레이캐스트 관련 변수
    private RaycastHit hit;                                         
    private GameObject preHit;                                      // 이전 hit
    private readonly float rayDistance = 10f;                       // 공격이 닿을 수 있는 최대 거리

    // 소리
    private PEA_ScoutSound scoutSound;

    // 에디터에서 연결해줄 변수
    public Transform[] firePos;

    // Start is called before the first frame update
    void Start()
    {
        scoutSound = GetComponentInParent<PEA_ScoutSound>();
    }

    // Update is called once per frame
    void Update()
    {
        //CheckTimeAfterFire();

        //if (Input.GetMouseButtonDown(0) && !isLoading && timeAfterFire >= attackInterval && loadBullets > 0)
        //{
        //    Fire();
        //}

        //if (Input.GetKeyDown(KeyCode.R) && loadBullets < maxLoadBulletCount)
        if (loadBullets == 0 && maxLoadBulletCount > 0)
        {
            state = State.Loading;
        }

        if (state == State.Loading)
        {
            ReLoad();
        }
    }

    private void CheckTimeAfterFire()
    {
        timeAfterFire += Time.deltaTime;
    }

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
            else if(distance <= middleRange)
            {
                damage += baseDamage;
            }
            else 
            {
                damage += minimumDamage;
            }
        }
    }

    private void ReLoad()
    {
        print("Redloading");
        curTime += Time.deltaTime;
        scoutSound.ScatterReload();

        if(curTime >= loadingTime)
        {
            numberOfAmmunition = numberOfAmmunition - (maxLoadBulletCount - loadBullets);
            loadBullets = maxLoadBulletCount;
            curTime = 0f;
            isLoading = false;
            print("ReLoaded");
        }
    }

    private void ShowDamage()
    {
        print(damage);
    }

    public void Fire()
    {
        if (state == State.Wait)
        {
            state = State.Fire;
        }
        else
        {
            return;
        }

        randomCritical = Random.Range(1, 100);
        if(randomCritical <= criticalPercent)
        {
            isCritical = true;
        }
        else
        {
            isCritical = false;
        }

        for(int i = 0; i < oneShotBullets; i++)
        {

            Debug.DrawRay(firePos[i].position, firePos[i].forward * rayDistance, Color.red);
            //Destroy(Instantiate(bulletPrefab, firePos[i].position, firePos[i].rotation), 0.625f);
            if(Physics.Raycast(firePos[i].position, firePos[i].forward, out hit, rayDistance))
            {
                print(hit.transform.gameObject);
                if(preHit == null)
                {
                    preHit = hit.transform.gameObject;
                }

                if (preHit == hit.transform.gameObject)
                {
                    SetDamage( Vector3.Distance(transform.position, hit.point), isCritical);
                }
            }
        }
        scoutSound.ScatterShoot();
        ShowDamage();
        loadBullets--;
        damage = 0;
        timeAfterFire = 0f;

        print("load Bullet : " + loadBullets);
        print("number Of Ammunition : " + numberOfAmmunition);
    }
}
