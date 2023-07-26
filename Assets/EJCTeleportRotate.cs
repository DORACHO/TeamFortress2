using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJCTeleportRotate : MonoBehaviour
{
    float rotSpeed = 150f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
    }
}
