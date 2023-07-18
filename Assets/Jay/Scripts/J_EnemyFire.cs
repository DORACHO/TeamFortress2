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
public class J_EnemyFire : MonoBehaviour
{
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


    // Start is called before the first frame update
    void Start()
    {
        nextFireTime = Time.deltaTime + fireRate;
        //플레이어의 레이어 값 추출
        playerlayer = LayerMask.NameToLayer("Player");
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(firePosition.position, firePosition.forward * 20f, Color.green);

        //float boxSize = 1f;
        //레이캐스트에 검출된 객체의 정보를 저장할 변수
        RaycastHit hit;

        if (Physics.Raycast(firePosition.position, //광선 발사 원점 좌표
                            firePosition.forward,  // 광선 발사 방향 벡터
                            out hit,               //검출된 객체의 정보를 반환받을 변수
                            1 << playerlayer))     // 검출할 레이어
        {
            isFire = true;
        }

        //레이캐스트에 적 캐릭터가 닿았을 때 자동발사
        if (!isReloading && isFire)
        {
            Fire();
            //PlayHealSound();
            currTime += Time.deltaTime;
            if (currTime >= nextFireTime)
            {
                //Fire();
                currTime = 0;
            }
        }
        else if (audioSource.isPlaying)
        {
            StopSound();
        }
    }

    void Fire()
    {
        Ray ray = new Ray(firePosition.position, firePosition.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, playerLayer))
        {
            //GameObject bullet = Instantiate(bulletFactory, firePosition.position, firePosition.rotation);
            //bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * speed;
            //HP를 증가시키자
            //int HPIncrease = baseHPIncrease ;
            //HP를 시간에 따라 증가시키자
            HPIncreaseTimer += Time.deltaTime;

            if (HPManager.instance.HP > maxHP)
            {
                HPManager.instance.HP = maxHP;
            }
            if (HPManager.instance.HP < maxHP)
            {
                HPManager.instance.HP += 10 * Time.deltaTime;
            }
            //if(MPManager.instance.MP > minMP)
            {
                //    MPManager.instance.MP -= 10 * Time.deltaTime;
            }
            // else
            {
                //    MPManager.instance.MP = 0;
            }
            VFX.transform.position = firePosition.position;
            J_ObjectPool.instance.Fire();
            //bullet.SetActive(true);
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
