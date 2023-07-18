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
        //�÷��̾��� ���̾� �� ����
        playerlayer = LayerMask.NameToLayer("Player");
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(firePosition.position, firePosition.forward * 20f, Color.green);

        //float boxSize = 1f;
        //����ĳ��Ʈ�� ����� ��ü�� ������ ������ ����
        RaycastHit hit;

        if (Physics.Raycast(firePosition.position, //���� �߻� ���� ��ǥ
                            firePosition.forward,  // ���� �߻� ���� ����
                            out hit,               //����� ��ü�� ������ ��ȯ���� ����
                            1 << playerlayer))     // ������ ���̾�
        {
            isFire = true;
        }

        //����ĳ��Ʈ�� �� ĳ���Ͱ� ����� �� �ڵ��߻�
        if (!isReloading && isFire)
        {
            Fire();
            PlayHealSound();
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
            //HP�� ������Ű��
            //int HPIncrease = baseHPIncrease ;
            //HP�� �ð��� ���� ������Ű��
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
