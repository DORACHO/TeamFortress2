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

    public int normalHP = 125;
    public int maxNormalHP = 125;
    
    public int overcuredHP = 185;
    public int emergencyOvercuredHP = 158;

    public TextMeshProUGUI hpText;

    // Start is called before the first frame update
    void Start()
    {
        HP = maxNormalHP;
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    void restrictHP()
    {
        if (HP > overcuredHP)
        {
            HP = overcuredHP;
        }

    }

    public int HP
    {
        get
        {
            return normalHP;
        }
        set
        {
            normalHP = value;
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

    public void Rebirth()
    {
        HP = maxNormalHP;
        print("�ٽ� �¾���ϴ�");
    }

    public void Dead()
    {
        if (EJPSHP.instance.HP < 0)
        {
            print("�÷��̾ �׾����ϴ�");
            characterAnim.SetTrigger("Death");
        }
    }

}
