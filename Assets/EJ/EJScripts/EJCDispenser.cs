using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJCDispenser : MonoBehaviour
{
    float currentTime;
    public float chargeTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > chargeTime)
        {
            EJPSHP.instance.HP += 5;
            EJPSGoldOnHand.instance.GOLD += 5;
            currentTime = 0;
        }
    }
}
