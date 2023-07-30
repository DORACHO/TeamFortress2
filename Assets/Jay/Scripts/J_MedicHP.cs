using MedicAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//태어날 때 체력이 최대체력이 되게하고싶다
//총에 맞으면 체력을 1감소하고싶다
//체력이 변경되면 UI로 표현하고싶다
public class J_MedicHP : MonoBehaviour
{
    public static J_MedicHP instance;
    public Animator anim;

    public GameObject respawnPoint;

    private void Awake()
    {
        instance = this;
    }
    int hp;
    private readonly int maxHp = 150;

    public int HP
    {
        get { return hp; }
        set
        {
            hp = value;
            //체력이 변경되면 UI로 표현하고싶다

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    //공격당함
    public void DamageProcess(int damage = 1)
    {

        //J_Medic1.instance.DamageProcess(damage);

        //if (state == State.Die)
        //{
        //    return;
        //}
        //agent.isStopped = true;
        //medicHP.HP -= 1;
        //if (medicHP.HP < 0)
        //{

        //    state = State.Die;

        //    Destroy(gameObject, 5);
        //    anim.SetTrigger("Die");

        //    Collider col = GetComponentInChildren<Collider>();
        //    if (col)
        //    {
        //        col.enabled = false;
        //    }
        //}
        //else
        //{
        //    state = State.Chase;
        //    agent.isStopped = false;
        //    anim.SetTrigger("Move");
        //}

    }

}
