using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_ScatterGun : MonoBehaviour
{
    private enum State
    {
        Loading,
        Fire,
        Wait
    }

    private State state = State.Wait;

    private int randomCritical = 0;                                 // ũ��Ƽ������ �ƴ��� ������ ���� ����
    private int loadBullets = 6;                                    // �����Ǿ� �ִ� �Ѿ� ����
    private int numberOfAmmunition = 32;                            // �����ϰ� �ִ� �Ѿ� ����(�������� ����)
    private float damage = 0f;
    private float timeAfterFire = 0f;                               // �Ѿ� �߻� �� ���� �ð�
    private float curTime = 0f;                                     // �Ѿ� ������ ����� �ð� ����
    private bool isLoading = false;                                 // ���� ���������� Ȯ��
    private bool isCritical = false;                                // �̹� ������ ġ��Ÿ�� ������ �������� Ȯ��

    // ����
    private readonly int oneShotBullets = 10;                       // �� ���� �߻�Ǵ� źȯ �� 
    private readonly float attackInterval = 0.625f;                 // ���� ����

    // ġ��Ÿ 
    private readonly int criticalPercent = 2;                       // ġ��Ÿ �߻���

    // �Ÿ��� ��������
    private readonly float baseDamage = 6f;                         // �⺻ ������ (�߰Ÿ� ������)
    private readonly float maximunDamage = 10.5f;                   // �ٰŸ� ������
    private readonly float minimumDamage = 3f;                      // ��Ÿ� ������
    private readonly float criticalDamage = 18f;                    // ġ��Ÿ ������

    // �Ѿ� ���� 
    private readonly int maxLoadBulletCount = 6;                    // �� ���� ���� ������ �Ѿ� ����
    private readonly float firstLoadTime = 0.7f;                    // �ʱ� ���� �ð�( ù �� ���� �ð�)
    private readonly float continuousLoadTime = 0.5f;               // ���� ���� �ð�( ù �� ���� �� �ߴ� ���� �ð�)
    private readonly float loadingTime = 3.2f;                      // �� ���� �ð�

    // �ٰŸ� �߰Ÿ� ��Ÿ� ����
    private readonly float closeRange = 3f;
    private readonly float middleRange = 5f;
    private readonly float longRange = 10f;

    // ����ĳ��Ʈ ���� ����
    private RaycastHit hit;                                         
    private GameObject preHit;                                      // ���� hit
    private readonly float rayDistance = 10f;                       // ������ ���� �� �ִ� �ִ� �Ÿ�

    // �����Ϳ��� �������� ����
    public Transform[] firePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckTimeAfterFire();

        //if (Input.GetMouseButtonDown(0) && !isLoading && timeAfterFire >= attackInterval && loadBullets > 0)
        //{
        //    Fire();
        //}

        //if (Input.GetKeyDown(KeyCode.R) && loadBullets < maxLoadBulletCount)
        if (loadBullets < maxLoadBulletCount)
        {
            state = State.Loading;
        }

        if (state == State.Loading)
        {
            ReLoad();
        }
    }

    private void CheckTimeAfterFire()
    {
        timeAfterFire += Time.deltaTime;
    }

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
            else if(distance <= middleRange)
            {
                damage += baseDamage;
            }
            else 
            {
                damage += minimumDamage;
            }
        }
    }

    private void ReLoad()
    {
        print("Redloading");
        curTime += Time.deltaTime;

        if(curTime >= loadingTime)
        {
            numberOfAmmunition = numberOfAmmunition - (maxLoadBulletCount - loadBullets);
            loadBullets = maxLoadBulletCount;
            curTime = 0f;
            isLoading = false;
            print("ReLoaded");
        }
    }

    private void ShowDamage()
    {
        print(damage);
    }

    public void Fire()
    {
        if (state == State.Wait)
        {
            state = State.Fire;
        }
        else
        {
            return;
        }
        randomCritical = Random.Range(1, 100);
        if(randomCritical <= criticalPercent)
        {
            isCritical = true;
        }
        else
        {
            isCritical = false;
        }

        for(int i = 0; i < oneShotBullets; i++)
        {

            Debug.DrawRay(firePos[i].position, firePos[i].forward * rayDistance, Color.red);
            //Destroy(Instantiate(bulletPrefab, firePos[i].position, firePos[i].rotation), 0.625f);
            if(Physics.Raycast(firePos[i].position, firePos[i].forward, out hit, rayDistance))
            {
                print(hit.transform.gameObject);
                if(preHit == null)
                {
                    preHit = hit.transform.gameObject;
                }

                if (preHit == hit.transform.gameObject)
                {
                    SetDamage( Vector3.Distance(transform.position, hit.point), isCritical);
                }
            }
        }
        ShowDamage();
        loadBullets--;
        damage = 0;
        timeAfterFire = 0f;

        print("load Bullet : " + loadBullets);
        print("number Of Ammunition : " + numberOfAmmunition);
    }
}
