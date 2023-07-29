using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJPlayerAnim4fullbody : MonoBehaviour
{
    public Animator anim4Fullbody;

    public enum State
    {
        Idle,
        Dead
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

    public void ChangeState4Fullbody(State s)
    {
        if (state == s) return;

        state = s;

        switch (state)
        {
            case State.Idle:
                anim4Fullbody.SetTrigger("Idle");
                break;
            case State.Dead:
                anim4Fullbody.SetTrigger("Dead");
                break;
        }

    }
}
