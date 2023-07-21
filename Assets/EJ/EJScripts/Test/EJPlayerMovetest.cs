using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace movetest
{
    public class EJPlayerMove : MonoBehaviour
    {
        public Animator anim;
        CharacterController cc;

        //Move ����
        public float speed = 5.71f;
        public float speedForward;
        public float speedBackward;
        public float speedCrawl;

        float knockbackSpeed = 5;

        //Jump ����
        float jumpPower = 5;
        float gravity = -9.81f;
        float yVelocity;

        int jumpCount;
        public int maxJumpCount = 1;

        //cam 
        Camera cam;

        //player State
        public enum State
        {
            Idle, // ��� �ִ� �� �ٲ�
            Move,
            Fire, // ���� sub, �� change
            Damage,
            Death
        }

        // Start is called before the first frame update
        void Start()
        {
            cc = GetComponent<CharacterController>();

            //Move���� �ʱ�ȭ
            speedForward = speed;
            speedBackward = speed / 100 * 90;
            speedCrawl = speed / 100 * 33;

            cam = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateMoveNJump();
        }


        //MoveNJump
        public void UpdateMoveNJump()
        {
            //Move
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

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
                speed = speedCrawl;

                //Crawl
                //ī�޶� ��ġ(�ü�)�� ��������.
                //cam.transform.localPosition = new Vector3(0, -0.5f, 0); 
            }

            //Move
            Vector3 dir = new Vector3(h, 0, v);
            dir = cam.transform.TransformDirection(dir);
            dir.y = 0;
            dir.Normalize();

            Vector3 velocity = dir * speed;
            velocity.y = yVelocity;

            //Jump
            yVelocity += gravity * Time.deltaTime;

            if (cc.isGrounded)
            {
                jumpCount = 0;
                yVelocity = 0;
            }

            if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
            {
                yVelocity = jumpPower;
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
}
