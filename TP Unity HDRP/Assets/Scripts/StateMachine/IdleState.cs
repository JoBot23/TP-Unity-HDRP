using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public ChaseState chaseState;
    public bool canSeePlayer = false;

    public override State RunCurrentState()
    {
        if(canSeePlayer) 
        {
            return chaseState;
        }
        else
        {
            return this;
        }
    }
}
