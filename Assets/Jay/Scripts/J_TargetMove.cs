using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class J_TargetMove : MonoBehaviour
{
    public float speed = 6.1f;
    GameObject target;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        Vector3 dir = target.transform.position - this.transform.position;
        dir.Normalize();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.transform.position;
    }
}
