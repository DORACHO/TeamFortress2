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

        //Q.순서대로 가져오는 방법?
        fireSFX = GetComponent<AudioSource>();
        reloadSFX = GetComponent<AudioSource>();
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

       //UpdateCameraReact();
        cx = Mathf.Lerp(cx, targetCX, Time.deltaTime * 8);

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


    /*public void KnockBack()
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
                
    }*/

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

    //나를 죽인 사람을 어떻게 찾아서 담는지
    //마지막 총알의 주체
    //Parent를 가져오는 방법?

    GameObject murderer ;

    public void Whoareyou()
    {
        //transform.localEulerAngles = new Vector3(cx, 0, 0);

        if(EJPSHP.instance.HP <= 0)
        {
            transform.parent = null;

            //디스턴스가 일정 이상일 때

            //GetComponentInChildren<MeshRenderer>().enabled = false;
            transform.position = Vector3.Lerp(transform.position, EJPSHP.instance.murdererPos, 0.7f);
        }
    }

    public void PlayFireSFX()
    {
        fireSFX.Play();
    }

    public void PlayReloadSFX()
    {
        reloadSFX.Play();
    }
}
