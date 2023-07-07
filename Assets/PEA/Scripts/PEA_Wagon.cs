using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PEA_Wagon : MonoBehaviour
{
    private int step = 0;
    private int redNum = 0;                                     // ��ó�� �ִ� �������� ��
    private int blueNum = 0;                                    // ��ó�� �ִ� ������� ��
    private int targetIndex = -1;
    private float speed = 2f;
    private float distance = 0f;
    private float stopTime = 0f;                                // ������ ��ġ�� �ð�

    private enum State
    {
        Wait,
        MoveForward,
        Backing
    }

    private State state = State.MoveForward;

    private bool isMoving = false;
    private bool isArrived = false;
    private bool isFinished = false;
    private bool isNearRed = false;                             // ��ó�� ���������� �ִ��� Ȯ��
    //private bool isBacking = false;                             // ���������� Ȯ��
    private bool isDepart = false;

    private readonly int maxBlueNum = 4;
    private readonly float maxSpeed = 2f;
    private readonly float turnSpeed = 5f;
    private readonly float backingSpeed = 1f;
    private readonly float maxStopTime = 5f;                   // �� �ڸ� �״�� �������� �� �ִ� �ð�

    private Transform target;
    private Vector3 dir;
    private NavMeshAgent nav;
    private RaycastHit hit;

    public Transform[] wayPoints;

    public int TargetIndex
    {
        get { return targetIndex; }
    }

    public bool IsFinished
    {
        get { return isFinished; }
    }

    public bool IsBacking
    {
        get 
        {
            return state == State.Backing;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.updateRotation = false;
        SetTarget();
    }

    // Update is called once per frame
    void Update()
    {
        //nav.SetDestination(wayPoints[wayPoints.Length - 1].position);

        //if(Physics.Raycast(transform.position, -transform.up, out hit, 2f))
        //{
        //    print(hit.normal);
        //    transform.Rotate(hit.transform.eulerAngles.x, 0, 0);

        //}

        print(target);
        //print("isBacking : " + isBacking);
        //print("isNearRed" + isNearRed);
        print("isMoving : " + isMoving);
        if (!isFinished)
        {
            print("isfinished = false");
            //    Rotate();
            //if (isMoving)
            //{
            //    //Move();
            //}
            //else if (blueNum == 0 && isDepart && !isBacking)
            //{
            //    CheckStopTime();
            //}

            //CheckArrived();

            switch (state)
            {
                case State.Wait:
                    if (blueNum == 0 && isDepart && !isArrived)
                    {
                        CheckStopTime();
                    }
                    break;

                case State.MoveForward:
                    CheckArrived();
                    break;

                case State.Backing:
                    CheckArrived();
                    break;
            }

            Rotate();

            if (isArrived)
            {
                print(" isarrived true");
                //SetTarget();
            }
        }
    }

    // �̵��� Ÿ�� ����
    private void SetTarget()
    {
        targetIndex = step;
        target = wayPoints[targetIndex];
        nav.SetDestination(target.position);
        isArrived = false;
    }

    private void SetSpeed()
    {
        if (state == State.Backing)
        {
            //speed = backingSpeed;
            if (!isArrived)
            {
                nav.speed = backingSpeed;
            }
            else
            {
                nav.speed = 0f;
            }
        }
        else
        {
            //speed = maxSpeed * ((float)blueNum / maxBlueNum);
            nav.speed = maxSpeed * ((float)blueNum / maxBlueNum);
        }
        //print(speed);
    }

    // ȸ��
    private void Rotate()
    {
        dir = target.position - transform.position;
        if (state == State.Backing)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-dir), turnSpeed * Time.deltaTime); 
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
        }
        //dir.x = 0f;

        //transform.localEulerAngles = nav.velocity;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            //nav.updateRotation = false;
            //print(hit.normal);
            transform.eulerAngles = new Vector3(hit.transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            //transform.Rotate(hit.transform.eulerAngles.x, 0, 0);

            //Quaternion q = Quaternion.LookRotation(hit.transform.forward, hit.normal);

            //transform.rotation = Quaternion.Lerp(transform.rotation, q, turnSpeed * Time.deltaTime); ;

            //transform.LookAt(hit.transform.forward * 10, hit.normal);

        }
    }

    // �̵�
    private void Move()
    {
        if (state == State.Backing)
        {
            transform.Translate(-transform.forward * speed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        }
    }

    // �������� �����ϸ� ���� Ÿ�� ����
    private void CheckArrived()
    {
        distance = Vector3.Distance(transform.position, target.position);
        if (distance <= 1.5f)
        {
            if (state == State.MoveForward)
            {
                // ������ �������� �����ϸ� ������ �����.
                if (targetIndex == wayPoints.Length - 1)
                {
                    isFinished = true;
                    //print("finish");
                    return;
                }
                //isArrived = true;
                step++;
                SetTarget();
            }
            else
            {
                isArrived = true;
                isMoving = false;
                state = State.Wait;
                SetSpeed();
            }
        }
    }

    // ��ġ�� �ð� ���
    private void CheckStopTime()
    {
        stopTime += Time.deltaTime;
        if (stopTime >= maxStopTime)
        {
            //isBacking = true;
            isMoving = true;
            state = State.Backing;
            step--;
            stopTime = 0f;
            nav.updateRotation = false;
            SetTarget();
            SetSpeed();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �����
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Blue")))
        {
            blueNum++;
            SetSpeed();

            if (!isNearRed)
            {
                // �������̾����� �ٽ� ����
                if (state == State.Backing)
                {
                    //isBacking = false;
                    //nav.updateRotation = true;
                    step++;
                    SetTarget();
                }
                isMoving = true;
                state = State.MoveForward;

                if (!isDepart)
                {
                    isDepart = true;
                }
            }
        }
        // ������
        else if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Red")))
        {
            redNum++;
            isNearRed = true;
            isMoving = false;
            state = State.Wait;
        }
        print("blue : " + blueNum + ", red : " + redNum);
    }

    private void OnTriggerExit(Collider other)
    {
        // �����
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Blue")))
        {
            stopTime = 0f;
            blueNum--;
            SetSpeed();

            if (blueNum <= 0)
            {
                isMoving = false;
                state = State.Wait;
            }
        }
        // ������
        else if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Red")))
        {
            redNum--;
            if (redNum <= 0)
            {
                redNum = 0;
                isNearRed = false;
                if (blueNum > 0)
                {
                    isMoving = true;
                    state = State.MoveForward;
                }
            }
        }
        print("blue : " + blueNum + ", red : " + redNum);
    }
}
