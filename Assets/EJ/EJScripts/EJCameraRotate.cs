using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJCameraRotate : MonoBehaviour
{
    //Rotate ����
    float rx, ry;
    public float rotSpeed = 200;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        //rx, ry ���� ���� �ʱ� eulerAngle ���� ���� 
        ry = player.transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse�� �����ӿ� ���� Rotate
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rx += my * rotSpeed * Time.deltaTime;
        ry += mx * rotSpeed * Time.deltaTime;

        rx = Mathf.Clamp(rx, -75, 75);

        transform.localEulerAngles = new Vector3(-rx, 0, 0);
        player.transform.localEulerAngles = new Vector3(0, ry, 0);
    }
}
