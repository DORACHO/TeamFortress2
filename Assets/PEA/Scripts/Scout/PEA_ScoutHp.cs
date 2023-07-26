using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PEA_ScoutHp : MonoBehaviour
{
    private int hp = 0;

    private readonly int maxHp = 125;

    private PEA_ScoutMove scoutMove;
    private PEA_ScoutMove_Standby scoutStandby;

    // Start is called before the first frame update
    void Start()
    {
        if(TryGetComponent<PEA_ScoutMove>(out scoutMove))
        {
            
        }
        else
        {
            TryGetComponent<PEA_ScoutMove_Standby>(out scoutStandby);
        }

        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 공격 당함
    public void Damage(int damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            if(scoutMove != null)
            {
                scoutMove.Die();
            }
            else
            {
                scoutStandby.Die();
            }
        }
    }
}
