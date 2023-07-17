using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Bullet : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody rb;
    float currTime = 0;
    public float returnTime = 2;
    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody>();
        rb.velocity = transform.forward*speed;
    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        if(currTime > returnTime)
        {
            J_ObjectPool.instance.Fire_Finished(gameObject);
            currTime = 0;
        }
        
        transform.forward = rb.velocity.normalized;
        //GetComponent<Rigidbody>().velocity = transform.up * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var otherRB = collision.gameObject.GetComponent<Rigidbody>();
        if(otherRB != null)
        {
            otherRB.AddForce(transform.forward * otherRB.mass * 20, ForceMode.Impulse);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        J_ObjectPool.instance.Fire_Finished(gameObject);
    }

}
