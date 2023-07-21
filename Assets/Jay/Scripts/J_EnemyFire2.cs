using JetBrains.Rider.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Raycast hit info가 플레이어에 맞으면 
//총을 쏘고싶다(총 = 힐)
//힐을 받게되면 HP가 올라간다
//HP가 올라가면 MP는 줄어든다


// ObjectPool처리하고싶다
// 태어날 때 총알을 미리 만들어서 총알목록에 등록하고 비활성화 해놓고
// 총알이 발사될때 목록에서 하나 가져와서 활성화하고싶다
// 총알이 화면밖에 나가거나 적과 부딪혔다면 비활성화하고싶다
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


       
        private int loadBullets = 15;                                   // 장전되어 있는 총알 개수
        private int numberOfAmmunition = 36;                            // 소지하고 있는 총알 개수(장전되지 않은)
        private float x = 0;                                            // 반동 회복에 사용할 x축 회전값 변수
        private float damage = 0f;
        private float timeAfterFire = 0f;                               // 총알 발사 후 지난 시간
        private float curTime = 0f;                                     // 총알 장전시간, 반동 회복 시간에 사용할 시간 변수
        private float sumReboundAngle = 0f;                             // 총 반동 각도
        private bool isLoading = false;                                 // 현재 장전중인지 확인

        private bool isRebounding = false;                              // 반동을 회복했는지 확인(true일 때 반동 회복중)

        //공격
        private readonly float attackInterval = 0.15f;                  // 공격 간격

        // 총알 장전 
        private readonly int maxLoadBulletCount = 12;                   // 한 번에 장전 가능한 총알 개수
        private readonly float loadingTime = 1.25f;                     // 장전 시간

        // 반동
        private readonly float reboundAngle = -10f;                     // 반동으로 총올라갈 총의 각도
        private readonly float reboundRecoveryTime = 0.1f;              // 반동 회복 시간

        // 레이캐스트 관련 변수
        private RaycastHit hit;
        private readonly float rayDistance = 10f;                       // 공격이 닿을 수 있는 최대 거리

        // 에디터에서 연결해줄 변수
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

            // 총구의 앞방향으로 레이를 쏘고, 거리에 따라 데미지 측정
            Debug.DrawRay(firePos.position, firePos.forward * rayDistance, Color.red);
            if (Physics.Raycast(firePos.position, firePos.forward, out hit, rayDistance))
            {
                //SetDamage(Vector3.Distance(transform.position, hit.point), isCritical);
            }

            // 반동주기, 측정된 데미지 출력, 장전된 총알 하나씩 빼기, 사용된 변수들 초기화해주기
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
