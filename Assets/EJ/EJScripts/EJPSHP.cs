using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EJPSHP : MonoBehaviour
{
    public static EJPSHP instance;

    private void Awake()
    {
        instance = this;
    }

    public int normalHP = 125;
    public int overcuredHP = 185;
    public int emergencyOvercuredHP = 158;

    public TextMeshProUGUI hpText;

    // Start is called before the first frame update
    void Start()
    {
        HP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        
        
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

            //계산하기
            //방향 빼서 구하기
            //계산해서 해보기
            //normalize
            murdererPos = position;
        }

    }

}
