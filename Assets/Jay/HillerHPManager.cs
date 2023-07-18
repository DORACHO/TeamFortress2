using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillerHPManager : MonoBehaviour
{
    public int HillerHP;
    public int maxHP = 225;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HillerHP = Mathf.Clamp(HillerHP, 0, maxHP);

        if(HillerHP <= 0)
        {

        }
    }
    void DecreaseHP(int amount)
    {
        HillerHP -= amount;
    }

    void IncreaseHP(int amount)
    {
        HillerHP += amount;
    }
}
