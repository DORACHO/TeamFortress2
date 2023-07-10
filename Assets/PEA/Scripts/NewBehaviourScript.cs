using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private bool isRight = true;
    private float y = 0f;
    private float rotateSpeed = 90f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRight)
        {
            y += rotateSpeed * Time.deltaTime;
            if (y >= 90f)
            {
                isRight = false;
            }
        }
        else
        {
            y -= rotateSpeed * Time.deltaTime;
            if (y <= -90f)
            {
                isRight = true;
            }
        }

        transform.localEulerAngles = new Vector3(0, y, 0);
    }
}
