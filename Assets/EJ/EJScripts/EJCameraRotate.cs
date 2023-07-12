using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJCameraRotate : MonoBehaviour
{
    //Rotate 변수
    float rx, ry;
    float rotSpeed = 100;
    public GameObject player;

    //Shake 변수
    float knockbackSpeed = 5;
    float knockbackRotation = 1;
    bool isUp;
    //knockBack의 회전 변수
    float cx;


    // Start is called before the first frame update
    void Start()
    {
        //rx, ry 값을 나의 초기 eulerAngle 값을 넣자 
        ry = player.transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse의 움직임에 따라 Rotate
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rx += my * rotSpeed * Time.deltaTime;
        ry += mx * rotSpeed * Time.deltaTime;

        rx = Mathf.Clamp(rx, -75, 75);

        transform.localEulerAngles = new Vector3(-rx+cx, 0, 0);
        player.transform.localEulerAngles = new Vector3(0, ry, 0);

        if (isKnockBack)
        {
            KnockBack();
        }
    }

    public bool isKnockBack;
    Vector3 knockbackPos;
    Vector3 currentPosition;
    
    public void KnockBack()
    {
        //위로 올라갔다가, 스무스하게 내려가고 싶다.
        transform.position = knockbackPos;
        cx += knockbackRotation;

        //만약에 나의 위치가 KnockbackPos와 가까워졌다면
        float distance = Vector3.Distance(transform.position, player.transform.position + knockbackPos);

        if (distance < 0.1f)
        {
            transform.position = Vector3.Lerp(knockbackPos, currentPosition, knockbackSpeed * Time.deltaTime);
            cx = Mathf.Lerp(cx, cx - knockbackRotation, 0.8f);
            isKnockBack = false;
        }
                
    }

    public void StartKnockBack()
    {
        isKnockBack = true;
        Vector3 knockBackdirection = -GetComponentInParent<EJPlayerFire>().firePosition.forward;

        // knockbackPos 초기화
        knockbackPos = transform.position + knockBackdirection ;
        // player가 knockbackPos를 향하는 방향
        knockbackPos = knockbackPos - player.transform.position;

        // currentPosition 초기화
        currentPosition = transform.position;
        // 현재 포지션은 현재 위치에서 이동한 위치 향하는 방향
        currentPosition = currentPosition - player.transform.position;
    }
}
