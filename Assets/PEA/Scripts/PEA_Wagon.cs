using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PEA_Wagon : MonoBehaviour
{
    private int step = 0;
    private int redNum = 0;                                     // 근처에 있는 레드팀원 수
    private int blueNum = 0;                                    // 근처에 있는 블루팀원 수
    private int targetIndex = -1;
    private float speed = 2f;
    private float distance = 0f;
    private float stopTime = 0f;                                // 수레가 방치된 시간

    private enum State
    {
        Wait,
        Move
    }

    private State state = State.Wait;

    private bool isMoving = false;
    private bool isArrived = false;
    private bool isFinished = false;
    private bool isNearRed = false;                             // 근처에 레드팀원이 있는지 확인
    private bool isBacking = false;                           // 역행중인지 확인
    private bool isDepart = false;                              // 가장 처음 출발했는지 확인(출발 전에는 stopTime을 재지 않기 위한 변수)

    private readonly int maxBlueNum = 4;
    private readonly float maxSpeed = 2f;
    private readonly float turnSpeed = 5f;
    private readonly float backingSpeed = 1f;
    private readonly float maxStopTime = 5f;                   // 그 자리 그대로 멈춰있을 수 있는 시간

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
            return isBacking;
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

        print(target.transform.gameObject);
        print(state);
        //print("isBacking : " + isBacking);
        //print("isNearRed" + isNearRed);
        //print("isMoving : " + isMoving);
        if (!isFinished)
        {
            //print("isfinished = false");
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

                case State.Move:
                    CheckArrived();
                    Rotate();
                    break;
            }


            if (isArrived)
            {
                //print(" isarrived true");
                //SetTarget();
            }
        }
    }

    // 이동할 타겟 설정
    private void SetTarget()
    {
        targetIndex = step;
        target = wayPoints[targetIndex];
        nav.SetDestination(target.position);
        isArrived = false;
    }

    private void SetSpeed()
    {
        if (isBacking)
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

    // 회전
    private void Rotate()
    {
        dir = target.position - transform.position;
        if (isBacking)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-dir), turnSpeed * Time.deltaTime); 
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
        }
        //dir.x = 0f;

        //transform.localEulerAngles = nav.velocity;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, LayerMask.NameToLayer("Rail")))
        {
            //nav.updateRotation = false;
            //print(hit.normal);
            transform.eulerAngles = new Vector3(hit.transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            //transform.forward = hit.transform.forward;
            //print(hit.transform.gameObject);
            //transform.Rotate(hit.transform.eulerAngles.x, 0, 0);

            //Quaternion q = Quaternion.LookRotation(hit.transform.forward, hit.normal);

            //transform.rotation = Quaternion.Lerp(transform.rotation, q, turnSpeed * Time.deltaTime); ;

            //transform.LookAt(hit.transform.forward * 10, hit.normal);

        }
    }

    // 이동
    private void Move()
    {
        if (isBacking)
        {
            transform.Translate(-transform.forward * speed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        }
    }

    // 경유지에 도착하면 다음 타겟 지정
    private void CheckArrived()
    {
        distance = Vector3.Distance(transform.position, target.position);
        if (distance <= 1.5f)
        {
            if (!isBacking)
            {
                // 마지막 종착점에 도착하면 게임이 종료됨.
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

    // 방치된 시간 계산
    private void CheckStopTime()
    {
        stopTime += Time.deltaTime;
        if (stopTime >= maxStopTime)
        {
            //isBacking = true;
            isMoving = true;
            isBacking = true;
            step--;
            stopTime = 0f;
            SetTarget();
            SetSpeed();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 블루팀
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            blueNum++;
            SetSpeed();

            if (!isNearRed)
            {
                // 역행중이었으면 다시 전진
                if (isBacking)
                {
                    //isBacking = false;
                    //nav.updateRotation = true;
                    step++;
                    SetTarget();
                }
                state = State.Move;
                isMoving = true;
                isBacking = false;

                if (!isDepart)
                {
                    isDepart = true;
                }
            }
        }
        // 레드팀
        else if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
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
        // 블루팀
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
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
        // 레드팀
        else if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            redNum--;
            if (redNum <= 0)
            {
                redNum = 0;
                isNearRed = false;
                if (blueNum > 0)
                {
                    isMoving = true;
                    isBacking = false;
                }
            }
        }
        print("blue : " + blueNum + ", red : " + redNum);
    }
}
