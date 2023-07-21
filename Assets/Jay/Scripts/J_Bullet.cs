using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Bullet : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody rb;
    float currTime = 0;
    public float returnTime = 2;

    private bool canFire;
    private int maxBullet = 10;
    private int currentBullet = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canFire)
        {
            currTime += Time.deltaTime;
            if (currTime > returnTime)
            {
                canFire = true;
                currTime = 0;


                if (currentBullet >= maxBullet)
                    return;

                FireBullet();

                //J_ObjectPool.instance.Fire_Finished(gameObject);


            }
        }

    }

    void FireBullet()
    {
        if (canFire)
        {
            rb.velocity = transform.forward * speed;
            currentBullet++;
            canFire = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var otherRB = collision.gameObject.GetComponent<Rigidbody>();
        if (otherRB != null)
        {
            otherRB.AddForce(transform.forward * otherRB.mass * 20, ForceMode.Impulse);
        }

        J_ObjectPool.instance.Fire_Finished(gameObject);
        currentBullet--;

    }
    private void OnTriggerEnter(Collider other)
    {
        J_ObjectPool.instance.Fire_Finished(gameObject);
        currentBullet--;
    }
}