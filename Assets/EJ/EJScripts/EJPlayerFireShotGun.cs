using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EJPlayerFireShotGun : MonoBehaviour
{

    enum bulletImpactName
    {
        Default,
        Enemy
    }

    //bulletImpact ȿ��
    public GameObject[] bulletImpactFactory;

    //muzzle ȿ��
    public GameObject bulletMuzzleImpactFactory;
    public Transform muzzlePos;
    public GameObject muzzleSprite;
    
    //��ź��
    int bulletForonetime = 9;
    public Transform[] firePos;
    float maxRayDistance = 10f;

    //�Ѿ� ��
    int fireCount;
    int magazineFireCount;

    //������ �Ѿ�
    int maxMagazine = 6;
    int leftMagazine;

    //���� �Ѿ�
    int leftBullet;
    //�� �Ѿ�
    int maxBullet = 32;

    //�Ѿ� �ҷ��� ����ִ� �Ѿ� �� (max:6)
    public TextMeshProUGUI bulletRulletText;

    // �� ���� �Ѿ� ��
    public TextMeshProUGUI leftBulletText;

    // �� �� �� �ִ���?
    public bool canFire = true;
    float currentTime;
    float delayTime = 0.2f;

    // �ִϸ�����
    public EJPlayerAnim stateMgr;

    // Start is called before the first frame update
    void Start()
    {
        leftBullet = maxBullet;
        leftMagazine = maxMagazine;
        canFire = true;

        enemyLayer = (1 << LayerMask.NameToLayer("Enemy"));
        defaultLayer = (1 << LayerMask.NameToLayer("Default"));
    }

    int enemyLayer;
    int defaultLayer;

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
                Camera.main.GetComponent<EJCameraRotate>().UpdateCameraReact();
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
                        GameObject bulletImpact = Instantiate(bulletImpactFactory[1]);
                       
                        //���� ���ӿ�����Ʈ�� �������� ¥�� ť�긦 �ϳ� �����
                        //�װ��� ����Ʈ�� ������ ���ڴ�?

                        //�Ȱ��� ũ���� ����Ʈ�� �����ǵ��� �ϴ� ��?
                       // bulletImpact.transform.localScale.Normalize();
                        bulletImpact.transform.position = hitInfo.point;
                        bulletImpact.transform.forward = hitInfo.normal;
                        bulletImpact.transform.parent = hitInfo.transform;

                        //Enemy���� Damage�� �ش� (�ӽ÷� �־��)
                        //EJEnemyHPForTest.instance.ENEMY_HP -= 5;

                        //GetComponentInChildren<EJCameraRotate>().PlayFireSFX();
                        EJSFX.instance.PlayFireSFX();

                        //������ �ֱ�
                        if(hitInfo.transform.TryGetComponent<PEA_ScoutHp>(out PEA_ScoutHp pea_ScoutHp))
                        {
                            pea_ScoutHp.Damage(4);
                        }

                        Destroy(bulletImpact, 3);
                    }

                    if (Physics.Raycast(firePos[i].position, firePos[i].forward, out hitInfo, maxRayDistance, defaultLayer))
                    {
                        print("Fire1 Clicked");
                        GameObject bulletImpact = Instantiate(bulletImpactFactory[0]);

                        //�Ȱ��� ũ���� ����Ʈ�� �����ǵ��� �ϴ� ��?
                        //bulletImpact.transform.localScale.Normalize();

                        bulletImpact.transform.position = hitInfo.point;
                        bulletImpact.transform.forward = hitInfo.normal;
                        bulletImpact.transform.parent = hitInfo.transform;

                        //Enemy���� Damage�� �ش� (�ӽ÷� �־��)
                        //EJEnemyHPForTest.instance.ENEMY_HP -= 5;

                        //GetComponentInChildren<EJCameraRotate>().PlayFireSFX();
                        EJSFX.instance.PlayFireSFX();

                        Destroy(bulletImpact, 3);
                    }
                }

                //UIǥ��
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


    #region fire���� UIǥ�� ������Ƽ
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
