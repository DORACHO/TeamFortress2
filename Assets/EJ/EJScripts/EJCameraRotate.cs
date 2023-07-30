using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    //��� ��, Model ü����
    public GameObject armModel;
    public GameObject fullbodyModel;
    public TextMeshProUGUI countDownNum;
    public GameObject respawnPoint;
    bool respawned = false;

    float countTime;

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
        countTime += Time.deltaTime;

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

        if (EJPSHP.instance.HP <= 0 &&!respawned)
        {
            
            //StartCoroutine(orderedRespawn());
            //StartCoroutine(RespawnincameraScript());
            //StartCoroutine(CountDownCoroutine(10));
            StartCoroutine(RespawnincameraScript22());
            respawned = true;
            

            

            //CountDowndeltaTime(10);
            //RespawnInvoke();


           // StopAllCoroutines();
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

    }

   
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

            EJSFX.instance.PlayKilledSFX();

            

            Vector3 characterCamPos = transform.position + Vector3.back*5;

            chaseCamera.transform.position = Vector3.Lerp(chaseCamera.transform.position, characterCamPos, Time.deltaTime * 5);
        chaseCamera.transform.forward = Camera.main.transform.forward;
        

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
            OFFFullBodyModel();
            ONArmBodyModel();
                         
    }



    public IEnumerator Whoareyou1coroutine()
    {

        GameObject chaseCamera = GameObject.FindWithTag("ChaseCamera");
        CameraChange2ChaseCam();

        //01. ���� Player������
        ONFullBodyModel();
        OFFArmBodyModel();

        EJSFX.instance.PlayKilledSFX();

        Vector3 characterCamPos = transform.position + Vector3.back * 5;

        while (true)
        {
            chaseCamera.transform.position = Vector3.Lerp(chaseCamera.transform.position, characterCamPos, Time.deltaTime * 3.5f);


            if (Vector3.Distance(chaseCamera.transform.position, characterCamPos) < 0.5f)
            {
                break;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        //print("5mWHOAREYOU1");
        //chaseCamera.transform.forward = Camera.main.transform.forward;
        //chaseCamera.transform.LookAt(player.transform);

        //whoareyou1coroutineRunning = false;
        yield return new WaitForSeconds(2f);

    }

    public IEnumerator Whoareyou2coroutine()
    {

        GameObject chaseCamera = GameObject.FindWithTag("ChaseCamera");

        //02. Enemy�� Cam�̵�
        Vector3 dir2Enemy2 = EJPSHP.instance.murdererPos - chaseCamera.transform.position;
        dir2Enemy2.Normalize();

        Vector3 murdererCamPos = EJPSHP.instance.murdererPos - (dir2Enemy2 * 3);

        while (true)
        {

        chaseCamera.transform.position = Vector3.Lerp(chaseCamera.transform.position, murdererCamPos, Time.deltaTime * 5);
        chaseCamera.transform.forward = Vector3.Lerp(chaseCamera.transform.forward, dir2Enemy2, Time.deltaTime * 5);

            if (Vector3.Distance(chaseCamera.transform.position, murdererCamPos)<0.5f)
            {
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);

        }


        //whoareyou2coroutineRunning = false;
        yield return new WaitForSeconds(2f);
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
        //GameObject fullbodymodel = GameObject.FindWithTag("fullBodyPlayer");
        //fullbodymodel.SetActive(true);
        fullbodyModel.SetActive(true);
        print("fullbody ON");
    }

    public void OFFFullBodyModel()
    {
        //GameObject fullbodymodel = GameObject.FindWithTag("fullBodyPlayer");
        //fullbodymodel.SetActive(false);
        fullbodyModel.SetActive(false);
        print("fullbody OFF");
    }

    public void ONArmBodyModel()
    {
        //GameObject fullbodymodel = GameObject.FindWithTag("ArmBody");
        //fullbodymodel.SetActive(true);
        armModel.SetActive(true);
        print("armbody ON");
    }

    public void OFFArmBodyModel()
    {
        //GameObject fullbodymodel = GameObject.FindWithTag("ArmBody");
        //fullbodymodel.SetActive(false);
        armModel.SetActive(false);
        print("armbody OFF");
    }

    //****************respawnCoroutineSuccess****************//
    bool whoareyou1coroutineRunning = false;
    bool whoareyou2coroutineRunning = false;
    bool countDownCalled = false;
    bool rebirthCalled = false;
    bool respawnMove = false;

    bool whoareyou1Done = false;
    bool whoareyou2Done = false;

    bool mainCamON = false;


    IEnumerator RespawnincameraScript22()
    {
        // CountDowndeltaTime(10);
        StartCoroutine(CountDownCoroutine(10));

        yield return StartCoroutine(Whoareyou1coroutine());    
        yield return StartCoroutine(Whoareyou2coroutine());      
        
        if (!mainCamON)
        {
            CameraChange2MainCam();
            mainCamON = true;          
        }

        if (!rebirthCalled)
        {
            RespawnMove();
            rebirthCalled = true;
        }

        AllFalse();
    }
    


    /*void RespawnInvoke()
    {
        if (!whoareyou1Done)
        {
            Whoareyou1();
            whoareyou1Done = false;
        }

        Invoke(nameof(Whoareyou2), 2f);

        CameraChange2MainCam();

        Invoke(nameof(RespawnincameraScript),3f);


    }*/


    void CountDowndeltaTime(int seconds)
    {
        ONCountDown();
        countTime = 0;

        for (int i = seconds-1; i >= 0; i--)
        {
            if (countTime > 1f)
            {
                leftSeconds = i;
                countDownNum.text = $"{leftSeconds}";
                countTime = 0;
            }                       
        }

        OFFCountDown();
    }

    IEnumerator RespawnincameraScript()
    {

        if (!countDownCalled)
        {
            StartCoroutine(CountDownCoroutine(10));
            countDownCalled = true;
            //yield return null;
        }

        if (!whoareyou1coroutineRunning)
        {
            whoareyou1coroutineRunning = true;
            yield return StartCoroutine(Whoareyou1coroutine());
            //yield return null;
        }

        yield return new WaitForSeconds(2f);

        if (!whoareyou2coroutineRunning)
        {
            whoareyou2coroutineRunning = true;
            yield return StartCoroutine(Whoareyou2coroutine());
            //print("whoareyou2runned");
        }

        yield return new WaitForSeconds(3f);

        if (!mainCamON)
        {
            CameraChange2MainCam();
            mainCamON = true;
            yield return null;
        }

        if (!respawnMove)
        {
            RespawnMove();
            
            respawnMove = true;
            //yield return null;
        }


        yield return new WaitForSeconds(0.5f);

        AllFalse();
        //yield return null;
    }

    public void AllFalse()
    {
        //whoareyou1coroutineRunning = false;
        //whoareyou2coroutineRunning = false;
        countDownCalled = false;
        rebirthCalled = false;
        respawnMove = false;
        mainCamON = false;
        teleportzoom = false;
        respawned = false;
    }

    //01. CountDown
    int leftSeconds;
    private IEnumerator CountDownCoroutine(int seconds)
    {
        ONCountDown();
        for (int i = seconds; i >= 0; i--)
        {
            leftSeconds = i;
            countDownNum.text = $"{leftSeconds}";
            yield return new WaitForSeconds(1f);
        }
        OFFCountDown();
    }

    public void ONCountDown()
    {
        TextMeshProUGUI countDownText = countDownNum;
        countDownText.enabled = true;
    }

    public void OFFCountDown()
    {
        TextMeshProUGUI countDownText = countDownNum;
        countDownText.enabled = false;
    }

    bool teleportzoom = false;

    //02.���� �������� Respawn
    public void RespawnMove()
    {
        print("RespawnMove");
        
        GameObject player = transform.parent.gameObject;
        print(player);
        player.transform.position = respawnPoint.transform.position + Vector3.up* 5;

        if (!teleportzoom)
        {
        TeleportZoomIn();
        TeleportDissolve();
            EJPSHP.instance.HP = EJPSHP.instance.normalMaxHP;
            ONArmBodyModel();
            OFFFullBodyModel();
            teleportzoom = true;
        }
    }
    //03.Rebirth
    public void RebirthinCamScript()
    {
        GameObject player = transform.parent.gameObject;
        print("Player�� ��� ����"+player);
        player.transform.position = respawnPoint.transform.position + Vector3.up * 5;
        print("Respawn����Ʈ��" + respawnPoint.transform);
        EJPSHP.instance.HP = EJPSHP.instance.normalMaxHP;

        ONArmBodyModel();
        OFFFullBodyModel();
    }

}