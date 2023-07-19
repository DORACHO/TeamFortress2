using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//태어날 때 체력이 최대체력이 되게하고싶다
//총에 맞으면 체력을 1감소하고싶다
//체력이 변경되면 UI로 표현하고싶다
public class J_MedicHP : MonoBehaviour
{
    int hp;
    public int maxHP = 150;
   // public Slider sliderHP;

    public int HP
    {
        get { return hp; }
        set 
        {
            hp = value;
            //체력이 변경되면 UI로 표현하고싶다
            //sliderHP.value = hp;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
