using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJPlayerDispenser : MonoBehaviour
{
    float currentTime;
    public float chargeTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.name.Contains("Dispenser"))
        {
            currentTime += Time.deltaTime;

            if (currentTime > chargeTime)
            {
                EJPSHP.instance.HP += 5;
                currentTime = 0;
            }
        }
    }
}
