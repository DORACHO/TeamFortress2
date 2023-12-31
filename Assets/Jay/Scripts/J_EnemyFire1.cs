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
    public class J_EnemyFire1 : MonoBehaviour
    {
        public static J_EnemyFire1 instance;

        private void Awake()
        {
            instance = this;
        }

        public enum State
        {
            Wait,
            Reloading,
            Fire,
        }
        public State state;
        
        //public float hillCheckDistance = 10f;
        public LayerMask playerLayer;
        private int playerlayer;
        private bool isFire = false;
        public float speed = 15f;
        public Transform firePosition;
        public GameObject bulletFactory;
        private GameObject bullet;
        private float nextFireTime;
        public float fireRate = 2f;
        private bool isReloading = false;
        public int baseHPIncrease = 1;
        public float HPIncrease;
        private BoxCollider boxCollider;
        public int maxHP = 185;
        //public int minMP = 0;
        public float MPDecrease;
        public float HPIncreaseTimer = 0f;
        public GameObject VFX;
        private AudioSource audioSource;
        public AudioClip HealClip;
        float currTime = 0;

        public int bulletCount;
        public int maxBulletCount = 10;



        // Start is called before the first frame update
        void Start()
        {
            nextFireTime = Time.deltaTime + fireRate;
            //플레이어의 레이어 값 추출
            playerlayer = LayerMask.NameToLayer("Player");
            audioSource = GetComponent<AudioSource>();
            VFX.SetActive(false);
        }
        

        // Update is called once per frame
        void Update()
        {
            switch (state)
            {
                case State.Wait:
                    UpdateWait();
                    break;
                case State.Reloading:
                    UpdateReloading();

                    break;
                case State.Fire:
                    UpdateFire();
                    break;
            }

            // 발사상태에서는
            //레이캐스트에 적 캐릭터가 닿았을 때 Ray로 타겟에게 총을 쏠 수 있다.
            // 총을 쏠 수 있을때 총쏘는 간격을 nextFireTime으로 하고싶다.
            // 최대 발사 횟수는 maxBulletCount로 하고싶다.
            // 다 쏘고나면 리로드 상태로 전이하고싶다.

            // 리로드 상태라면 리로드 시간동안 아무것도 하지않다가. 시간이 다되면 bulletCount를 0으로 초기화 하고
            // 대기상태로 전이하고싶다.
            if (!isReloading && isFire)
            {
                
                
            }
            else if (audioSource.isPlaying)
            {
                StopSound();
            }

            if (false == isFire)
            {
                //PlayHealSound();
                
            }
        }
 
        private void UpdateWait()
        {
            // 대기상태에서 할일은 레이캐스트에 적 캐릭터가 닿았을 때 발사상태로 전이하고싶다.
            Debug.DrawRay(firePosition.position, firePosition.forward * 20f, Color.green);

            //float boxSize = 1f;
            //레이캐스트에 검출된 객체의 정보를 저장할 변수
            RaycastHit hit;

            if (Physics.Raycast(firePosition.position, //광선 발사 원점 좌표
                                firePosition.forward,  // 광선 발사 방향 벡터
                                out hit,               //검출된 객체의 정보를 반환받을 변수
                                1 << playerlayer))     // 검출할 레이어
            {
                state = State.Fire;
                //코쿠틴 시작
                J_HPBackGround.Instance.StartImage();

            }
        }

        private void UpdateFire()
        {

            EJPSHP.instance.SetHP(-1, this.transform.position);
            currTime += Time.deltaTime;
            if (currTime >= 3)//nextFireTime)
            {
                state = State.Reloading;
                //코루틴 스탑
                J_HPBackGround.Instance.EndImage();
                if (bulletCount >= maxBulletCount)
                {
                    state = State.Reloading;
                }
                else
                {
                    Fire();
                    bulletCount++;
                }
                currTime = 0;
            }
        }

        private void UpdateReloading()
        {
            currTime += Time.deltaTime;
            if (currTime >= 1)
            {
                state = State.Wait;
                currTime = 0;
            }
        }



        public void Fire()
        {
            Ray ray = new Ray(firePosition.position, firePosition.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, playerLayer))
            {
                HPIncreaseTimer += Time.deltaTime;
                EJPSHP.instance.SetHP(-1, this.transform.position);
   
                VFX.SetActive(true);
                VFX.transform.position = firePosition.position;
                J_HPBackGround.Instance.StartImage();
                print("111111111111");
                J_ObjectPool.instance.Fire();
            }
            else
            {
                print("22222222222");
                J_HPBackGround.Instance.EndImage();
            }
        }
        void PlayHealSound()
        {
            print("asdf");
            if (!audioSource.isPlaying)
            {
                audioSource.clip = HealClip;
                audioSource.Play();
            }
        }
        void StopSound()
        {
            audioSource.Stop();
        }
    }
}

  
