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
    public float attackRange = 3;
    int fireCount;
    int maxFireCount = 1;

    //Rotate
    public GameObject head;

    float ry = 0;
    public float rotateAngle = 0.0003f;

    void Start()
    {

    }

    void Update()
    {
        //AttackRange 안에 들어온 Enemy 검출

        //OverlapSphere: ray 기준 반경 Nm 이내에 들어온 gameObject를
        //collider로 검출해서 배열에 담는다.

        Collider[] enemies 
            = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        //enemy가 검출되면 쏜다   
        if (enemies.Length > 0)
        {
            print("enemy detected!");
            for(int i = 0; i<enemies.Length; i++)
            {
                target = enemies[i].gameObject;
                Invoke(nameof(UpdateFire),3);
                fireCount = 0;
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
        // hit info의 target 

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
            }
        }

    }

    private void UpdateIdle()
    {
        print("No one detected!");

        ry = head.transform.localEulerAngles.y;

        if (ry > 90)
        {
            ry -= rotateAngle * Time.deltaTime;
        }
        else  
        {
            ry += rotateAngle * Time.deltaTime;
        }
        
        

        //총구 head가 회전한다.
        head.transform.eulerAngles += new Vector3(0, ry, 0);
        //head.transform.Rotate(0, ry, 0);       
    }



    
}
