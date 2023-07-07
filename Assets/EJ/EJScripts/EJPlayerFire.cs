using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EJPlayerFire : MonoBehaviour
{
    //�´� ��ġ�� ���� �Ѿ� ȿ�� ���� 
    enum bulletImpactName
    {
        Floor,
        Enemy
    }

    //�Ѿ� ȿ���� ����� ����
    public GameObject[] bulletImpactFactories;


    //Fire ������
    //������� �� �Ѿ��� ��
    int fireCount;
    int rulletFireCount;

    //������ �Ѿ�
    public int maxBulletRullet = 6;
    public int leftBulletRullet;

    //���� �Ѿ� ��
    public int leftBullet;
    //�� �Ѿ� �� (źâ���� ��ģ ��)
    public int maxbullet = 32;


    //�Ѿ� �ҷ��� ����ִ� �Ѿ� �� (max:6)
    public TextMeshProUGUI bulletRulletText;
    // �� ���� �Ѿ� ��
    public TextMeshProUGUI leftBulletText;

    //�ǰ� ���ʹ� ������
    bool isAttackHit;

    //�Ѿ� UI
    public TextMeshProUGUI hpText;


    // Start is called before the first frame update
    void Start()
    {
        leftBullet = maxbullet;
        leftBulletRullet = maxBulletRullet;
        //GameObject a = bulletImpactFactories[(int)bulletImpactName.Floor];
    }

    // Update is called once per frame
    void Update()
    {
        //�ʱ�ȭ����� �ϴ� ����?
        LEFT_BULLET = LEFT_BULLET;
        LEFT_BULLETRULLET = LEFT_BULLETRULLET;
        
        UpdateFire();
        UpdateBulletCharge();
    }

    // �Ѿ��� �������Ѵ�.
    private void UpdateBulletCharge()
    {
       if (Input.GetKeyDown(KeyCode.R))
        {
            fireCount = 0;
            leftBullet = maxbullet;
            
        }
    }

    //���� ��� �ִ�?
    bool canFire = true;
    float currentTime;
    float delayTime = 3.5f;

    private void UpdateFire()
    {
        // ���� �� ���ִٸ�
        if (canFire)
        {
            if (Input.GetButtonDown("Fire1") && fireCount < maxbullet)
            {
                currentTime += Time.deltaTime;

                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

                //�ǰ� ���̾�
                //1. enemy
                //2. Construct

                int layer = (1 << LayerMask.NameToLayer("Default"));
                RaycastHit hitInfo;

                fireCount++;
                rulletFireCount++;

                leftBullet = maxbullet- fireCount;
                leftBulletRullet = maxBulletRullet - rulletFireCount;

                //�����ִ� �Ѿ� ���� �ƽ� �ҷ���ŭ
                if(leftBulletRullet==0 && leftBullet >= 6)
                {
                    leftBulletRullet += maxBulletRullet;
                    rulletFireCount = 0;
                }else if(leftBulletRullet ==0 && leftBullet < 6)
                {
                    leftBulletRullet += leftBullet;
                    rulletFireCount = 0;
                }

                //���࿡ fireCount �� 6�� ������
                //���� ����� �Ѵ�.
                if (fireCount % maxBulletRullet == 0 || leftBullet ==0 )
                {
                    canFire = false;
                    Invoke(nameof(OffFire), delayTime);
                    //Invoke(nameof(bulletRulletCharge), delayTime);
                }
               
        

                if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
                {
                    GameObject bulletImpact = Instantiate(bulletImpactFactories[(int)bulletImpactName.Floor]);

                    bulletImpact.transform.localScale = Vector3.one;
                    bulletImpact.transform.position = hitInfo.point;

                    bulletImpact.transform.forward = hitInfo.normal;

                    //���� ���� ���� �±׿� �� �ʿ�
                    isAttackHit = true;
                    //���� enemy���, ������ ��
                    //���� construct��� ������ ��

                }
                else
                {
                    //���
                    //���� ���� �ʾҴ�.
                    isAttackHit = false;
                }
            }
        }

    }

    void OffFire()
    {
        canFire = true;
    }

    void bulletRulletCharge()
    {        
        leftBulletRullet += maxBulletRullet;
    }



    #region fire���� UIǥ�� ������Ƽ
    public int LEFT_BULLET
    {
        get { return leftBullet; }
        set 
        { 
            leftBullet = value;
            leftBulletText.text = $"{leftBullet}";
        }
    }

    public int LEFT_BULLETRULLET
    {
        get { return leftBulletRullet; }
        set
        {
            leftBulletRullet = value;
            bulletRulletText.text = $"{leftBulletRullet}";
        }
    }

    #endregion
}
