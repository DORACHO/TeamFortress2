using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJPlayerMove : MonoBehaviour
{
    Animator anim;
    CharacterController cc;

    //Move ����
    float speed = 5.71f;
    float speedForward;
    float speedBackward;
    float speedCrawl;

    float knockbackSpeed = 5;

    //Jump ����
    float jumpPower = 5;
    float gravity = -9.81f;
    float yVelocity;

    int jumpCount;
    int maxJumpCount = 1;

    //cam 
    Camera cam;

    //����üũ Ŭ����
    public EJPlayerAnim stateMgr;

    //bool isRun = false;
    bool isClickCtrl = false;

    //Jump üũ�� �ϰ� ������ �� �Դ� �� ����
    //isGrounded�� ��� ���°� ���� ���� �����ΰ�?
    bool isJumpingNow = false;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();

        //Move���� �ʱ�ȭ
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

        //���࿡ �����̴� ���¶��
        if(h != 0 || v != 0)
        {
            print("�����̰� �ֽ��ϴ�!");
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
            //print("�������ֽ��ϴ�!");
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


        //Move ���⿡ ���� �ӵ� ����
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
            //ī�޶� ��ġ(�ü�)�� ��������.
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
    public void Teleport(Vector3 exitPos)
    {
        cc.enabled = false;
        transform.position = exitPos;
        cc.enabled = true;
    }


}
