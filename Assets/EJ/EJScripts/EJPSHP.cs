using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EJPSHP : MonoBehaviour
{
    //public EJPlayerCharacterAnim stateMgr;
    public static EJPSHP instance;
    public Animator characterAnim;

    public GameObject playerRespawnPoint;

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
       
 /*       if (HP <= 0)
        {
            GetComponentInChildren<EJCameraRotate>().Whoareyou();
        }*/


        //medic호출
        if (Input.GetKeyDown(KeyCode.M))
        {
            EJSFX.instance.CallMedicSFX();
        }
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
     
            print("플레이어가 죽었습니다");
            EJPlayerModelChangeState.instance.OFFArmModel();
            EJPlayerModelChangeState.instance.ONFullBodyModel();

            characterAnim.SetTrigger("Death");

            Invoke(nameof(Rebirth), 5);

    }


    public void Rebirth()
    {
        //Model을 armbody로 다시 바꾼다
        //EJPlayerModelChangeState.instance.ONArmModel();
        //EJPlayerModelChangeState.instance.OFFFullBodyModel();
        HP = normalMaxHP;

        GetComponentInChildren<EJCameraRotate>().TeleportZoomIn();
        GetComponentInChildren<EJCameraRotate>().TeleportDissolve();

        //리스폰 위치 나중에 수정
        transform.position = playerRespawnPoint.transform.position + Vector3.up*3;
        print("다시 태어났습니다");

        //HP를 회복시켜 둔다.
    }
    
}
