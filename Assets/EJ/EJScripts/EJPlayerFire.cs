using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EJPlayerFire : MonoBehaviour
{
    //맞는 위치에 따른 총알 효과 구분 
    enum bulletImpactName
    {
        Floor,
        Enemy
    }

    //총알 효과를 담아줄 공장
    public GameObject[] bulletImpactFactories;
    public GameObject bulletmuzzleImpactFactory;
    public Transform firePosition;

    //Fire 변수들
    RaycastHit hitInfo;
    float impactTime = 2;
    

    //현재까지 쏜 총알의 수
    int fireCount;
    int rulletFireCount;

    //장전한 총알
    int maxBulletRullet = 6;
    int leftBulletRullet;

    //남은 총알 수
    int leftBullet;
    //총 총알 수 (탄창까지 합친 것)
    int maxbullet = 32;


    //총알 불렛에 들어있는 총알 수 (max:6)
    public TextMeshProUGUI bulletRulletText;
    // 총 남은 총알 수
    public TextMeshProUGUI leftBulletText;

    //피격 에너미 데미지
    bool isAttackHit;

    //총알 UI
    public TextMeshProUGUI hpText;


    // Start is called before the first frame update
    void Start()
    {
        leftBullet = maxbullet;
        leftBulletRullet = maxBulletRullet;
        //GameObject a = bulletImpactFactories[(int)bulletImpactName.Floor];
    }

    // Update is called once per frame
    void Update()
    {
        //초기화해줘야 하는 이유?
        LEFT_BULLET = LEFT_BULLET;
        LEFT_BULLETRULLET = LEFT_BULLETRULLET;
        
        UpdateFire();
        UpdateBulletCharge();
    }

    // 총알을 재장전한다.
    private void UpdateBulletCharge()
    {
       if (Input.GetKeyDown(KeyCode.R))
        {
            fireCount = 0;
            leftBullet = maxbullet;          
        }
    }

    //총을 쏠수 있니?
    bool canFire = true;
    float currentTime;
    float delayTime = 3.5f;

    private void UpdateFire()
    {
        // 총을 쏠 수있다면
        if (canFire)
        {
            if (Input.GetButtonDown("Fire1") && fireCount < maxbullet)
            {
                currentTime += Time.deltaTime;

                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

                //총구에서 효과 나오기
                GameObject bulletmuzzleImpact = Instantiate(bulletmuzzleImpactFactory);
                bulletmuzzleImpact.transform.position = firePosition.transform.position;

                if (currentTime > impactTime)
                {
                    Destroy(bulletmuzzleImpact);
                    currentTime = 0;
                }

                int layer = (1 << LayerMask.NameToLayer("Default"));

                //쏘면 knockback
                EJCameraRotate knockback = GetComponentInChildren <EJCameraRotate>();

                if (knockback != null)
                {
                    //knockback.KnockBack(hitInfo);
                    knockback.StartKnockBack();
                }

                //fireCount UI계산
                fireCount++;
                rulletFireCount++;

                leftBullet = maxbullet- fireCount;
                leftBulletRullet = maxBulletRullet - rulletFireCount;

                //남아있는 총알 수가 맥스 불렛만큼
                if(leftBulletRullet==0 && leftBullet >= 6)
                {
                    leftBulletRullet += maxBulletRullet;
                    rulletFireCount = 0;
                }else if(leftBulletRullet ==0 && leftBullet < 6)
                {
                    leftBulletRullet += leftBullet;
                    rulletFireCount = 0;
                }

                //만약에 fireCount 가 6의 배수라면
                //총을 못쏘게 한다.
                if (fireCount % maxBulletRullet == 0 || leftBullet ==0 )
                {
                    canFire = false;
                    Invoke(nameof(OffFire), delayTime);
                    //Invoke(nameof(bulletRulletCharge), delayTime);
                }
               
        

                if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
                {
                    GameObject bulletImpact = Instantiate(bulletImpactFactories[(int)bulletImpactName.Floor]);
                    
                    bulletImpact.transform.localScale = Vector3.one;
                    bulletImpact.transform.position = hitInfo.point;
                    bulletImpact.transform.forward = hitInfo.normal;

                    //총구의 앞방향이 crossHair을 보게 한다.
                    //gun.transform.LookAt(hitInfo.transform);

                    //맞은 곳에 따른 태그와 비교 필요
                    isAttackHit = true;
                    //만약 enemy라면, 데미지 얼마
                    //만약 construct라면 데미지 얼마

                }
                else
                {
                    //허공
                    //적이 맞지 않았다.
                    isAttackHit = false;
                }
            }
        }

    }

    void OffFire()
    {
        canFire = true;
    }

    void bulletRulletCharge()
    {        
        leftBulletRullet += maxBulletRullet;
    }


    #region fire관련 UI표시 프로퍼티
    public int LEFT_BULLET
    {
        get { return leftBullet; }
        set 
        { 
            leftBullet = value;
            leftBulletText.text = $"{leftBullet}";
        }
    }

    public int LEFT_BULLETRULLET
    {
        get { return leftBulletRullet; }
        set
        {
            leftBulletRullet = value;
            bulletRulletText.text = $"{leftBulletRullet}";
        }
    }

    #endregion
}
