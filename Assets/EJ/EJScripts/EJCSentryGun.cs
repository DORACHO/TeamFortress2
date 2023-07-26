using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EJCSentryGun : MonoBehaviour
{
    //RayFire, BulletFire
    public Transform firePosition;
    public GameObject bulletImpactFactory;
    public GameObject bulletFactory;

    float attackRange = 8;
    int fireCount;
    int maxFireCount = 1;

    //Fire target
    public LayerMask enemyLayer;
    GameObject target;

    //Rotate
    public GameObject head;

    float ry = 0;
    float rotateAngle = 20;

    //BulletFire ObjectPool
    List<GameObject> bulletObjectPool;
    int bulletObjectPoolCount = 2;
    public static List<GameObject> deActiveBulletObjectPool;
    public Transform bulletParent;


    public List<GameObject> DeActiveBulletObjectPool
    {
        get { return deActiveBulletObjectPool; }
    }

    void Start()
    {
        //model axis =>unity world axis
        transform.right = firePosition.forward;

        //bulletFire objectPool
        /*bulletObjectPool = new List<GameObject>();
        deActiveBulletObjectPool = new List<GameObject>();

        for (int i = 0; i < bulletObjectPoolCount; i++)
        {
            GameObject bullet = Instantiate(bulletFactory);

            bullet.transform.parent = bulletParent;
            bullet.SetActive(false);
            bulletObjectPool.Add(bullet);
            deActiveBulletObjectPool.Add(bullet);
        }*/

    }

    void Update()
    {
        //AttackRange 안에 들어온 Enemy 검출
        print(fireCount);
        //OverlapSphere: ray 기준 반경 Nm 이내에 들어온 gameObject를
        //collider로 검출해서 배열에 담는다.

        Collider[] enemies
            = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);


        //enemy가 검출되면 쏜다   
        if (enemies.Length > 0)
        {
                print("enemy detected!");
                Invoke(nameof(fireCountReset), 3);

           
            for (int i = 0; i < enemies.Length; i++)
            {
                if (fireCount < maxFireCount)
                {
                    target = enemies[i].gameObject;

                    //head.transform.LookAt(target.transform);
                    transform.right = firePosition.forward;
                    firePosition.transform.LookAt(target.transform);


                    //head.transform.forward = - target.transform.forward;
                    //ry = -target.transform.forward.y;

                    UpdateRayFire();
                    //UpdateMakeBullet();

                    Invoke(nameof(fireCountReset), 3);                    
                }
            }            
        }
        //enemy가 오지 않으면
        else
        {
            UpdateIdle();
        }

    }


    private void UpdateRayFire()
    {
        print("fire!");

        RaycastHit hitInfo;
        Ray ray = new Ray(firePosition.transform.position, firePosition.transform.forward);

        Debug.DrawRay(firePosition.transform.position, firePosition.transform.forward, Color.red);

        if (Physics.Raycast(ray, out hitInfo, attackRange, enemyLayer))
        {
            if (hitInfo.transform.gameObject == target) 
            {
                GameObject bulletImpact = Instantiate(bulletImpactFactory);
                bulletImpact.transform.forward = hitInfo.normal;
                bulletImpact.transform.position = hitInfo.point;
                EJSFX_SentryGun.instance.PlaySentryFireSFX();

                fireCount++;
                Destroy(target);
            }
        }
    }



    private void UpdateMakeBullet()
    {
        GameObject bullet = GetBulletFromObjectPool();

        if (bullet != null)
        {
            bullet.transform.position = firePosition.position;
            bullet.transform.forward = firePosition.forward;
        }
    }

    GameObject GetBulletFromObjectPool()
    {
        if (DeActiveBulletObjectPool.Count > 0)
        {
            GameObject bullet = DeActiveBulletObjectPool[0];
            bullet.SetActive(true);
            DeActiveBulletObjectPool.Remove(bullet);
            return bullet;
        }
        return null;
    }

    bool isRightRotate;
    private void UpdateIdle()
    {

        if (isRightRotate)
        {
            ry += rotateAngle * Time.deltaTime;

            if (ry >= 90)
            {
                isRightRotate = false;
            }
        }else
        {
            ry -= rotateAngle * Time.deltaTime;

            if (ry <= -90)
            {
                isRightRotate = true;
            }
        }

        head.transform.localEulerAngles = new Vector3(0, ry, 0);

    }  
    
    void fireCountReset()
    {
        fireCount = 0;
    }

    //적의 정보를 담아주는 함수
    public GameObject sentryTarget()
    {
        Collider[] enemies
           = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (fireCount < maxFireCount)
                {
                    target = enemies[i].gameObject;

                    return target;
                }
            }
        }
        return null;
    }
}
