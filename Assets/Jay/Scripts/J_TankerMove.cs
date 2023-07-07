using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class J_TankerMove : MonoBehaviour
{
    public float frontspeed = 4.38f;
    public float backSpeed = 3.94f;
    private GameObject Destination;
    private Vector3 targetPosition;
    public float jumpPower = 5f;
    public float gravity = -9.81f;
    float yVelocity;
    bool isMovingBackward;
    public float jumpProbability = 20f;

    NavMeshAgent agent;
    CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        Destination = GameObject.Find("Destinations");
        cc = gameObject.GetComponent<CharacterController>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        targetPosition = Destination.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       // yVelocity += gravity * Time.deltaTime;

        //if (cc.isGrounded && isMovingBackward && Random.value < jumpProbability)
        {
            //yVelocity = jumpPower;
        }
       
        agent.destination = Destination.transform.position;
        
    }
}
