using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EJCameraRotateBackUP
{
    public class EJCameraRotate : MonoBehaviour
    {
        //Rotate ����
        float rx, ry;
        float rotSpeed = 100;
        public GameObject player;

        //Camera React ����
        float cx;
        float targetCX;

        //Shake ����
        float knockbackSpeed = 5;
        float knockbackRotation = 1;
        bool isUp;

        //Audio ����
        public AudioSource fireSFX;
        public AudioSource reloadSFX;

        // Start is called before the first frame update
        void Start()
        {
            //rx, ry ���� ���� �ʱ� eulerAngle ���� ���� 
            ry = player.transform.localEulerAngles.y;

            //cx �ʱ�ȭ
            //targetCX = 6;
            targetCX = transform.localEulerAngles.x;

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

            transform.localEulerAngles = new Vector3(-rx + cx, 0, 0);
            player.transform.localEulerAngles = new Vector3(0, ry, 0);

            //UpdateCameraReact();
            cx = Mathf.Lerp(cx, targetCX, Time.deltaTime * 8);


            CameraChange2ChaseCam();
            CameraChange2MainCam();
            Whoareyou();

        }

        public bool isKnockBack;
        Vector3 knockbackPos;
        Vector3 currentPosition;


        //��ư�� ������ fireScript���� �Լ� ȣ���ϰų�,
        // fireScript�� canFire bool�� true��� �۵��ϰ� �ϰų�.

        public void UpdateCameraReact()
        {
            //if (Input.GetButtonDown("Fire1"))
            //if (GetComponent<EJPlayerFireShotGun>().canFire)
            {
                cx -= 10;
            }
        }



        public void StartKnockBack()
        {
            isKnockBack = true;
            Vector3 knockBackdirection = -GetComponentInParent<EJPlayerFire>().firePosition.forward;

            // knockbackPos �ʱ�ȭ
            knockbackPos = transform.position + knockBackdirection;
            // player�� knockbackPos�� ���ϴ� ����
            knockbackPos = knockbackPos - player.transform.position;

            // currentPosition �ʱ�ȭ
            currentPosition = transform.position;
            // ���� �������� ���� ��ġ���� �̵��� ��ġ ���ϴ� ����
            currentPosition = currentPosition - player.transform.position;
        }

        //���� ���� ����� ��� ã�Ƽ� �����
        //������ �Ѿ��� ��ü
        //Parent�� �������� ���?

        GameObject murderer;

        public bool chaseCameraON;
        public bool mainCameraON;

        public void CameraChange2ChaseCam()
        {
            if (EJPSHP.instance.HP <= 0)
            {
                OFFMainCamera();
                ONChaseCamera();
            }
        }

        public void CameraChange2MainCam()
        {
            if (EJPSHP.instance.HP > 0)
            {
                ONMainCamera();
                OFFChaseCamera();
            }
        }

        public void Whoareyou()
        {
            if (EJPSHP.instance.HP <= 0)
            {
                //Update���� ȣ����� �ʴ� �� ����ؾ��Ѵ�!...
                EJPSHP.instance.Dead();

                if (chaseCameraON)
                {
                    print("whoareyou �������Դϴ�");
                    GameObject chaseCamera = GameObject.FindWithTag("ChaseCamera");

                    CameraChange2ChaseCam();
                    //01. ���� Player������
                    ONCharacter();
                    Vector3 characterCamPos = transform.position - Vector3.back;

                    //02. Enemy�� Cam�̵�
                    Vector3 dir2Enemy = EJPSHP.instance.murdererPos - chaseCamera.transform.position;
                    dir2Enemy.Normalize();

                    // transform.parent = null;
                    //Vector3 murdererCamPos = EJPSHP.instance.murdererPos + (-dir2Enemy /*+ Vector3.up * 0.5f + Vector3.*/);
                    Vector3 murdererCamPos = EJPSHP.instance.murdererPos - (dir2Enemy * 3);

                    chaseCamera.transform.position = Vector3.Lerp(chaseCamera.transform.position, murdererCamPos, Time.deltaTime * 5);

                    chaseCamera.transform.forward = Vector3.Lerp(chaseCamera.transform.forward, dir2Enemy, Time.deltaTime * 5);

                    //chaseCamera.transform.LookAt(EJPSHP.instance.murdererPos);

                    Invoke(nameof(CameraChange2MainCam), 3);
                }
            }
        }

        public void ONCharacter()
        {
            GameObject fullBodyModel = GameObject.FindWithTag("fullBodyPlayer");
            fullBodyModel.SetActive(true);
        }

        public void OFFCharacter()
        {
            GameObject fullBodyModel = GameObject.FindWithTag("fullBodyPlayer");
            fullBodyModel.SetActive(false);
        }

        public void OFFMainCamera()
        {
            GetComponent<Camera>().enabled = false;
            mainCameraON = false;
            //print("����ī�޶� �������ϴ�");

        }
        public void ONMainCamera()
        {
            GetComponent<Camera>().enabled = true;
            mainCameraON = true;
            //print("����ī�޶� �������ϴ�");
        }

        public void OFFChaseCamera()
        {
            GameObject chaseCamera = GameObject.FindWithTag("ChaseCamera");
            chaseCamera.GetComponent<Camera>().enabled = false;
            chaseCameraON = false;
            //print("����ī�޶� �������ϴ�");
        }

        public void ONChaseCamera()
        {
            GameObject chaseCamera = GameObject.FindWithTag("ChaseCamera");
            chaseCamera.GetComponent<Camera>().enabled = true;
            chaseCameraON = true;
            //print("����ī�޶� �������ϴ�");
        }
    }
}