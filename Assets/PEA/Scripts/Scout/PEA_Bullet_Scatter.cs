using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_Bullet_Scatter : MonoBehaviour
{
    private float speed = 30f;
    private float distance = 0f;
    private bool isCritical = false;

    private Vector3 startPos = Vector3.zero;
    private Vector3 hitPos = Vector3.zero;
    private PEA_ScatterGun scatterGun;



    private void OnEnable()
    {
        startPos = transform.position;
        Critical();
    }
    // Start is called before the first frame update
    void Start()
    {
        scatterGun = GameObject.FindObjectOfType<PEA_ScatterGun>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }

    private void Critical()
    {
        int randomNum = Random.Range(1, 100);
        if(randomNum <= 2)
        {
            isCritical = true;
        }
        else
        {
            isCritical = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        hitPos = collision.contacts[0].point;
        distance = Vector3.Distance(startPos, hitPos);
        scatterGun.SetDamage(distance, isCritical);
        Destroy(gameObject);
    }
}
