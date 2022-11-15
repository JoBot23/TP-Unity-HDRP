using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public ChaseState chaseState;
    PatrickController papate;

    void Start() 
    {
        papate = PatrickController.instance;    
    }

    public override State RunCurrentState()
    {
        if(papate.canSeePlayer) 
        {
            papate.ChasingSight();
            return chaseState;
        }
        else
        {
            return this;
        }
    }
}
