using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EJPSHP : MonoBehaviour
{
    //public EJPlayerCharacterAnim stateMgr;
    public static EJPSHP instance;
    public Animator characterAnim;

    private void Awake()
    {
        instance = this;
    }

    public float currentHP = 125;

    public float normalMaxHP = 125;
    public float overcuredHP = 185;
    public float emergencyOvercuredHP = 158;

    public TextMeshProUGUI hpText;

    // Start is called before the first frame update
    void Start()
    {
        HP = normalMaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        RestrictHP();
        Dead();
    }


    public float HP
    {
        get
        {
            return currentHP;
        }
        set
        {
            currentHP = value;
            hpText.text = $"{value}";
        }
    }

    public Vector3 murdererPos;
    public void SetHP(int damage, Vector3 position)
    {
        if (HP > 0)
        {
            HP -= damage;
            murdererPos = position;
        }
    }

    public void RestrictHP()
    {
        if (HP < 0)
        {
            HP = 0;
        }
        if (HP > overcuredHP)
        {
            HP = overcuredHP;
        }
    }

    public void Dead()
    {
        if (EJPSHP.instance.HP <= 0)
        {
            print("�÷��̾ �׾����ϴ�");
            //EJPlayerModelChangeState.instance.OFFArmModel();
            //EJPlayerModelChangeState.instance.ONFullBodyModel();

            characterAnim.SetTrigger("Death");

            Invoke(nameof(Rebirth), 5);
        }
    }

    Vector3 respawnPoint;

    public void Rebirth()
    {
        //Model�� armbody�� �ٽ� �ٲ۴�
        //EJPlayerModelChangeState.instance.ONArmModel();
        //EJPlayerModelChangeState.instance.OFFFullBodyModel();

        //������ ��ġ ���߿� ����
        respawnPoint = new Vector3(-24.3f, 11, -94.7f);
        transform.position = respawnPoint;

        //HP�� ȸ������ �д�.
        HP = normalMaxHP;
        print("�ٽ� �¾���ϴ�");
    }

}