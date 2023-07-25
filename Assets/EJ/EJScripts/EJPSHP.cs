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
            print("플레이어가 죽었습니다");
            EJPlayerModelChangeState.instance.OFFArmModel();
            EJPlayerModelChangeState.instance.ONFullBodyModel();

            characterAnim.SetTrigger("Death");

            Invoke(nameof(Rebirth), 5);
        }
    }

    Vector3 respawnPoint;

    public void Rebirth()
    {
        //Model을 armbody로 다시 바꾼다
        EJPlayerModelChangeState.instance.ONArmModel();
        EJPlayerModelChangeState.instance.OFFFullBodyModel();

        //리스폰 위치 나중에 수정
        respawnPoint = new Vector3(-24.3f, 11, -94.7f);
        transform.position = respawnPoint;

        //HP를 회복시켜 둔다.
        HP = normalMaxHP;
        print("다시 태어났습니다");
    }
    
}
