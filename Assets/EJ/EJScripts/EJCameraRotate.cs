using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //teleportZoom����
    float originFOV = 60f;
    float zoomDuration = 1f;
    float zoomTimer = 0f;
    bool isZooming;

    public Image UIwhiteDissolve;
    float originAlpha = 0f;
    bool isDissolved;


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


        //CameraChange2ChaseCam();
        //CameraChange2MainCam();
/*        if (EJPSHP.instance.HP <= 0)
        {
            Whoareyou();
        }*/

        if (isZooming && isDissolved)
        {
            ZoomCamera();
            WhiteDissolve();
        }


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
        cx -= 10;
        print("ī�޶� �ݵ��մϴ�");
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

            OFFMainCamera();
            ONChaseCamera();
               
    }

    public void CameraChange2MainCam()
    {
            print("mainCam���� �ٲ�����ϴ�");
            ONMainCamera();
            OFFChaseCamera();

            //EJPSHP.instance.Rebirth();

    }

    public bool isWhoareyouDone;
    public void Whoareyou()
    {

            //Update���� ȣ����� �ʴ� �� ����ؾ��Ѵ�!...
            //EJPSHP.instance.Dead();

   
                //print("whoareyou �������Դϴ�");
                GameObject chaseCamera = GameObject.FindWithTag("ChaseCamera");

                CameraChange2ChaseCam();
                //01. ���� Player������
                /*ONCharacter();
                Vector3 characterCamPos = transform.position - Vector3.back;*/

                //02. Enemy�� Cam�̵�
                Vector3 dir2Enemy = EJPSHP.instance.murdererPos - chaseCamera.transform.position;
                dir2Enemy.Normalize();

                Vector3 murdererCamPos = EJPSHP.instance.murdererPos - (dir2Enemy * 3);

                chaseCamera.transform.position = Vector3.Lerp(chaseCamera.transform.position, murdererCamPos, Time.deltaTime * 5);
                chaseCamera.transform.forward = Vector3.Lerp(chaseCamera.transform.forward, dir2Enemy, Time.deltaTime * 5);

        //chaseCamera.transform.LookAt(EJPSHP.instance.murdererPos);

        //Invoke(nameof(CameraChange2MainCam), 5);

    }
    public void Whoareyou1()
    {
        GameObject chaseCamera = GameObject.FindWithTag("ChaseCamera");
        CameraChange2ChaseCam();

        //01. ���� Player������
        ONFullBodyModel();
        OFFArmBodyModel();
        Vector3 characterCamPos = transform.position - Vector3.back;

        chaseCamera.transform.position = Vector3.Lerp(chaseCamera.transform.position, characterCamPos, Time.deltaTime * 5);
    }

    public void Whoareyou2()
    {

        GameObject chaseCamera = GameObject.FindWithTag("ChaseCamera");

        //02. Enemy�� Cam�̵�
        Vector3 dir2Enemy2 = EJPSHP.instance.murdererPos - chaseCamera.transform.position;
        dir2Enemy2.Normalize();

        Vector3 murdererCamPos = EJPSHP.instance.murdererPos - (dir2Enemy2 * 3);

        chaseCamera.transform.position = Vector3.Lerp(chaseCamera.transform.position, murdererCamPos, Time.deltaTime * 5);
        chaseCamera.transform.forward = Vector3.Lerp(chaseCamera.transform.forward, dir2Enemy2, Time.deltaTime * 5);

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

    public void TeleportZoomIn()
    {
        if (!isZooming)
        {
            zoomTimer = 0f;
            isZooming = true;
            originFOV = GetComponent<Camera>().fieldOfView;
        }
    }


    //Camera FOV�� 75���� 60����
    public void ZoomCamera()
    {
        zoomTimer += Time.deltaTime;
        float t = zoomTimer / zoomDuration;
        t = Mathf.Clamp01(t);

        float newFOV = Mathf.Lerp(originFOV + 15, originFOV, t);
        GetComponent<Camera>().fieldOfView = newFOV;
        EJSFX.instance.PlayTeleportSFX();

        if (t >= 1f)
        {
            isZooming = false;
            GetComponent<Camera>().fieldOfView = originFOV;
        }
    }

    public void TeleportDissolve()
    {
        if (!isDissolved)
        {
            zoomTimer = 0f;
            isDissolved = true;
        }
    }

    public void WhiteDissolve()
    {
        zoomTimer += Time.deltaTime;
        float t = zoomTimer / zoomDuration;
        t = Mathf.Clamp01(t);

        float NewAlphaValue = Mathf.Lerp(0.4f, originAlpha, t);

        Color whiteDissolve = UIwhiteDissolve.color;
        whiteDissolve.a = NewAlphaValue;
        UIwhiteDissolve.color = whiteDissolve;


        if (t >= 1f)
        {
            isDissolved = false;
            whiteDissolve.a = originAlpha;
            UIwhiteDissolve.color = whiteDissolve;
        }        
    }

    public void ONFullBodyModel()
    {
        GameObject fullbodymodel = GameObject.FindWithTag("fullBodyPlayer");
        fullbodymodel.SetActive(true);
    }

    public void OFFFullBodyModel()
    {
        GameObject fullbodymodel = GameObject.FindWithTag("fullBodyPlayer");
        fullbodymodel.SetActive(false);
    }

    public void ONArmBodyModel()
    {
        GameObject fullbodymodel = GameObject.FindWithTag("ArmBody");
        fullbodymodel.SetActive(true);
    }

    public void OFFArmBodyModel()
    {
        GameObject fullbodymodel = GameObject.FindWithTag("ArmBody");
        fullbodymodel.SetActive(false);
    }
}