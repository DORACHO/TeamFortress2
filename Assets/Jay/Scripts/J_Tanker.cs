using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Apple;

public class J_Tanker : MonoBehaviour
{
    public float speed = 4.38f;
    private GameObject target;
    float yVelocity;
    bool isMovingBackward;
    public float jumpProbability = 20f;

    public float jumpPower = 5f;
    public float gravity = -9.81f;
    NavMeshAgent agent;
    CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Destination");
     
        agent = GetComponent<NavMeshAgent>();
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        yVelocity += gravity * Time.deltaTime;

        if(cc.isGrounded && isMovingBackward && Random.value < jumpProbability)
        {
            yVelocity = jumpPower;
        }
        agent.destination = target.transform.position;
    }
}
