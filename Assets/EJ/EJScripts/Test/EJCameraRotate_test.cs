using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rotatetest
{
    public class EJCameraRotate : MonoBehaviour
    {
        //Rotate 변수
        float rx, ry;
        float rotSpeed = 100;
        public GameObject player;

        //Shake 변수
        float knockbackSpeed = 5;
        float knockbackRotation = 15;
        bool isUp;


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

            transform.localEulerAngles = new Vector3(-rx, 0, 0);
            player.transform.localEulerAngles = new Vector3(0, ry, 0);

            if (isKnockBack)
            {
                KnockBack();
            }
        }

        public bool isKnockBack;
        Vector3 knockbackPos;
        Vector3 currentPosition;
        //Knock Back
        //public void KnockBack(RaycastHit hitInfo)
        public void KnockBack()
        {
            //Vector3 knockBackdirection = hitInfo.normal;

            //Vector3 knockBackdirection = - GetComponentInParent<EJPlayerFire>().firePosition.forward;

            //순간 이동처럼 밀리기 때문에 부자연
            //cc.Move(knockBackdirection * knockbackSpeed);     

            //카메라가 밀릴 포지션을 구한다.
            //Vector3 knockbackPos = transform.position + knockBackdirection * knockbackSpeed;



            // Lerp(from, to, 시간에 따른 위치)
            transform.position = Vector3.Lerp(transform.position, player.transform.position + knockbackPos, knockbackSpeed * Time.deltaTime);

            //만약에 나의 위치가 KnockbackPos 와 가까워졌다면
            float distance = Vector3.Distance(transform.position, player.transform.position + knockbackPos);
            if (distance < 0.1f)
            {
                knockbackPos = currentPosition;
            }


            ////camera가 위쪽으로 rotate했다가 다시 원점으로 흔들린다.
            //ry += knockbackRotation * Time.deltaTime;

            //if (ry > knockbackRotation)
            //{
            //    isUp = true;
            //    if (isUp)
            //    {
            //        ry -= knockbackRotation * Time.deltaTime;

            //        //원래 포지션으로 돌아간다.
            //        transform.position = Vector3.Lerp(knockbackPos, transform.position, knockbackSpeed * Time.deltaTime);
            //        isUp = false;
            //        isKnockBack = false;
            //    }
            //}                    
        }

        public void StartKnockBack()
        {
            isKnockBack = true;
            Vector3 knockBackdirection = -GetComponentInParent<EJPlayerFire>().firePosition.forward;
            knockbackPos = transform.position + knockBackdirection;
            knockbackPos = knockbackPos - player.transform.position;
            currentPosition = transform.position;
            currentPosition = currentPosition - player.transform.position;
        }
    }
}
