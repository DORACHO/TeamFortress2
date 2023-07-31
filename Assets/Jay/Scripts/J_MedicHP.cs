using MedicAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class J_MedicHP : MonoBehaviour
{
    public J_Medic1 stateMgr;
    public static J_MedicHP instance;
    public Animator anim;
    //public GameObject respawnPoint;
    public UnityEngine.AI.NavMeshAgent agent;

    //Ik 비활성화
    public GameObject IKLeftHand;
    public GameObject IKRightHand;

    private void Awake()
    {
        instance = this;
    }

    int hp = 150;
    private readonly int maxHp = 189;

    public int HP
    {
        get { return hp; }
        set
        {
            hp = value;
            // When the physical strength changes, I want to express it in the UI
            // Insert code here to update UI for HP change
            if (hp <= 0)
            {

                //Die();
            }
        }
    }

    public bool IsDead
    {
        get { return stateMgr.state == J_Medic1.State.Die; }
    }

    void Update()
    {
       

        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            DamageProcess(1000);
        }
    }
    void Die()
    {
        stateMgr.state = J_Medic1.State.Die;

        if (!IsDead)
        {
            anim.SetTrigger("Die");
        }

        
    }

    public void OnIK()
    {
        IKLeftHand.SetActive(true);
        IKRightHand.SetActive(true);
    }
    public void DamageProcess(int damage = 1)
    {
        if (stateMgr.state == J_Medic1.State.Die)
        {
            return;
        }

        agent.isStopped = true;
        HP -= damage;

        if (HP <= 0)
        {

            IKLeftHand.SetActive(false);
            IKRightHand.SetActive(false);
            stateMgr.state = J_Medic1.State.Die;
            //Destroy(gameObject, 5);

            anim.SetTrigger("Die");
            this.GetComponent<NavMeshAgent>().enabled = false;

            //Collider col = GetComponentInChildren<Collider>();
            //if (col)
            //{
            //    col.enabled = false;
            //}

        }
        else
        {
            stateMgr.state = J_Medic1.State.Chase;
            agent.isStopped = false;
            anim.SetTrigger("Move");
        }
    }
}
