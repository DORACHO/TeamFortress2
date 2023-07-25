using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJPlayerAnim : MonoBehaviour
{
    public Animator anim;


    //player State
    public enum State
    {
        Fire,
        Reload,
        Construct
    }

    public State state;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //���°� �ٲ� �ʱ�ȭ ���� �ϴ� �Լ� 
    public void ChangeState(State s)
    {
        //���࿡ ���� ���°� �ٲٷ��� ���¶� ������ ��ȯ
        if (state == s) return;

        state = s;

        switch (state)
        {
            case State.Fire:   
                if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                anim.SetTrigger("Idle");              
                break;
            case State.Reload:
                anim.SetTrigger("ReLoad");
                break;
            case State.Construct:
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Construct"))
                anim.SetTrigger("Construct");             
                break;
        }
    }


}

