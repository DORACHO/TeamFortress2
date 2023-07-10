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
    

}
