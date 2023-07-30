using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EJPSHP : MonoBehaviour
{
    public EJPlayerAnim4fullbody stateMgr;
    public static EJPSHP instance;
    public Animator characterAnim;

    public GameObject playerRespawnPoint;

    private void Awake()
    {
        instance = this;
    }

    public float currentHP = 125;

    public float normalMaxHP = 125;
    public float overcuredHP = 185;
    public float emergencyOvercuredHP = 158;

    public TextMeshProUGUI hpText;

    //죽었을 때 countDown
    public TextMeshProUGUI countDownNum;

    // Start is called before the first frame update
    void Start()
    {
        HP = normalMaxHP;       
    }

    // Update is called once per frame
    void Update()
    {
        RestrictHP();

/*         if (HP <= 0)
        {           
            //StartCoroutine(orderedRespawn());
            //StartCoroutine(orderedRespawn2());
        }*/



        //medic호출
        if (Input.GetKeyDown(KeyCode.M))
        {
            EJSFX.instance.CallMedicSFX();
        }
    }


    public float HP
    {
        get
        {
            return currentHP;
        }
        set
        {
            currentHP = value;
            hpText.text = $"{value}";            
        }
    }

    public Vector3 murdererPos;      
    public void SetHP(int damage, Vector3 position)
    {
        if (HP > 0)
        {
            HP -= damage;
            murdererPos = position;
        }
    }

    public void RestrictHP()
    {
        if (HP < 0)
        {
            HP = 0;
        }
        if (HP > overcuredHP)
        {
            HP = overcuredHP;
        }
    }




/*    public void Dead()
    {
     
            print("플레이어가 죽었습니다");
            EJPlayerModelChangeState.instance.OFFArmModel();
            EJPlayerModelChangeState.instance.ONFullBodyModel();

            characterAnim.SetTrigger("Death");

            Invoke(nameof(Rebirth), 5);

    }*/


    public void Rebirth()
    {
        //HP를 회복시켜 둔다.
        HP = normalMaxHP;

        //리스폰 위치 나중에 수정
        transform.position = playerRespawnPoint.transform.position + Vector3.up*5;
        print("다시 태어났습니다");       
    }
    
    IEnumerator orderedRespawn()
    {
        //10초 카운트다운
        
        StartCoroutine(CountDownCoroutine(10));

        GetComponentInChildren<EJCameraRotate>().Whoareyou();


        yield return new WaitForSeconds(3f);

        GetComponentInChildren<EJCameraRotate>().CameraChange2MainCam();

       
        //두 줄 실행이 안되는데 모르겠음
        //GetComponentInChildren<EJCameraRotate>().TeleportZoomIn();
       //GetComponentInChildren<EJCameraRotate>().TeleportDissolve();

        Rebirth();

        yield return new WaitForSeconds(0.5f);
        
    }


    bool countDownCalled;
    bool rebirthCalled;
    bool whoareyou1Called;
    bool whoareyou2Called;
    bool mainCamON;

    IEnumerator orderedRespawn2()
    {
        //10초 카운트다운
        if (!countDownCalled)
        {
            StartCoroutine(CountDownCoroutine(10));
            //stateMgr.ChangeState4Fullbody(EJPlayerAnim4fullbody.State.Dead);
            countDownCalled = true;
        }

 
        StartCoroutine(GetComponentInChildren<EJCameraRotate>().Whoareyou1coroutine());

        yield return new WaitForSeconds(2f);

        //GetComponentInChildren<EJCameraRotate>().Whoareyou2();

        StartCoroutine(GetComponentInChildren<EJCameraRotate>().Whoareyou2coroutine());
        

        yield return new WaitForSeconds(3f);

        GetComponentInChildren<EJCameraRotate>().CameraChange2MainCam();

        //두 줄 실행이 안되는데 모르겠음
        //GetComponentInChildren<EJCameraRotate>().TeleportZoomIn();
        //GetComponentInChildren<EJCameraRotate>().TeleportDissolve();
        

        if (!rebirthCalled)
        {
            Rebirth();
            rebirthCalled = true;
        }

        yield return new WaitForSeconds(0.5f);

    }

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

    public void Whoareyou1inthisScript()
    {
        GetComponentInChildren<EJCameraRotate>().Whoareyou1();
        print("whoareyou1이 실행되었습니다");
    }

    public void Whoareyou2inthisScript()
    {
        GetComponentInChildren<EJCameraRotate>().Whoareyou2();
    }

    public void modelchange()
    {
        GetComponentInChildren<EJCameraRotate>().OFFFullBodyModel();
        GetComponentInChildren<EJCameraRotate>().ONArmBodyModel();
    }

}
