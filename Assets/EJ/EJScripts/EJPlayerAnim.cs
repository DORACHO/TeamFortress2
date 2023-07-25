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


    //상태가 바뀔때 초기화 설정 하는 함수 
    public void ChangeState(State s)
    {
        //만약에 현재 상태가 바꾸려는 상태랑 같으면 반환
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

