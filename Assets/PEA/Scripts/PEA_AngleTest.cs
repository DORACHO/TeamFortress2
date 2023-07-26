using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_AngleTest : MonoBehaviour
{
    float angle = 0f;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        Vector3 dirToPlayer = playerPos - transform.position;
        dirToPlayer.Normalize();

        angle = Vector3.Angle(transform.forward, dirToPlayer);
        print(angle);
    }
}
