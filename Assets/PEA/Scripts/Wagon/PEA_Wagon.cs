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


    // 웨이포인트들 중에 체크포인트의 인덱스값
    private int checkPointIndex = 27;

    private enum State
    {
        Wait,
        Move
    }

    private State state = State.Wait;

    //private bool isMoving = false;
    private bool isArrived = false;
    private bool isFinished = false;
    private bool isNearRed = false;                             // 근처에 레드팀원이 있는지 확인
    private bool isBacking = false;                             // 역행중인지 확인
    private bool isDepart = false;                              // 가장 처음 출발했는지 확인(출발 전에는 stopTime을 재지 않기 위한 변수)

    private readonly int maxBlueNum = 1;
    private readonly float maxSpeed = 2f;
    private readonly float turnSpeed = 5f;
    private readonly float backingSpeed = 1f;
    private readonly float maxStopTime = 10f;                  // 그 자리 그대로 멈춰있을 수 있는 시간

    private Transform target;
    private Vector3 dir;
    private NavMeshAgent nav;
    private RaycastHit hit;

    private List<Collider> nearReds = new List<Collider>();

    public Transform[] wayPoints;
    public PEA_ScoutMove[] scoutMove;

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
        SetTarget(IsBacking);
    }

    // Update is called once per frame
    void Update()
    {
        //print(target.transform.gameObject);
        //print(state);
        //print("isBacking : " + isBacking);
        //print("isNearRed" + isNearRed);
        //print("isMoving : " + isMoving);

        // 문이 열리기 전엔 움직이지 않음
        if (GameManager.instance.IsStart)
        {
            // 출발 전이고 근처에 블루팀원이 있으면 속도 설정
            if (!isDepart && blueNum != 0)
            {
                SetSpeed();
            }


            if (!isFinished)
            {               
                Rotate();
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
                        break;
                }

                if (isArrived)
                {
                    SetTarget(isBacking);
                }
            }
        }
    }

    // 이동할 타겟 설정
    private void SetTarget(bool isBacking)
    {
        // 역행중이면 타겟을 이전으로 돌려주고 정주행중이면 다음 타겟으로 넘어감
        if (isBacking && step > 0)
        {
            step--;
        }
        else
        {
            step++;
        }

        targetIndex = step;
        target = wayPoints[targetIndex];
        nav.SetDestination(target.position);
        isArrived = false;
    }

    // 스피드 조절
    private void SetSpeed()
    {
        //  역행중일 때
        if (isBacking)
        {
            // 체크포인트 도착하기 전이면 역행속도
            if (!isArrived)
            {
                nav.speed = backingSpeed;
            }

            // 체크포인트 도착했으면 멈추기
            else
            {
                nav.speed = 0f;
            }
        }

        //역행 중이 아닐 때는 근처에 있는 블루팀 수만큼 비레한 속도
        else
        {
            print(blueNum);
            if(blueNum > 0)
            {
                print(speed);
                nav.speed = maxSpeed;
            }
            else
            {
                nav.speed = 0;
            }
            //nav.speed = maxSpeed * ((float)blueNum / maxBlueNum);
        }
    }


    // 회전
    private void Rotate()
    {
        dir = target.position - transform.position;
        if (isBacking)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, turnSpeed * Time.deltaTime); 
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, turnSpeed * Time.deltaTime);
        }
        //dir.x = 0f;

        //transform.localEulerAngles = nav.velocity;

        //if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, LayerMask.NameToLayer("Rail")))
        //{
        //    //print(hit.transform.name);
        //    //print(transform.forward);
        //    //print(hit.transform.forward);
        //    transform.eulerAngles = new Vector3(hit.transform.localEulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        //    //transform.forward = hit.transform.forward;
        //    //print(hit.transform.gameObject);
        //    //transform.Rotate(hit.transform.eulerAngles.x, 0, 0);

        //    //Quaternion q = Quaternion.LookRotation(hit.transform.forward, hit.normal);

        //    //transform.rotation = Quaternion.Lerp(transform.rotation, q, turnSpeed * Time.deltaTime); ;

        //    //transform.LookAt(hit.transform.forward * 10, hit.normal);

        //}
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
        if (distance <= 1f)
        {
            if (!isBacking)
            {
                // 체크포인트에 도착하면 스카웃들에게 알려줌
                if (targetIndex == checkPointIndex)
                {
                    for (int i = 0; i < scoutMove.Length; i++)
                    {
                        scoutMove[i].PassByCheckPoint();
                        UIManager.instance.ChangeCheckPointImage();
                    }
                }

                // 마지막 종착점에 도착하면 게임이 종료됨.
                else if (targetIndex == wayPoints.Length - 1)
                {
                    isFinished = true;
                    GameManager.instance.Goal();
                    return;
                }

                SetTarget(isBacking);
            }
            else
            {
                // 체크포인트까지 역행
                // 체크포인트에서는 멈춤
                if(targetIndex == checkPointIndex)
                {
                    isArrived = true;
                    //isMoving = false;
                    state = State.Wait;
                    //SetSpeed();
                }
                else
                {
                    SetTarget(isBacking);
                }
            }
        }
    }

    // 방치된 시간 계산
    private void CheckStopTime()
    {
        stopTime += Time.deltaTime;
        if (stopTime >= maxStopTime)
        {
            //isMoving = true;
            isBacking = true;
            stopTime = 0f;
            SetTarget(isBacking);
            //SetSpeed();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        // 문이 열리기 전에는 움직이지 않음
        if(!GameManager.instance.IsStart)
        {
            return;
        }

        // 블루팀
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            print(other.name);
            blueNum++;
            SetSpeed();

            // 주변에 적이 없으면
            if (!isNearRed)
            {
                // 역행중이었으면 다시 전진
                if (isBacking)
                {
                    isBacking = false;
                    //nav.updateRotation = true;
                    SetTarget(isBacking);
                }

                // 이동하기
                state = State.Move;
                //isMoving = true;

                if (!isDepart)
                {
                    isDepart = true;
                }
            }
        }
        // 레드팀
        else if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            // 죽은 상태의 스카웃은 무시함.
            if (other.TryGetComponent<PEA_ScoutMove>(out PEA_ScoutMove scoutMove))
            {
                if (scoutMove.IsDead)
                {
                    nearReds.Add(other);
                    redNum++;
                    isNearRed = true;
                    nearReds.Add(other);
                    //isMoving = false;
                    state = State.Wait;
                }
            }
            else if (other.TryGetComponent<PEA_ScoutMove_Standby>(out PEA_ScoutMove_Standby scoutMove_Standby))
            {
                if (scoutMove_Standby.IsDead)
                {
                    nearReds.Add(other);
                    redNum++;
                    isNearRed = true;
                    nearReds.Add(other);
                    //isMoving = false;
                    state = State.Wait;
                }
            }           
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 주변에 스카웃이 있다면 스카웃이 죽었는지 계속해서 확인함.
        if (isNearRed)
        {
            foreach(Collider collider in nearReds)
            {
                if(collider.TryGetComponent<PEA_ScoutMove>(out PEA_ScoutMove scoutMove))
                {
                    if (scoutMove.IsDead)
                    {
                        nearReds.Remove(other);
                    }
                }
                else if(collider.TryGetComponent<PEA_ScoutMove_Standby>(out PEA_ScoutMove_Standby scoutMove_Standby))
                {
                    if (scoutMove_Standby.IsDead)
                    {
                        nearReds.Remove(other);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 블루팀
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            stopTime = 0f;
            blueNum--;

            if (blueNum <= 0)
            {
                //isMoving = false;
                blueNum = 0;
                state = State.Wait;
            }
            SetSpeed();
        }
        // 레드팀
        else if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            redNum--;
            if (nearReds.Contains(other))
            {
                nearReds.Remove(other);
            }

            if (redNum <= 0)
            {
                redNum = 0;
                isNearRed = false;
                if (blueNum > 0)
                {
                    //isMoving = true;
                    isBacking = false;
                }
            }
        }
    }
}
