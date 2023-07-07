using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_PistolGun : MonoBehaviour
{
    private enum State 
    {
        Loading,
        Fire,
        Wait
    }

    private State state = State.Wait;


    private int randomCritical = 0;                                 
    private int loadBullets = 12;                                   // �����Ǿ� �ִ� �Ѿ� ����
    private int numberOfAmmunition = 36;                            // �����ϰ� �ִ� �Ѿ� ����(�������� ����)
    private float x = 0;                                            // �ݵ� ȸ���� ����� x�� ȸ���� ����
    private float damage = 0f;
    private float timeAfterFire = 0f;                               // �Ѿ� �߻� �� ���� �ð�
    private float curTime = 0f;                                     // �Ѿ� �����ð�, �ݵ� ȸ�� �ð��� ����� �ð� ����
    private float sumReboundAngle = 0f;                             // �� �ݵ� ����
    private bool isLoading = false;                                 // ���� ���������� Ȯ��
    private bool isCritical = false;                                // �̹� ������ ġ��Ÿ�� ������ �������� Ȯ��
    private bool isRebounding = false;                              // �ݵ��� ȸ���ߴ��� Ȯ��(true�� �� �ݵ� ȸ����)

    //����
    private readonly float attackInterval = 0.15f;                  // ���� ����

    // ġ��Ÿ 
    private readonly int criticalPercent = 2;                       // ġ��Ÿ �߻���

    // �Ÿ��� ��������
    private readonly float baseDamage = 15f;                        // �⺻ ������ (�߰Ÿ� ������)
    private readonly float maximunDamage = 22f;                     // �ٰŸ� ������
    private readonly float minimumDamage = 8f;                      // ��Ÿ� ������
    private readonly float criticalDamage = 45f;                    // ġ��Ÿ ������

    // �Ѿ� ���� 
    private readonly int maxLoadBulletCount = 12;                   // �� ���� ���� ������ �Ѿ� ����
    private readonly float loadingTime = 1.25f;                     // ���� �ð�


    // �ٰŸ� �߰Ÿ� ��Ÿ� ����
    private readonly float closeRange = 3f;
    private readonly float middleRange = 5f;
    private readonly float longRange = 10f;

    // �ݵ�
    private readonly float reboundAngle = -10f;                     // �ݵ����� �ѿö� ���� ����
    private readonly float reboundRecoveryTime = 0.1f;              // �ݵ� ȸ�� �ð�

    // ����ĳ��Ʈ ���� ����
    private RaycastHit hit;
    private readonly float rayDistance = 10f;                       // ������ ���� �� �ִ� �ִ� �Ÿ�

    // �����Ϳ��� �������� ����
    public Transform firePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //print("eulerangle : " + transform.eulerAngles);
        //print("rotation : " + transform.rotation);

        //if (Input.GetMouseButtonDown(0) && !isLoading && timeAfterFire >= attackInterval && loadBullets > 0)
        //{
        //    Fire();
        //}

        //if (Input.GetKeyDown(KeyCode.R) && loadBullets < maxLoadBulletCount)
        if (loadBullets == 0 && maxLoadBulletCount != 0)
        {
            state = State.Loading;
        }

        if (isRebounding)
        {
            ReboundRecovery();
        }

        switch (state)
        {
            case State.Loading:
                ReLoad();
                break;

            case State.Fire:
                CheckTimeAfterFire();
                break;

            case State.Wait:
                break;
        }
    }

    // ���� �� �ð� ���
    private void CheckTimeAfterFire()
    {
        timeAfterFire += Time.deltaTime;
        if(timeAfterFire >= attackInterval)
        {
            state = State.Wait;
        }
    }

    // �Ÿ��� ������ ����(Fire���� ȣ��)
    public void SetDamage(float distance, bool isCritical)
    {
        if (isCritical)
        {
            damage += criticalDamage;
        }
        else
        {
            if (distance <= closeRange)
            {
                damage += maximunDamage;
            }
            else if (distance <= middleRange)
            {
                damage += baseDamage;
            }
            else
            {
                damage += minimumDamage;
            }
        }
    }

    // ������
    private void ReLoad()
    {
        print("Redloading");
        curTime += Time.deltaTime;

        if (curTime >= loadingTime)
        {
            numberOfAmmunition = numberOfAmmunition - (maxLoadBulletCount - loadBullets);
            loadBullets = maxLoadBulletCount;
            curTime = 0f;
            state = State.Wait;
            print("ReLoaded");
        }
    }

    // ������ ���(Fire���� ȣ��)
    private void ShowDamage()
    {
        print(damage);
    }

    // �� ���
    public void Fire()
    {
        if(state == State.Wait)
        {
            state = State.Fire;
        }
        else
        {
            return;
        }

        // �̹� �������� ����Ÿ�� ������ �������� ���Ѵ�
        randomCritical = Random.Range(1, 100);
        if (randomCritical <= criticalPercent)
        {
            isCritical = true;
        }
        else
        {
            isCritical = false;
        }

        // �ѱ��� �չ������� ���̸� ���, �Ÿ��� ���� ������ ����
        Debug.DrawRay(firePos.position, firePos.forward * rayDistance, Color.red);
        //Destroy(Instantiate(bulletPrefab, firePos[i].position, firePos[i].rotation), 0.625f);
        if (Physics.Raycast(firePos.position, firePos.forward, out hit, rayDistance))
        {
            SetDamage(Vector3.Distance(transform.position, hit.point), isCritical);
        }

        // �ݵ��ֱ�, ������ ������ ���, ������ �Ѿ� �ϳ��� ����, ���� ������ �ʱ�ȭ���ֱ�
        Rebound();
        ShowDamage();
        loadBullets--;
        curTime = 0f;
        damage = 0;
        //timeAfterFire = 0f;

        print("load Bullet : " + loadBullets);
        print("number Of Ammunition : " + numberOfAmmunition);
    }

    private void Rebound()
    {
        transform.Rotate(reboundAngle, 0, 0);
        sumReboundAngle = transform.eulerAngles.x - 360f;
        x = sumReboundAngle;
        isRebounding = true;
    }

    private void ReboundRecovery()
    {
        x -= sumReboundAngle * (Time.deltaTime / reboundRecoveryTime);
        transform.eulerAngles = new Vector3(x, 0, 0);
        if (x >= 0)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
            isRebounding = false;
        }
    }
}
