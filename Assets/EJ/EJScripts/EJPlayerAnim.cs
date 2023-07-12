using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJPlayerAnim : MonoBehaviour
{
    public Animator anim;

    //player State
    public enum State
    {
        Idle, // ¡„∞Ì ¿÷¥¬ ∞‘ πŸ≤Ò
        Move,
        Fire, // ¿Â¿¸ sub, √— change
        Damage,
        Death
    }

    public State state;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Move:
                break;
            case State.Fire:
                break;
            case State.Damage:
                break;
            case State.Death:
                break;
        }
    }
}
