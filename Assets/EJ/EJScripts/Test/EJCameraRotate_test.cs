using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rotatetest
{
    public class EJCameraRotate : MonoBehaviour
    {
        //Rotate ����
        float rx, ry;
        float rotSpeed = 100;
        public GameObject player;

        //Shake ����
        float knockbackSpeed = 5;
        float knockbackRotation = 15;
        bool isUp;


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

            //���� �̵�ó�� �и��� ������ ���ڿ�
            //cc.Chase(knockBackdirection * knockbackSpeed);     

            //ī�޶� �и� �������� ���Ѵ�.
            //Vector3 knockbackPos = transform.position + knockBackdirection * knockbackSpeed;



            // Lerp(from, to, �ð��� ���� ��ġ)
            transform.position = Vector3.Lerp(transform.position, player.transform.position + knockbackPos, knockbackSpeed * Time.deltaTime);

            //���࿡ ���� ��ġ�� KnockbackPos �� ��������ٸ�
            float distance = Vector3.Distance(transform.position, player.transform.position + knockbackPos);
            if (distance < 0.1f)
            {
                knockbackPos = currentPosition;
            }


            ////camera�� �������� rotate�ߴٰ� �ٽ� �������� ��鸰��.
            //ry += knockbackRotation * Time.deltaTime;

            //if (ry > knockbackRotation)
            //{
            //    isUp = true;
            //    if (isUp)
            //    {
            //        ry -= knockbackRotation * Time.deltaTime;

            //        //���� ���������� ���ư���.
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
