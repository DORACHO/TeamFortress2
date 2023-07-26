using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_Door : MonoBehaviour
{
    private float speed = 6f;
    private float originY = 0f;
    private readonly float maxY = 6f;

    // Start is called before the first frame update
    void Start()
    {
        originY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.IsStart)
        {
            DoorOpen();
        }
    }

    private void DoorOpen()
    {
        transform.Translate(transform.up * speed * Time.deltaTime);

        if(transform.position.y >= originY + maxY)
        {
            print("doorOpened");
            enabled = false;

        }
    }
}
