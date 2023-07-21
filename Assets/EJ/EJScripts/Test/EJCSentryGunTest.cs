using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJCSentryGunTest : MonoBehaviour
{


    //Attack
    bool isEnemy = true;
    public float attackRange = 3;
    GameObject target;

    //Fire
    public Transform firePosition;
    public GameObject bulletImpactFactory;

    float currentTime;
    public float fireTime = 0.355f;
    public float fireWaitTime = 0.5f;
    public float fireFinishedTime = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFire();   
    }

    private void UpdateFire()
    {
        currentTime += Time.deltaTime;

        if (isEnemy)
        {
            if (currentTime > fireTime)
            {
                //ray라는 광선을 만들었다
                Ray ray = new Ray(firePosition.transform.position, firePosition.transform.forward);

                //hitInfo에 ray에 맞은 것을 담는다.
                RaycastHit hitInfo;
                int layer = (1 << LayerMask.NameToLayer("Enemy"));


                if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
                {
                    GameObject bulletImpact = Instantiate(bulletImpactFactory);
                    bulletImpact.transform.forward = hitInfo.normal;
                    bulletImpact.transform.position = hitInfo.point;

                }

                currentTime = 0;
            }

        }


    }
}
