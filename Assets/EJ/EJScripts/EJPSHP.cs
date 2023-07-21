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

    public int hp = 125;
    //public int maxNormalHP = 125;
    public int maxNormalHP = Mathf.Clamp(125, 0, 185);
    public int overcuredHP = Mathf.Clamp(125, 0, 185);

    //public int overcuredHP = 185;
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
        RestrictHP();
    }


    public int HP
    {
        get
        {
            return hp;

        }
        set
        {
            hp = value;
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
        print("다시 태어났습니다");
    }

    public void RestrictHP()
    {
        if (HP < 0)
        {
            HP = 0;
        }
        if (HP > 185)
        {
            HP = 185;
        }
    }

    public void Dead()
    {
        if (EJPSHP.instance.HP < 0)
        {
            print("플레이어가 죽었습니다");
            characterAnim.SetTrigger("Death");
        }
    }

}
