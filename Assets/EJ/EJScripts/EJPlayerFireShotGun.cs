using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EJPlayerFireShotGun : MonoBehaviour
{
    //muzzle È¿°ú
    public GameObject bulletImpactFactory;
    public GameObject bulletMuzzleImpactFactory;
    public Transform muzzlePos;
    
    //»êÅºÃÑ
    int bulletForonetime = 9;
    public Transform[] firePos;
    float maxRayDistance = 10f;

    //ÃÑ¾Ë ¼ö
    int fireCount;
    int magazineFireCount;

    //ÀåÀüÇÑ ÃÑ¾Ë
    int maxMagazine = 6;
    int leftMagazine;

    //³²Àº ÃÑ¾Ë
    int leftBullet;
    //ÃÑ ÃÑ¾Ë
    int maxBullet = 32;

    //ÃÑ¾Ë ºÒ·¿¿¡ µé¾îÀÖ´Â ÃÑ¾Ë ¼ö (max:6)
    public TextMeshProUGUI bulletRulletText;

    // ÃÑ ³²Àº ÃÑ¾Ë ¼ö
    public TextMeshProUGUI leftBulletText;

    // ÃÑ ½ò ¼ö ÀÖ´ÂÁö?
    public bool canFire = true;
    float currentTime;
    float delayTime = 3.5f;

    // ¾Ö´Ï¸ÞÀÌÅÍ
    public EJPlayerAnim stateMgr;

    // Start is called before the first frame update
    void Start()
    {
        leftBullet = maxBullet;
        leftMagazine = maxMagazine;
        canFire = true;

        layer = (1 << LayerMask.NameToLayer("Enemy"));
    }

    int layer;

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
                
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                RaycastHit hitInfo;

                GameObject bulletMuzzleImpact = Instantiate(bulletMuzzleImpactFactory);
                bulletMuzzleImpact.transform.position = muzzlePos.transform.position;
                bulletMuzzleImpact.transform.forward = muzzlePos.transform.forward;


                for (int i = 0; i < bulletForonetime; i++)
                {
                    Debug.DrawRay(firePos[i].position, firePos[i].forward * maxRayDistance, Color.red);
                    
                   if (Physics.Raycast(firePos[i].position, firePos[i].forward, out hitInfo, maxRayDistance))
                   {
                        print("Fire1 Clicked"); 
                        GameObject bulletImpact = Instantiate(bulletImpactFactory);
                        bulletImpact.transform.position = hitInfo.point;
                        bulletImpact.transform.forward = hitInfo.normal;

                        //Enemy¿¡°Ô Damage¸¦ ÁØ´Ù (ÀÓ½Ã·Î ³Ö¾îµÒ)
                        //EJEnemyHPForTest.instance.ENEMY_HP -= 5;

                        GetComponentInChildren<EJCameraRotate>().PlayFireSFX();
                    }
                }
                //UIÇ¥½Ã
                fireCount++;
                magazineFireCount++;

                leftBullet = maxBullet - fireCount;
                leftMagazine = maxMagazine - magazineFireCount;

                GetComponentInChildren<EJCameraRotate>().UpdateCameraReact();

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

            GetComponentInChildren<EJCameraRotate>().PlayReloadSFX();
        }
    }

    void ONFire()
    {
        canFire = true;
    }

    #region fire°ü·Ã UIÇ¥½Ã ÇÁ·ÎÆÛÆ¼
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
