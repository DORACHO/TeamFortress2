using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EJCSentryGun : MonoBehaviour
{
    //Fire
    public Transform firePosition;
    public GameObject bulletImpactFactory;

    //Fire target
    public LayerMask enemyLayer;
    GameObject target;
    bool isEnemy = true;
    float attackRange = 8;
    int fireCount;
    int maxFireCount = 1;

    //Rotate
    public GameObject head;

    float ry = 0;
    float rotateAngle = 20;

    void Start()
    {

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
                    head.transform.forward = - target.transform.forward;
                    ry = -target.transform.forward.y;

                    UpdateFire();
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


    private void UpdateFire()
    {
        print("fire!");

        RaycastHit hitInfo;
        Ray ray = new Ray(firePosition.transform.position, firePosition.transform.forward);
       
        if (Physics.Raycast(ray, out hitInfo, float.MaxValue))
        {
            if (hitInfo.transform.gameObject == target) 
            {
                GameObject bulletImpact = Instantiate(bulletImpactFactory);
                bulletImpact.transform.forward = hitInfo.normal;
                bulletImpact.transform.position = hitInfo.point;
                fireCount++;
                Destroy(target);
            }
        }
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
}
