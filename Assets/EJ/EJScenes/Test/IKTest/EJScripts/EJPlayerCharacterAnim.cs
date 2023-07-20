using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJPlayerCharacterAnim : MonoBehaviour
{
    public Animator characterAnim;

    public enum State
    {
        Idle,
        Die
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

    public void ChangeState(State s)
    {
        if (state == s) return;

        state = s;

        switch (state)
        {
            case State.Idle:
                characterAnim.SetTrigger("Idle");
                break;
            case State.Die:
                characterAnim.SetTrigger("Death");
                break;
        }

    }
}
