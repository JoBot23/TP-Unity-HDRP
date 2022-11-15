using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public AttackState attackState;
    public SearchingState seekingState;
    public bool isInAttackRange = false;
    private Vector3 lastSeen;

    PatrickController papate;

    void Start() 
    {
        papate = PatrickController.instance;    
    }

    public override State RunCurrentState()
    {
        if(isInAttackRange)
        {
            return attackState;
        }
        else
        {
            if(papate.target)
            {
                if(!papate.chasing) papate.chasing = true;
                lastSeen = papate.target.transform.position;
            }
            else 
            {
                seekingState.StartSeeking();
                return seekingState;
            }

            papate.agent.SetDestination(lastSeen);
            return this;
        }
    }
}
