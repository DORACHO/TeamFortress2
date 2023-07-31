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


    // ��������Ʈ�� �߿� üũ����Ʈ�� �ε�����
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
    private bool isNearRed = false;                             // ��ó�� ���������� �ִ��� Ȯ��
    private bool isBacking = false;                             // ���������� Ȯ��
    private bool isDepart = false;                              // ���� ó�� ����ߴ��� Ȯ��(��� ������ stopTime�� ���� �ʱ� ���� ����)

    private readonly int maxBlueNum = 1;
    private readonly float maxSpeed = 2f;
    private readonly float turnSpeed = 5f;
    private readonly float backingSpeed = 1f;
    private readonly float maxStopTime = 10f;                  // �� �ڸ� �״�� �������� �� �ִ� �ð�

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

        // ���� ������ ���� �������� ����
        if (GameManager.instance.IsStart)
        {
            // ��� ���̰� ��ó�� ��������� ������ �ӵ� ����
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

    // �̵��� Ÿ�� ����
    private void SetTarget(bool isBacking)
    {
        // �������̸� Ÿ���� �������� �����ְ� ���������̸� ���� Ÿ������ �Ѿ
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

    // ���ǵ� ����
    private void SetSpeed()
    {
        //  �������� ��
        if (isBacking)
        {
            // üũ����Ʈ �����ϱ� ���̸� ����ӵ�
            if (!isArrived)
            {
                nav.speed = backingSpeed;
            }

            // üũ����Ʈ ���������� ���߱�
            else
            {
                nav.speed = 0f;
            }
        }

        //���� ���� �ƴ� ���� ��ó�� �ִ� ����� ����ŭ ���� �ӵ�
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


    // ȸ��
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


    // �̵�
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
    
    // �������� �����ϸ� ���� Ÿ�� ����
    private void CheckArrived()
    {
        distance = Vector3.Distance(transform.position, target.position);
        if (distance <= 1f)
        {
            if (!isBacking)
            {
                // üũ����Ʈ�� �����ϸ� ��ī���鿡�� �˷���
                if (targetIndex == checkPointIndex)
                {
                    for (int i = 0; i < scoutMove.Length; i++)
                    {
                        scoutMove[i].PassByCheckPoint();
                        UIManager.instance.ChangeCheckPointImage();
                    }
                }

                // ������ �������� �����ϸ� ������ �����.
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
                // üũ����Ʈ���� ����
                // üũ����Ʈ������ ����
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

    // ��ġ�� �ð� ���
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

        // ���� ������ ������ �������� ����
        if(!GameManager.instance.IsStart)
        {
            return;
        }

        // �����
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            print(other.name);
            blueNum++;
            SetSpeed();

            // �ֺ��� ���� ������
            if (!isNearRed)
            {
                // �������̾����� �ٽ� ����
                if (isBacking)
                {
                    isBacking = false;
                    //nav.updateRotation = true;
                    SetTarget(isBacking);
                }

                // �̵��ϱ�
                state = State.Move;
                //isMoving = true;

                if (!isDepart)
                {
                    isDepart = true;
                }
            }
        }
        // ������
        else if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            // ���� ������ ��ī���� ������.
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
        // �ֺ��� ��ī���� �ִٸ� ��ī���� �׾����� ����ؼ� Ȯ����.
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
        // �����
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
        // ������
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
