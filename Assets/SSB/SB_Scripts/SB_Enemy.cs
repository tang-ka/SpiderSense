using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum State
    {
        Idle,
        Move,
        Attack,
        Die
    }

    State state;

    
    void Start()
    {
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if( state == State.Idle)
        {
            //UpdateIdle();
        }
    }
}
