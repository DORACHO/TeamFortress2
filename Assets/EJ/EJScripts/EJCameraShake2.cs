

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJCameraRotate : MonoBehaviour
{
    //Rotate 변수
    float rx, ry;
    float rotSpeed = 100;
    public GameObject player;

    //Camera React 변수
    float cx;
    float targetCX;

    //Shake 변수
    float knockbackSpeed = 5;
    float knockbackRotation = 1;
    bool isUp;

    //Audio 변수
    public AudioSource fireSFX;
    public AudioSource reloadSFX;

    // Start is called before the first frame update
    void Start()
    {
        //rx, ry 값을 나의 초기 eulerAngle 값을 넣자 
        ry = player.transform.localEulerAngles.y;

        //cx 초기화
        //targetCX = 6;
        targetCX = transform.localEulerAngles.x;

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


    //버튼을 누르고 fireScript에서 함수 호출하거나,
    // fireScript의 canFire bool이 true라면 작동하게 하거나.

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

        // knockbackPos 초기화
        knockbackPos = transform.position + knockBackdirection;
        // player가 knockbackPos를 향하는 방향
        knockbackPos = knockbackPos - player.transform.position;

        // currentPosition 초기화
        currentPosition = transform.position;
        // 현재 포지션은 현재 위치에서 이동한 위치 향하는 방향
        currentPosition = currentPosition - player.transform.position;
    }

    //나를 죽인 사람을 어떻게 찾아서 담는지
    //마지막 총알의 주체
    //Parent를 가져오는 방법?

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
            //Update에서 호출되지 않는 법 고민해야한다!...
            EJPSHP.instance.Dead();

            if (chaseCameraON)
            {
                print("whoareyou 실행중입니다");
                GameObject chaseCamera = GameObject.FindWithTag("ChaseCamera");
                CameraChange2ChaseCam();

                //01. 죽은 Player나오기
                ONCharacter();
                Vector3 characterCamPos = transform.position - Vector3.back;
                chaseCamera.transform.position = Vector3.Lerp(chaseCamera.transform.position, characterCamPos, Time.deltaTime * 5);

                //02. Enemy로 Cam이동
                Vector3 dir2Enemy = EJPSHP.instance.murdererPos - chaseCamera.transform.position;
                dir2Enemy.Normalize();

                // transform.parent = null;
                //Vector3 murdererCamPos = EJPSHP.instance.murdererPos + (-dir2Enemy /*+ Vector3.up * 0.5f + Vector3.*/);
                Vector3 murdererCamPos = EJPSHP.instance.murdererPos - (dir2Enemy * 3);

                chaseCamera.transform.position = Vector3.Lerp(characterCamPos, murdererCamPos, Time.deltaTime * 5);

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
        //print("메인카메라가 꺼졌습니다");

    }
    public void ONMainCamera()
    {
        GetComponent<Camera>().enabled = true;
        mainCameraON = true;
        //print("메인카메라가 켜졌습니다");
    }

    public void OFFChaseCamera()
    {
        GameObject chaseCamera = GameObject.FindWithTag("ChaseCamera");
        chaseCamera.GetComponent<Camera>().enabled = false;
        chaseCameraON = false;
        //print("추적카메라가 꺼졌습니다");
    }

    public void ONChaseCamera()
    {
        GameObject chaseCamera = GameObject.FindWithTag("ChaseCamera");
        chaseCamera.GetComponent<Camera>().enabled = true;
        chaseCameraON = true;
        //print("추적카메라가 켜졌습니다");
    }
}

