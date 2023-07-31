using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    //teleportZoom변수
    float originFOV = 60f;
    float zoomDuration = 1f;
    float zoomTimer = 0f;
    bool isZooming;

    public Image UIwhiteDissolve;
    float originAlpha = 0f;
    bool isDissolved;


    //Audio 변수
    public AudioSource fireSFX;
    public AudioSource reloadSFX;

    //사망 시, Model 체인지
    public GameObject armModel;
    public GameObject fullbodyModel;
    public TextMeshProUGUI countDownNum;
    public GameObject respawnPoint;
    bool respawned = false;
    GameObject playerCamParent;

    float countTime;

    // Start is called before the first frame update
    void Start()
    {
        //rx, ry 값을 나의 초기 eulerAngle 값을 넣자 
        ry = player.transform.localEulerAngles.y;

        //cx 초기화
        //targetCX = 6;
        targetCX = transform.localEulerAngles.x;

        playerCamParent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        countTime += Time.deltaTime;

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

            GameManager.instance.BlueKillRed();

            

            //StartCoroutine(RespawnincameraScript22());
            StartCoroutine(RespawnincameraScript33());
            respawned = true;
                       
            //CountDowndeltaTime(10);
            //RespawnInvoke();

           // StopAllCoroutines();
        }
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
        cx -= 10;
        print("카메라가 반동합니다");
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

            OFFMainCamera();
            ONChaseCamera();
               
    }

    public void CameraChange2MainCam()
    {
            print("mainCam으로 바뀌었습니다");
            ONMainCamera();
            OFFChaseCamera();

    }

   
    public void Whoareyou()
    {

            //Update에서 호출되지 않는 법 고민해야한다!...
            //EJPSHP.instance.Dead();   
            //print("whoareyou 실행중입니다");
                GameObject chaseCamera = GameObject.FindWithTag("ChaseCamera");

                CameraChange2ChaseCam();
                //01. 죽은 Player나오기
                /*ONCharacter();
                Vector3 characterCamPos = transform.position - Vector3.back;*/

                //02. Enemy로 Cam이동
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

            //01. 죽은 Player나오기
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

            //02. Enemy로 Cam이동
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

        //01. 죽은 Player나오기
        ONFullBodyModel();
        OFFArmBodyModel();

        EJSFX.instance.PlayKilledSFX();

        Vector3 characterCamPos = transform.position - chaseCamera.transform.forward * 5;

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

        //02. Enemy로 Cam이동
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

    public void TeleportZoomIn()
    {
        if (!isZooming)
        {
            zoomTimer = 0f;
            isZooming = true;
            originFOV = GetComponent<Camera>().fieldOfView;
        }
    }


    //Camera FOV를 75에서 60으로
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

    IEnumerator RespawnincameraScript33()
    {
        // CountDowndeltaTime(10);
        StartCoroutine(CountDownCoroutine(10));

        //CameraChange2ChaseCam();
        yield return StartCoroutine(Whoareyou1coroutine());
        yield return StartCoroutine(Whoareyou2coroutine());

        if (!mainCamON)
        {
            CameraChange2MainCam();
            mainCamON = true;
        }

        if (!rebirthCalled)
        {
            RespawnMove22();
            rebirthCalled = true;
        }

        AllFalse();
    }

   

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

    //02.시작 지점으로 Respawn
    public void RespawnMove()
    {
        print("RespawnMove");
        
        GameObject player = transform.parent.gameObject;
   

        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = respawnPoint.transform.position + Vector3.up* 5;
        
        //player.GetComponent<CharacterController>().enabled = true;
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

    public void RespawnMove22()
    {
        print("RespawnMove");

        print(playerCamParent);

        playerCamParent.GetComponent<CharacterController>().enabled = false;
        playerCamParent.transform.position = respawnPoint.transform.position + Vector3.up * 5;


        if (!teleportzoom)
        {
            TeleportZoomIn();
            TeleportDissolve();
            EJPSHP.instance.HP = EJPSHP.instance.normalMaxHP;
            ONArmBodyModel();
            OFFFullBodyModel();
            teleportzoom = true;
        }

        playerCamParent.GetComponent<CharacterController>().enabled = true;
    }

    //03.Rebirth
    public void RebirthinCamScript()
    {
        GameObject player = transform.parent.gameObject;
        print("Player에 담긴 것은"+player);
        player.transform.position = respawnPoint.transform.position + Vector3.up * 5;
        print("Respawn포인트는" + respawnPoint.transform);
        EJPSHP.instance.HP = EJPSHP.instance.normalMaxHP;

        ONArmBodyModel();
        OFFFullBodyModel();
    }

}