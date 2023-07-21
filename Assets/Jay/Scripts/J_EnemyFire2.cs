using JetBrains.Rider.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Raycast hit info�� �÷��̾ ������ 
//���� ���ʹ�(�� = ��)
//���� �ްԵǸ� HP�� �ö󰣴�
//HP�� �ö󰡸� MP�� �پ���


// ObjectPooló���ϰ�ʹ�
// �¾ �� �Ѿ��� �̸� ���� �Ѿ˸�Ͽ� ����ϰ� ��Ȱ��ȭ �س���
// �Ѿ��� �߻�ɶ� ��Ͽ��� �ϳ� �����ͼ� Ȱ��ȭ�ϰ�ʹ�
// �Ѿ��� ȭ��ۿ� �����ų� ���� �ε����ٸ� ��Ȱ��ȭ�ϰ�ʹ�
namespace MedicAI
{
    public class J_EnemyFire2 : MonoBehaviour
    {
        private enum State
        {
            Loading,
            Fire,
            Wait,
        }

        private State state;


       
        private int loadBullets = 15;                                   // �����Ǿ� �ִ� �Ѿ� ����
        private int numberOfAmmunition = 36;                            // �����ϰ� �ִ� �Ѿ� ����(�������� ����)
        private float x = 0;                                            // �ݵ� ȸ���� ����� x�� ȸ���� ����
        private float damage = 0f;
        private float timeAfterFire = 0f;                               // �Ѿ� �߻� �� ���� �ð�
        private float curTime = 0f;                                     // �Ѿ� �����ð�, �ݵ� ȸ�� �ð��� ����� �ð� ����
        private float sumReboundAngle = 0f;                             // �� �ݵ� ����
        private bool isLoading = false;                                 // ���� ���������� Ȯ��

        private bool isRebounding = false;                              // �ݵ��� ȸ���ߴ��� Ȯ��(true�� �� �ݵ� ȸ����)

        //����
        private readonly float attackInterval = 0.15f;                  // ���� ����

        // �Ѿ� ���� 
        private readonly int maxLoadBulletCount = 12;                   // �� ���� ���� ������ �Ѿ� ����
        private readonly float loadingTime = 1.25f;                     // ���� �ð�

        // �ݵ�
        private readonly float reboundAngle = -10f;                     // �ݵ����� �ѿö� ���� ����
        private readonly float reboundRecoveryTime = 0.1f;              // �ݵ� ȸ�� �ð�

        // ����ĳ��Ʈ ���� ����
        private RaycastHit hit;
        private readonly float rayDistance = 10f;                       // ������ ���� �� �ִ� �ִ� �Ÿ�

        // �����Ϳ��� �������� ����
        public Transform firePos;
        
        public GameObject VFX;
        private AudioSource audioSource;
        public AudioClip HealClip;

        void Start()
        {
            
        }
        void Update()
        {
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

        private void CheckTimeAfterFire()
        {
            timeAfterFire += Time.deltaTime;
            if (timeAfterFire >= attackInterval)
            {
                state = State.Wait;
            }
        }

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

        void Fire()
        {
            if (state == State.Wait)
            {
                state = State.Fire;
            }
            else
            {
                return;
            }

            // �ѱ��� �չ������� ���̸� ���, �Ÿ��� ���� ������ ����
            Debug.DrawRay(firePos.position, firePos.forward * rayDistance, Color.red);
            if (Physics.Raycast(firePos.position, firePos.forward, out hit, rayDistance))
            {
                //SetDamage(Vector3.Distance(transform.position, hit.point), isCritical);
            }

            // �ݵ��ֱ�, ������ ������ ���, ������ �Ѿ� �ϳ��� ����, ���� ������ �ʱ�ȭ���ֱ�
            Rebound();
            //ShowDamage();
            loadBullets--;
            curTime = 0f;
            damage = 0;
            //timeAfterFire = 0f;

            print("load Bullet : " + loadBullets);
            print("number Of Ammunition : " + numberOfAmmunition);
        }

        private void ShowDamage()
        {
            
        }

        private void Rebound()
        {
            transform.localEulerAngles += new Vector3(reboundAngle, 0, 0);
            sumReboundAngle = transform.eulerAngles.x - 360f;
            x = sumReboundAngle;
            isRebounding = true;
        }

        private void ReboundRecovery()
        {
            x -= sumReboundAngle * (Time.deltaTime / reboundRecoveryTime);
            transform.localEulerAngles = new Vector3(x, 0, 0);
            if (x >= 0)
            {
                transform.localEulerAngles = Vector3.zero;
                isRebounding = false;
            }
        }
    }
}
