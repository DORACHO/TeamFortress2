using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private float speed = 6f;
    private float originY = 0f;
    private readonly float maxY = 3f;

    // Start is called before the first frame update
    void Start()
    {
        originY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.IsStart && transform.position.y < originY + maxY)
        {
            DoorOpen();
        }
        else if(GameManager.instance.IsStart && transform.position.y >= originY + maxY)
        {
            enabled = false;
        }
    }

    private void DoorOpen()
    {
        transform.Translate(transform.up * speed * Time.deltaTime);
    }
}
