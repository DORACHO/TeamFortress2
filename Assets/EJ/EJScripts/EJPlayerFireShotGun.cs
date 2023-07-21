using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EJPlayerFireShotGun : MonoBehaviour
{
    public EJCameraRotate ejcameraRotate;

    enum bulletImpactName
    {
        Floor,
        Enemy
    }

    //bulletImpact 효과
    public GameObject[] bulletImpactFactory;

    //muzzle 효과
    public GameObject bulletMuzzleImpactFactory;
    public Transform muzzlePos;
    public GameObject muzzleSprite;
    
    //산탄총
    int bulletForonetime = 9;
    public Transform[] firePos;
    float maxRayDistance = 10f;

    //총알 수
    int fireCount;
    int magazineFireCount;

    //장전한 총알
    int maxMagazine = 6;
    int leftMagazine;

    //남은 총알
    int leftBullet;
    //총 총알
    int maxBullet = 32;

    //총알 불렛에 들어있는 총알 수 (max:6)
    public TextMeshProUGUI bulletRulletText;

    // 총 남은 총알 수
    public TextMeshProUGUI leftBulletText;

    // 총 쏠 수 있는지?
    public bool canFire = true;
    float currentTime;
    float delayTime = 0.5f;

    // 애니메이터
    public EJPlayerAnim stateMgr;

    // Start is called before the first frame update
    void Start()
    {
        leftBullet = maxBullet;
        leftMagazine = maxMagazine;
        canFire = true;

        enemyLayer = (1 << LayerMask.NameToLayer("Enemy"));
        mapLayer = (1 << LayerMask.NameToLayer("Map"));
    }

    int enemyLayer;
    int mapLayer;

    // Update is called once per frame
    void Update()
    {

        LEFT_BULLET = LEFT_BULLET;
        LEFT_MAGAZINE = LEFT_MAGAZINE;
        UpdateFire();
        UpdateBulletCharge();
    }

    private void UpdateFire()
    {
        currentTime += Time.deltaTime;
        stateMgr.ChangeState(EJPlayerAnim.State.Fire);

        if (canFire)
         {
            if (Input.GetButtonDown("Fire1") && fireCount<maxBullet)
            {

                //ejcameraRotate.UpdateCameraReact();

                //GameObject mainCam = GameObject.FindWithTag("MainCamera");
                //mainCam.GetComponent<EJCameraRotate>().UpdateCameraReact();
                /*print(Camera.main.name);
                if(Camera.main.TryGetComponent<EJCameraRotate>(out EJCameraRotate cameraRotate))
                {
                    cameraRotate.UpdateCameraReact();
                }
                else
                {
                    print("cameraRotate Null");
                }*/
                Camera.main.GetComponent<EJCameraRotateBackUP.EJCameraRotate>().UpdateCameraReact();
                //GetComponentInChildren<EJCameraRotate>().UpdateCameraReact();

                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                RaycastHit hitInfo;

                GameObject bulletMuzzleImpact = Instantiate(bulletMuzzleImpactFactory);
                //bulletMuzzleImpact.transform.position = muzzlePos.transform.position;
                bulletMuzzleImpact.transform.forward = muzzlePos.transform.right;
                bulletMuzzleImpact.transform.parent = muzzlePos.transform;

                ONmuzzleSprite();
                Invoke(nameof(OFFmuzzleSprite), 0.1f);

                for (int i = 0; i < bulletForonetime; i++)
                {
                    Debug.DrawRay(firePos[i].position, firePos[i].forward * maxRayDistance, Color.red);
                    
                   if (Physics.Raycast(firePos[i].position, firePos[i].forward, out hitInfo, maxRayDistance, enemyLayer))
                   {
                        print("Fire1 Clicked"); 
                        GameObject bulletImpact = Instantiate(bulletImpactFactory[0]);
                       
                        //맞은 게임오브젝트에 단위벡터 짜리 큐브를 하나 만들고
                        //그곳에 이펙트가 쏴지면 좋겠다?

                        //똑같은 크기의 이펙트가 생성되도록 하는 법?
                       // bulletImpact.transform.localScale.Normalize();
                        bulletImpact.transform.position = hitInfo.point;
                        bulletImpact.transform.forward = hitInfo.normal;
                        bulletImpact.transform.parent = hitInfo.transform;

                        //Enemy에게 Damage를 준다 (임시로 넣어둠)
                        //EJEnemyHPForTest.instance.ENEMY_HP -= 5;

                        //GetComponentInChildren<EJCameraRotate>().PlayFireSFX();
                        EJSFX.instance.PlayFireSFX();

                        Destroy(bulletImpact, 3);
                    }

                    if (Physics.Raycast(firePos[i].position, firePos[i].forward, out hitInfo, maxRayDistance, mapLayer))
                    {
                        print("Fire1 Clicked");
                        GameObject bulletImpact = Instantiate(bulletImpactFactory[1], hitInfo.transform);

                        //똑같은 크기의 이펙트가 생성되도록 하는 법?
                        //bulletImpact.transform.localScale.Normalize();

                        bulletImpact.transform.position = hitInfo.point;
                        bulletImpact.transform.forward = hitInfo.normal;
                        bulletImpact.transform.parent = hitInfo.transform;

                        //Enemy에게 Damage를 준다 (임시로 넣어둠)
                        //EJEnemyHPForTest.instance.ENEMY_HP -= 5;

                        //GetComponentInChildren<EJCameraRotate>().PlayFireSFX();
                        EJSFX.instance.PlayFireSFX();

                        Destroy(bulletImpact, 3);
                    }
                }

                //UI표시
                fireCount++;
                magazineFireCount++;

                leftBullet = maxBullet - fireCount;
                leftMagazine = maxMagazine - magazineFireCount;
                
            }
        }
        if (leftMagazine == 0 && leftBullet >= 6)
        {
             leftMagazine += maxMagazine;
             magazineFireCount = 0;
        }
        else if (leftBullet == 0 && leftBullet < 6)
       {
             leftMagazine += leftBullet;
             magazineFireCount = 0;
        }

        if (fireCount>0 && (fireCount % maxMagazine == 0 || leftBullet == 0))
        {
             canFire = false;
             canFire = false;
             Invoke(nameof(ONFire), delayTime);
        }
    }


    public void UpdateBulletCharge()
    {
       if (Input.GetKeyDown(KeyCode.R))
        {
            stateMgr.ChangeState(EJPlayerAnim.State.Reload);
            fireCount = 0;
            leftBullet = maxBullet;
            leftMagazine = maxMagazine;

            EJSFX.instance.PlayLoadSFX();
        }
    }

    void ONFire()
    {
        canFire = true;
    }

    void ONmuzzleSprite()
    {
        muzzleSprite.SetActive(true);
    }

    void OFFmuzzleSprite()
    {
        muzzleSprite.SetActive(false);
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

    public int LEFT_MAGAZINE
    {
        get { return leftMagazine; }
        set
        {
            leftMagazine = value;
            bulletRulletText.text = $"{leftMagazine}";
        }
    }

    #endregion
}
