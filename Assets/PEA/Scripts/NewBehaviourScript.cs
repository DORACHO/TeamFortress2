using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private bool isRight = true;
    private float y = 0f;
    private float rotateSpeed = 90f;

    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dirToPlayer = player.position - transform.position.normalized;

        print(Vector3.Angle(transform.forward, dirToPlayer));
        if (Vector3.Angle(transform.forward, dirToPlayer) <= 30)
        {
            print("attack");
        }
    }
}
