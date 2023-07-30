using JetBrains.Annotations;
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
        //public GameObject VFX;
        private AudioSource audioSource;
        public AudioClip HealClip;
        float currTime = 0;

        public int bulletCount;
        public int maxBulletCount = 10;

        public GameObject LaserEffect;
        

        // Start is called before the first frame update
        void Start()
        {
            nextFireTime = Time.deltaTime + fireRate;
            //�÷��̾��� ���̾� �� ����
            playerlayer = LayerMask.NameToLayer("Player");
            audioSource = GetComponent<AudioSource>();
            //VFX.SetActive(false);
            LaserEffect.SetActive(false);
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

            // �߻���¿�����
            //����ĳ��Ʈ�� �� ĳ���Ͱ� ����� �� Ray�� Ÿ�ٿ��� ���� �� �� �ִ�.
            // ���� �� �� ������ �ѽ�� ������ nextFireTime���� �ϰ�ʹ�.
            // �ִ� �߻� Ƚ���� maxBulletCount�� �ϰ�ʹ�.
            // �� ����� ���ε� ���·� �����ϰ�ʹ�.

            // ���ε� ���¶�� ���ε� �ð����� �ƹ��͵� �����ʴٰ�. �ð��� �ٵǸ� bulletCount�� 0���� �ʱ�ȭ �ϰ�
            // �����·� �����ϰ�ʹ�.
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
            // �����¿��� ������ ����ĳ��Ʈ�� �� ĳ���Ͱ� ����� �� �߻���·� �����ϰ�ʹ�.
            Debug.DrawRay(firePosition.position, firePosition.forward * 20f, Color.green);

            //float boxSize = 1f;
            //����ĳ��Ʈ�� ����� ��ü�� ������ ������ ����
            RaycastHit hit;

            if (Physics.Raycast(firePosition.position, //���� �߻� ���� ��ǥ
                                firePosition.forward,  // ���� �߻� ���� ����
                                out hit,               //����� ��ü�� ������ ��ȯ���� ����
                                1 << playerlayer))     // ������ ���̾�
            {
                state = State.Fire;
                //����ƾ ����
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
                //�ڷ�ƾ ��ž
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
                LaserEffect.SetActive(true);
                LaserEffect.transform.position = firePosition.position;
                //VFX.SetActive(true);
                //VFX.transform.position = firePosition.position;
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

  
