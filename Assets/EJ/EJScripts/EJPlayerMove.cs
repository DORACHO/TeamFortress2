using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJPlayerMove : MonoBehaviour
{
    Animator anim;
    CharacterController cc;

    //Move 변수
    float speed = 5.71f;
    float speedForward;
    float speedBackward;
    float speedCrawl;

    float knockbackSpeed = 5;

    //Jump 변수
    float jumpPower = 5f;
    float gravity = -9.81f;
    //-9.81f
    float yVelocity;

    int jumpCount;
    int maxJumpCount = 1;

    //cam 
    Camera cam;

    //상태체크 클래스
    public EJPlayerAnim stateMgr;

    //bool isRun = false;
    bool isClickCtrl = false;

    //Jump 체크를 하고 싶은데 안 먹는 거 같음
    //isGrounded가 계속 상태가 같지 않은 이유인가?
    bool isJumpingNow = false;

    //walking SFX
    public AudioSource walkingSFXSource;
    //public AudioClip walkingSFX;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();

        //Move변수 초기화
        speedForward = speed;
        speedBackward = speed / 100 * 90;
        speedCrawl = speed / 100 * 33;

        cam = Camera.main;

        anim = GetComponentInChildren<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        //Move
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        
        //만약에 움직이는 상태라면
        if(h != 0 || v != 0)
        {
            if (walkingSFXSource.isPlaying == false)
            {
                //GetComponentInChildren<EJSFX_Walk>().PlayWalkSFX();
                walkingSFXSource.Play();
            }

            //if(isClickCtrl)
            //{
            //    stateMgr.ChangeState(EJPlayerAnim.State.Crouch);
            //}
            //else
            //{
            //    stateMgr.ChangeState(EJPlayerAnim.State.Run);
            //}
            //anim.SetBool("IsRun", true);

            //if(isRun == false)
            //{
            //    anim.SetTrigger("Run");
            //    isRun = true;
            //}
        }
        else
        {
            walkingSFXSource.Stop();
            //print("정지해있습니다!");
            //if (isClickCtrl)
            //{
            //    stateMgr.ChangeState(EJPlayerAnim.State.CrouchIdle);
            //}
            //else
            //{
            //    stateMgr.ChangeState(EJPlayerAnim.State.Idle);
            //}

            //anim.SetBool("IsRun", false);

            //if (isRun == true)
            //{
            //    anim.SetTrigger("Idle");
            //    isRun = false;
            //}
        }


        //Move 방향에 따른 속도 조건
        if (v > 0)
        {
            speed = speedForward;
        }
        else if (v < 0)
        {
            speed = speedBackward;
        }

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            isClickCtrl = true;
            speed = speedCrawl;
            //stateMgr.ChangeState(EJPlayerAnim.State.Crouch);
            //anim.SetTrigger("Crawl");

            //Crawl
            //카메라 위치(시선)이 내려간다.
            //cam.transform.localPosition = new Vector3(0, -0.5f, 0); 
        }
        else
        {
            isClickCtrl = false;
        }

        //Move
        Vector3 dir = new Vector3(h, 0, v);
        dir = cam.transform.TransformDirection(dir);
        dir.y = 0;
        dir.Normalize();

        Vector3 velocity = dir * speed;
        velocity.y = yVelocity;
        //print(velocity.magnitude);
        //Jump
        yVelocity += gravity * Time.deltaTime;

        if (cc.isGrounded)
        {
            jumpCount = 0;
            yVelocity = 0;
            isJumpingNow = false;
        }
        else 
        {
            isJumpingNow = true;
        }
        
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            yVelocity = jumpPower;
            //anim.SetTrigger("Jump");
            //stateMgr.ChangeState(EJPlayerAnim.State.Jump);
            jumpCount++;
        }
        cc.Move(velocity * Time.deltaTime);
    }

    //Teleport
    //시간 지연 시키기
    //CamZoomIn
    //dissolve 흰화면 crossdissolve
    //파티클 효과
    public void Teleport(Vector3 exitPos)
    {
        cc.enabled = false;
        transform.position = exitPos;
        cc.enabled = true;
    }
}
