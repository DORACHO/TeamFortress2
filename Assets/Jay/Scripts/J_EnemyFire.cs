using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Raycast hit info�� �÷��̾ ������ 
//���� ���ʹ�(�� = ��)
//���� �ްԵǸ� HP�� �ö󰣴�
//HP�� �ö󰡸� MP�� �پ���

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
    public int minMP = 0;
    public float MPDecrease;
    public float HPIncreaseTimer = 0f;
    public GameObject VFX;
    private AudioSource audioSource;
    public AudioClip HealClip;


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
            if (Time.deltaTime >= nextFireTime)
            {
                Fire();
            }
        }

        {
            Fire();
        }

        void Fire()
        {
            Ray ray = new Ray(firePosition.position, firePosition.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, playerLayer))
            {
                PlayHealSound();
                GameObject bullet = Instantiate(bulletFactory, firePosition.position, firePosition.rotation);
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * speed;
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
                if(MPManager.instance.MP > minMP)
                {
                    MPManager.instance.MP -= 10 * Time.deltaTime;
                }
                else
                {
                    MPManager.instance.MP = 0;
                }
                VFX.transform.position = this.transform.position - new Vector3(5, 1, 0);
   
            }
        }
        void PlayHealSound()
        {
            if(audioSource != null && HealClip != null)
            {
                audioSource.PlayOneShot(HealClip);
            }
        }
    }
}
